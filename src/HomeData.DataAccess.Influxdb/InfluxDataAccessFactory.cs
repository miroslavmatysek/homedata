using HomeData.Model.Config;
using InfluxDB.Client;
using Microsoft.Extensions.Logging;

namespace HomeData.DataAccess.Influxdb;

public class InfluxDataAccessFactory : IDataAccessFactory
{
    private readonly ILogger<InfluxDataAccessFactory> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly DataAccessConfig _dataAccessConfig;

    public InfluxDataAccessFactory(ILogger<InfluxDataAccessFactory> logger, DataAccessConfig dataAccessConfig,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _dataAccessConfig = dataAccessConfig;
    }

    public IMonitoringDataAccess Create(string bucket, string measurement)
    {
        _logger.LogDebug("Try to create new monitoring data access for: [Bucket: {Bucket}, Measurement: {Measurement}]",
            bucket, measurement);
        var client = new InfluxDBClient(_dataAccessConfig.Url, _dataAccessConfig.ApiToken);

        var result = new InfluxMonitoringDataAccess(client, bucket, _dataAccessConfig.Organization, measurement,
            (ILogger<InfluxMonitoringDataAccess>)_serviceProvider.GetService(
                typeof(ILogger<InfluxMonitoringDataAccess>))! ?? throw new InvalidOperationException());

        _logger.LogDebug("Created new monitoring data access for: [Bucket: {Bucket}, Measurement: {Measurement}]",
            bucket, measurement);
        return result;
    }
}