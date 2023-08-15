using HomeData.Tasks.Solax;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace HomeData.Tasks.Manager;

public class QuartzTaskManager : ITaskManager
{
    private const string DefaultGroup = "DefaultGroup";

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

        await AddJob<SolaxX3G4JobTask>();
    }

    private async Task AddJob<T>() where T : IJobTask
    {
        var id = nameof(T);
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity(id, DefaultGroup)
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(30)
                .RepeatForever())
            .Build();

        IJobDetail job = JobBuilder.Create<T>()
            .WithIdentity(id, DefaultGroup)
            .Build();

        await _scheduler.ScheduleJob(job, trigger);
    }

    public async Task StopAsync()
    {
        await _scheduler.Shutdown();
    }
}