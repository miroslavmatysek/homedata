using DryIoc;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace HomeData.Tasks.Manager;

public class QuartzJobFactory : IJobFactory, IDisposable
{
    private readonly Container _container;
    private readonly Dictionary<string, IResolverContext> _scopes;

    public QuartzJobFactory(Container container)
    {
        _container = container;
        _scopes = new Dictionary<string, IResolverContext>();
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var result = GetScope($"{bundle.JobDetail.Key.Group}-{bundle.JobDetail.Key.Name}")
            .Resolve(bundle.JobDetail.JobType);

        if (result is IJobTask { IsInit: false } jobTask)
        {
            jobTask.Init(_container.Resolve<ILogger>());
            return jobTask;
        }
        return (IJob)result;
    }

    private IResolverContext GetScope(string key)
    {
        if (!_scopes.ContainsKey(key))
        {
            _scopes.Add(key, _container.OpenScope(key));
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