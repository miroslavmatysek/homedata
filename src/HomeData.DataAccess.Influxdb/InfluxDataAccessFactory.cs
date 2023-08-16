using HomeData.Model.Config;
using InfluxDB.Client;
using Microsoft.Extensions.Logging;

namespace HomeData.DataAccess.Influxdb;

public class InfluxDataAccessFactory : IDataAccessFactory
{
    private readonly ILogger<InfluxDataAccessFactory> _logger;
    private readonly DataAccessConfig _dataAccessConfig;

    public InfluxDataAccessFactory(ILogger<InfluxDataAccessFactory> logger, DataAccessConfig dataAccessConfig)
    {
        _logger = logger;
        _dataAccessConfig = dataAccessConfig;
    }


    public IMonitoringDataAccess Create(string bucket, string measurement)
    {
        var client = new InfluxDBClient(_dataAccessConfig.Url,_dataAccessConfig.ApiToken);
        return new InfluxMonitoringDataAccess(client, bucket, _dataAccessConfig.Organization, measurement);
    }
}