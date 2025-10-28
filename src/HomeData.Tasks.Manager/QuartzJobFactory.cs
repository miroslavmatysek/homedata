using HomeData.DataAccess;
using HomeData.Model.Config;
using HomeData.Provider.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace HomeData.Tasks.Manager;

public class QuartzJobFactory : IJobFactory, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, IServiceScope> _scopes;

    public QuartzJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _scopes = new Dictionary<string, IServiceScope>();
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var result = (IJobTask)GetScope($"{bundle.JobDetail.Key.Group}-{bundle.JobDetail.Key.Name}")
            .ServiceProvider.GetRequiredService(bundle.JobDetail.JobType);

        if (!result.IsInit)
        {
            TaskConfig config = GetTaskConfig(bundle.JobDetail.JobType.Name);
            if (config == null)
                throw new ArgumentNullException($"Task: {bundle.JobDetail.JobType.Name} was not configured");

            var dataAccessFactory = _serviceProvider.GetRequiredService<IDataAccessFactory>();
            result.Init(_serviceProvider.GetRequiredService<ILogger>(),
                dataAccessFactory.Create(config.Bucket, config.Measurement),
                config, config.UtcTime ? new UtcDateTimeProvider() : new LocalTimeProvider());
        }

        return result;
    }

    private TaskConfig GetTaskConfig(string jobTypeName)
    {
        var key = jobTypeName.Replace("JobTask", "").ToLower();
        var allConfig = _serviceProvider.GetRequiredService<TasksConfig>();
        return allConfig.Items.FirstOrDefault(j => key.Equals(j.Name.ToLower()));
    }

    private IServiceScope GetScope(string key)
    {
        if (!_scopes.ContainsKey(key))
        {
            _scopes.Add(key, _serviceProvider.CreateScope());
        }

        return _scopes[key];
    }

    public void ReturnJob(IJob job)
    {
        // i couldn't find a way to release services with your preferred DI,
        // its up to you to google such things
    }

    public void Dispose()
    {
        foreach (var scope in _scopes.Values)
        {
            scope.Dispose();
        }
    }
}