using GalleryOfLuna.Vk.Derpibooru.EntityFramework;
using GalleryOfLuna.Vk.Derpibooru.EntityFramework.Model;
using GalleryOfLuna.Vk.Publishing.EntityFramework;
using GalleryOfLuna.Vk.Publishing.EntityFramework.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk
{
    public class PublishImageJob : IJob
    {
        private readonly Target _target;
        private readonly PublishingDbContext _publishingDbContext;
        private readonly DerpibooruDbContext _derpibooru;
        private readonly ILogger<PublishImageJob> _logger;

        public PublishImageJob(
            Target target,
            PublishingDbContext publishingDbContext,
            DerpibooruDbContext derpibooru,
            ILogger<PublishImageJob> logger)
        {
            _target = target;
            _publishingDbContext = publishingDbContext;
            _derpibooru = derpibooru;
            _logger = logger;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            await using var transaction = await _publishingDbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            var publishedIds = await _publishingDbContext.PublishedImages.Select(image => long.Parse(image.ImageId))
                .ToListAsync(cancellationToken);

            var tags = _target.Tags.Union(_target.ExcludedTags);
            var tagIds = await _derpibooru.Tags
                .Where(tag => tags.Contains(tag.Name))
                .Select(tag => new {tag.Id, tag.Name})
                .ToDictionaryAsync(tag => tag.Name, tag => tag.Id, cancellationToken);

            var includedTagIds = _target.Tags.Select(tag => tagIds[tag]).ToArray();
            var excludedTagIds = _target.ExcludedTags.Select(tag => tagIds[tag]).ToArray();

            IQueryable<Image> query = _derpibooru.Images
                .AsNoTracking()
                .Include(d => d.ImageTaggings)
                .ThenInclude(d => d.Tag)
                .OrderByDescending(d => d.WilsonScore)
                .Where(image => !publishedIds.Contains(image.Id))
                .Where(image => image.Score > _target.Threshold);

            if (_target.Until.HasValue)
                query = query.Where(image => image.CreatedAt < _target.Until.Value);

            if (_target.After.HasValue)
                query = query.Where(image => image.CreatedAt > _target.After.Value);

            if (includedTagIds.Any())
                foreach (var includedTagId in includedTagIds)
                    query = query.Where(image => image.ImageTaggings.Select(x=>x.TagId).Contains(includedTagId));

            if (excludedTagIds.Any())
                foreach (var excludedTagId in excludedTagIds)
                    query = query.Where(image => !image.ImageTaggings.Select(x => x.TagId).Contains(excludedTagId));

            var images = await query
                .Select(image => new
                {
                    image.Id,
                    Tags = image.ImageTaggings.Select(x => x.Tag.Name)
                })
                .Take(5)
                .ToListAsync(cancellationToken);


            var publishedImages = images.Select(image => new PublishedImage(DateTime.UtcNow, "Derpibooru", image.Id.ToString()));
            _publishingDbContext.PublishedImages.AddRange(publishedImages);
            await _publishingDbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            foreach (var image in images)
                _logger.LogDebug("{id} with tags {tags}", image.Id,
                    string.Join(", ", image.Tags));
        }
    }
}