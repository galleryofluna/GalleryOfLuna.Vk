using GalleryOfLuna.Vk.Configuration;
using GalleryOfLuna.Vk.Derpibooru.EntityFramework;
using GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model;
using GalleryOfLuna.Vk.Publishing.EntityFramework;
using GalleryOfLuna.Vk.Publishing.EntityFramework.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using System.Data;

namespace GalleryOfLuna.Vk
{
    public class PublishImageJob : IJob
    {
        private readonly Target _target;
        private readonly PublishingDbContext _publishingDbContext;
        private readonly DerpibooruDbContext _derpibooru;
        private readonly HttpClient _httpClient;
        private readonly VkConfiguration _vkConfiguration;
        private readonly VkClient _vkClient;
        private readonly ILogger<PublishImageJob> _logger;

        public PublishImageJob(
            Target target,
            PublishingDbContext publishingDbContext,
            DerpibooruDbContext derpibooru,
            HttpClient httpClient,
            IOptions<VkConfiguration> vkConfiguration,
            VkClient vkClient,
            ILogger<PublishImageJob> logger)
        {
            _target = target;
            _publishingDbContext = publishingDbContext;
            _derpibooru = derpibooru;
            _httpClient = httpClient;
            _vkConfiguration = vkConfiguration.Value;
            _vkClient = vkClient;
            _logger = logger;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            await using var transaction = await _publishingDbContext.Database.BeginTransactionAsync(
                IsolationLevel.Serializable,
                cancellationToken);

            var image = await FindImageAsync(cancellationToken);

            await using var imageStream = await OpenStreamAsync(image);

            await PublishImageAsync(image, imageStream, cancellationToken);

            await MarkImageAsPublished(image, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

        private async Task<Image> FindImageAsync(CancellationToken cancellationToken = default)
        {
            var publishedIds = await _publishingDbContext.PublishedImages.Select(image => long.Parse(image.ImageId))
                .ToListAsync(cancellationToken);

            var tags = _target.Tags.Union(_target.ExcludedTags);
            var tagIds = await _derpibooru.Tags
                .Where(tag => tags.Contains(tag.Name))
                .Select(tag => new { tag.Id, tag.Name })
                .ToDictionaryAsync(tag => tag.Name, tag => tag.Id, cancellationToken);

            var includedTagIds = _target.Tags.Select(tag => tagIds[tag]).ToArray();
            var excludedTagIds = _target.ExcludedTags.Select(tag => tagIds[tag]).ToArray();

            var query = _derpibooru.Images
                .AsNoTracking()
                .Include(d => d.User)
                .Include(d => d.ImageTaggings)
                .ThenInclude(d => d.Tag)
                .OrderByDescending(d => d.WilsonScore)
                .Where(image => !publishedIds.Contains(image.Id))
                .Where(image => image.Score > _target.Threshold)
                // TODO: Support animated formats
                .Where(image => image.ImageFormat != "webm")
                .Where(image => image.ImageFormat != "gif");

            if (_target.Until.HasValue)
                query = query.Where(image => image.CreatedAt < _target.Until.Value);

            if (_target.After.HasValue)
                query = query.Where(image => image.CreatedAt > _target.After.Value);

            if (includedTagIds.Any())
            {
                foreach (var includedTagId in includedTagIds)
                    query = query.Where(image => image.ImageTaggings.Select(x => x.TagId).Contains(includedTagId));
            }

            if (excludedTagIds.Any())
            {
                foreach (var excludedTagId in excludedTagIds)
                    query = query.Where(image => !image.ImageTaggings.Select(x => x.TagId).Contains(excludedTagId));
            }

            var image = await query.FirstAsync(cancellationToken);

            return image;
        }

        private async Task<Stream> OpenStreamAsync(Image image, CancellationToken cancellationToken = default)
        {
            const string derpiCdnBaseUrl = "https://derpicdn.net/img/";
            var d = image.CreatedAt;
            var imageFormat = GetImageFormat(image);

            // lib\philomena_web\views\image_view.ex
            // Be careful with that
            var requestUriBuilder = new UriBuilder(derpiCdnBaseUrl);
            requestUriBuilder.Path += $"download/{d.Year}/{d.Month}/{d.Day}/{image.Id}.{imageFormat}";

            var requestUri = requestUriBuilder.ToString();

            return await _httpClient.GetStreamAsync(requestUri, cancellationToken);
        }

        private async Task PublishImageAsync(
            Image image,
            Stream imageStream,
            CancellationToken cancellationToken = default)
        {
            var uploadServerInfo =
                await _vkClient.GetWallUploadServerAsync(
                    _vkConfiguration.GroupId,
                    cancellationToken);

            var (server, photo, hash) = await _vkClient.UploadPhotoAsync(
                uploadServerInfo.UploadUrl,
                GetImageFormat(image),
                imageStream,
                cancellationToken);

            var savedPhotos = await _vkClient.SaveWallPhotoAsync(
                _vkConfiguration.GroupId,
                photo,
                server,
                hash,
                cancellationToken);

            var savedPhoto = savedPhotos.First();

            var message = GetMessage(image);
            var copyright = await GetCopyright(image);
            var attachments = $"photo{savedPhoto.OwnerId}_{savedPhoto.Id}";

            await _vkClient.Post(
                -_vkConfiguration.GroupId,
                message,
                attachments,
                copyright,
                cancellationToken);
        }

        private async Task<PublishedImage> MarkImageAsPublished(
            Image image,
            CancellationToken cancellationToken = default)
        {
            var publishedImage = new PublishedImage(DateTime.UtcNow, "Derpibooru", image.Id.ToString());
            _publishingDbContext.PublishedImages.Add(publishedImage);
            await _publishingDbContext.SaveChangesAsync(cancellationToken);

            return publishedImage;
        }

        private string GetImageFormat(Image image)
        {
            switch (image.ImageFormat)
            {
                case "svg":
                    return "png";

                default:
                    return image.ImageFormat;
            }
        }

        private string GetMessage(Image image) => 
@$"Автор {image.User?.Name ?? "Background Pony"}

Рейтинг: {image.Score}
Добавили в избранное: {image.Favorites}
#Derpibooru #GalleryOfLuna";

        private async Task<string> GetCopyright(Image image)
        {
            const string derpibooruBaseUrl = "https://derpibooru.org/";

            var imageSource = await _derpibooru.ImageSources.FirstOrDefaultAsync(x => x.ImageId == image.Id);
            if (!string.IsNullOrWhiteSpace(imageSource?.Source))
                return imageSource!.Source;

            return new UriBuilder(derpibooruBaseUrl)
            {
                Path = $"images/{image.Id}"
            }.ToString();
        }
    }
}