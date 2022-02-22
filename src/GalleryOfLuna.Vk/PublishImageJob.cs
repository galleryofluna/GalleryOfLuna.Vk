using GalleryOfLuna.Vk.Configuration;
using GalleryOfLuna.Vk.Derpibooru.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk
{
    public class PublishImageJob : IJob
    {
        private readonly Target _target;
        private readonly DerpibooruDbContext _derpibooru;
        private readonly ILogger<PublishImageJob> _logger;

        public PublishImageJob(Target target, DerpibooruDbContext derpibooru, ILogger<PublishImageJob> logger)
        {
            _target = target;
            _derpibooru = derpibooru;
            _logger = logger;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var images = await _derpibooru.Images
                .AsNoTracking()
                .Include(d => d.ImageTaggings)
                .ThenInclude(d => d.Tag)
                .OrderByDescending(i => i.CreatedAt)
                .Select(image => new
                {
                    image.Id,
                    Tags = image.ImageTaggings.Select(x => x.Tag.Name)
                })
                .Take(5)
                .ToListAsync(cancellationToken);

            foreach (var image in images)
                _logger.LogDebug("{id} with tags {tags}", image.Id,
                    string.Join(", ", image.Tags));
        }
    }
}