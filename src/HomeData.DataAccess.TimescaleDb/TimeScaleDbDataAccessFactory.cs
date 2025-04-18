using HomeData.DataAccess.TimescaleDb.Dao;
using HomeData.DataAccess.TimescaleDb.Scope;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HomeData.DataAccess.TimescaleDb;

public class TimeScaleDbDataAccessFactory : IDataAccessFactory
{
    private readonly ILogger<TimeScaleDbDataAccessFactory> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TimeScaleDbDataAccessFactory(ILogger<TimeScaleDbDataAccessFactory> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public IMonitoringDataAccess Create(string bucket, string measurement)
    {
        return new TimescaleDbMonitoringDataAccess(bucket, measurement,
            _serviceProvider.GetRequiredService<IScopeProvider>(),
            _serviceProvider.GetRequiredService<IMeasurementDao>(),
            _serviceProvider.GetRequiredService<ILogger<TimescaleDbMonitoringDataAccess>>());
    }
}