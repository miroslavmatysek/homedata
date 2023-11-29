using System.Collections.Concurrent;
using HomeData.Model.TaskSetup;
using Microsoft.Extensions.Logging;

namespace HomeData.Service;

public class TaskServiceProvider : ITaskServiceProvider
{
    private readonly ILogger<TaskServiceProvider> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Guid, ITaskService> _services;
    private readonly object _servicesLock;

    public TaskServiceProvider(ILogger<TaskServiceProvider> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _services = new ConcurrentDictionary<Guid, ITaskService>();
        _servicesLock = new object();
    }

    public T Get<T>(ITaskSetup setup) where T : ITaskService
    {
        var type = typeof(T);

        _logger.LogDebug("Try to get service of type: {Type} for id: {Id}", type.Name, setup.Id);

        if (_services.TryGetValue(setup.Id, out var result))
        {
            _logger.LogDebug("Service of type: {Type} was cached for id: {Id}", type.Name, setup.Id);
            return (T)result;
        }

        lock (_servicesLock)
        {
            result = (T)_serviceProvider.GetService(type);

            if (result == null)
                throw new NotSupportedException($"Type: {type.Name} is not supported for id: {setup.Id}");

            result.Init(setup);
            _services.TryAdd(setup.Id, result);

            _logger.LogInformation("Service of type: {Type} was created for id: {Id}", type.Name, setup.Id);
            return (T)result;
        }
    }

    public void Dispose()
    {
        _logger.LogDebug("Try to disposed all task services");

        foreach (var key in _services.Keys)
        {
            if (_services.TryRemove(key, out var item))
            {
                item.Dispose();
                _logger.LogDebug("Service for id: {Id} was disposed", key);
            }
        }
    }
}