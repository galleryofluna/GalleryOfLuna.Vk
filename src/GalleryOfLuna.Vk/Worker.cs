using GalleryOfLuna.Vk.Configuration;
using GalleryOfLuna.Vk.Derpibooru.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk
{
    public class Worker : BackgroundService
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<Worker> _logger;
        private readonly DatabaseConfiguration _databaseConfiguration;

        public Worker(
            HealthCheckService healthCheckService,
            IServiceScopeFactory serviceScopeFactory,
            IOptionsMonitor<DatabaseConfiguration> dbOptionsMonitor,
            ILogger<Worker> logger)
        {
            _databaseConfiguration = dbOptionsMonitor.Get(Sections.Derpibooru);
            _healthCheckService = healthCheckService;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var healthReport = await _healthCheckService.CheckHealthAsync(stoppingToken);
                while (healthReport.Status != HealthStatus.Healthy)
                {
                    _logger.LogWarning("Heatlhcheck status is {status}. Sleep for a minute", healthReport.Status);
                    await Task.Delay(60000, stoppingToken);
                    healthReport = await _healthCheckService.CheckHealthAsync(stoppingToken);
                }

                await using var scope = _serviceScopeFactory.CreateAsyncScope();
                var derpibooru = scope.ServiceProvider.GetRequiredService<DerpibooruDbContext>();
                var images = await derpibooru.Images
                    .Include(d => d.ImageTaggings)
                    .ThenInclude(d => d.Tag)
                    .OrderByDescending(i => i.CreatedAt)
                    .Take(10)
                    .ToListAsync();

                foreach (var image in images)
                    _logger.LogError("{id} with tags {tags}", image.Id,
                        string.Join(", ", image.ImageTaggings.Select(x => x.Tag).Select(x => x.Name)));
                _logger.LogWarning(_databaseConfiguration.ConnectionString);
                _logger.LogWarning("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(60 * 1000, stoppingToken);
            }
        }
    }
}