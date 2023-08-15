using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace HomeData.Tasks.Manager;

public class QuartzTaskManager : ITaskManager
{
    private readonly ILogger<QuartzTaskManager> _logger;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;


        private IScheduler _scheduler;

        public QuartzTaskManager(ILogger<QuartzTaskManager> logger, ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        public async Task StartAsync()
        {
            await StartUpAsync();

            _ = _scheduler.Start();
        }

        public async Task RunAsync(CancellationToken ct)
        {
            await StartUpAsync();
            await _scheduler.Start(ct);
        }

        private async Task StartUpAsync()
        {
            _scheduler = await _schedulerFactory.GetScheduler();

            // Tell the scheduler to use the custom job factory
            _scheduler.JobFactory = _jobFactory;

        }

        public async Task StopAsync()
        {
            await _scheduler.Shutdown();
        }
}