using GalleryOfLuna.Vk.Configuration;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace GalleryOfLuna.Vk
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DatabaseConfiguration _databaseConfiguration;

        public Worker(IOptionsMonitor<DatabaseConfiguration> dbOptionsMonitor, ILogger<Worker> logger)
        {
            _databaseConfiguration = dbOptionsMonitor.Get(Sections.Derpibooru);
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning(_databaseConfiguration.ConnectionString);
                _logger.LogWarning("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(60 * 1000, stoppingToken);
            }
        }
    }
}