using GalleryOfLuna.Vk.Configuration;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

using System.Threading.Tasks.Dataflow;

namespace GalleryOfLuna.Vk
{
    public class Scheduler : BackgroundService
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptionsMonitor<TargetsConfiguration> _targetsOptionsMonitor;
        private readonly ILogger<Scheduler> _logger;

        private readonly PriorityQueue<Target, DateTime> _queue = new();
        private readonly SemaphoreSlim _queueMutex = new(1);
        private ActionBlock<JobInformation>? _processing;

        public Scheduler(
            HealthCheckService healthCheckService,
            IServiceProvider serviceProvider,
            IOptionsMonitor<TargetsConfiguration> targetsOptionsMonitor,
            ILogger<Scheduler> logger)
        {
            _healthCheckService = healthCheckService;
            _serviceProvider = serviceProvider;
            _targetsOptionsMonitor = targetsOptionsMonitor;
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            // TODO: Review this section. It needs while the database is restoring
            var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);
            while (healthReport.Status != HealthStatus.Healthy)
            {
                _logger.LogWarning("Heatlhcheck status is {status}. Sleep for a minute", healthReport.Status);
                await Task.Delay(60000, cancellationToken);
                healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);
            }

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ReloadTargets(_targetsOptionsMonitor.CurrentValue, stoppingToken);
            RegisterReloadTargetsOnChange(stoppingToken);
            _processing = CreateProcessingBlock(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _queueMutex.WaitAsync(stoppingToken);
                    if (_queue.TryPeek(out var target, out var priority))
                    {
                        if (priority < DateTime.UtcNow)
                        {
                            _processing.Post(JobInformation.Create<PublishImageJob>(target));
                            _queue.EnqueueDequeue(
                                target,
                                target.Schedule.GetNextOccurrence(DateTime.UtcNow) ?? DateTime.MaxValue);
                        }
                    }

                    await Task.Delay(100, stoppingToken);
                }
                finally
                {
                    _queueMutex.Release();
                }
            }

            _processing.Complete();
            await _processing.Completion;
        }

        private async Task ReloadTargets(
            TargetsConfiguration configuration,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogWarning("Reloading targets");
                await _queueMutex.WaitAsync(cancellationToken);

                var currentTargets = _queue.UnorderedItems.Select(x => x.Element).ToList();
                var newTargets = configuration.Select(Target.From).ToList();

                var removedTargets = currentTargets.Except(newTargets).Select(t => t.ToConfiguration());
                var addedTargets = newTargets.Except(currentTargets).Select(t => t.ToConfiguration());

                var range = newTargets.Select(target =>
                    (target, target.Schedule.GetNextOccurrence(DateTime.UtcNow) ?? DateTime.MaxValue));

                _queue.Clear();
                _queue.EnqueueRange(range);

                if (addedTargets.Any())
                    _logger.LogInformation("Next targets has been added to schedule: {items}", addedTargets);

                if (removedTargets.Any())
                    _logger.LogInformation("Next targets has been excluded from schedule: {items}", removedTargets);
            }
            finally
            {
                _queueMutex.Release();
            }
        }

        private void RegisterReloadTargetsOnChange(CancellationToken stoppingToken) =>
            _targetsOptionsMonitor.OnChange(async targets => await ReloadTargets(targets, stoppingToken));

        private ActionBlock<JobInformation> CreateProcessingBlock(CancellationToken stoppingToken) =>
            new(async jobInfo =>
                {
                    await using var scope = _serviceProvider.CreateAsyncScope();
                    var job = ActivatorUtilities.CreateInstance(
                                  scope.ServiceProvider,
                                  jobInfo.JobType,
                                  jobInfo.Parameters) as IJob
                              ?? throw new InvalidOperationException("Created job instance is null");
                    _logger.LogInformation("Job {jobType} starts execution", jobInfo.JobType.Name);
                    await job.Execute(stoppingToken)
                        .ContinueWith(LogJobCompletion(jobInfo), stoppingToken);
                },
                new ExecutionDataflowBlockOptions
                {
                    CancellationToken = stoppingToken,
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                });

        private Action<Task> LogJobCompletion(JobInformation jobInfo) => task =>
        {
            if (task.IsCompletedSuccessfully)
                _logger.LogInformation("Job {jobType} ended successfully", jobInfo.JobType.Name);

            if (task.IsFaulted)
                _logger.LogError(task.Exception, "Job {jobType} ended with an exception", jobInfo.JobType.Name);

            if (task.IsCanceled)
                _logger.LogWarning("Job {jobType} was cancelled", jobInfo.JobType.Name);
        };

        private class JobInformation
        {
            public Type JobType { get; }
            public object[] Parameters { get; }

            private JobInformation(Type jobType, params object[] parameters)
            {
                JobType = jobType;
                Parameters = parameters;
            }

            public static JobInformation Create<TJob>(params object[] parameters) where TJob : IJob =>
                new(typeof(TJob), parameters);
        }
    }
}