using HomeData.DataAccess.Influxdb.Model;
using HomeData.DataAccess.Model;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Logging;

namespace HomeData.DataAccess.Influxdb;

public class InfluxMonitoringDataAccess : IMonitoringDataAccess
{
    private readonly ILogger<InfluxMonitoringDataAccess> _logger;
    private readonly IInfluxDBClient _client;
    private readonly string _bucket;
    private readonly string _organization;
    private readonly string _measurement;

    public InfluxMonitoringDataAccess(IInfluxDBClient client, string bucket, string organization, string measurement, ILogger<InfluxMonitoringDataAccess> logger)
    {
        _client = client;
        _logger = logger;
        _bucket = bucket;
        _organization = organization;
        _measurement = measurement;
    }

    public IMeasurementFieldContainer Create(DateTime time)
    {
        var pd = PointData.Measurement(_measurement).Timestamp(time, WritePrecision.S);
        return new InfluxMeasurementFieldContainer(pd);
    }

    public async Task WritePointAsync(IMeasurementFieldContainer points)
    {
        _logger.LogTrace("Try to save points [{Points}]", points.Count);
        if (points is InfluxMeasurementFieldContainer pointsData)
        {
            await _client.GetWriteApiAsync().WritePointAsync(pointsData.Data, _bucket, _organization);
            _logger.LogDebug("Points were saved [Points: {Points}]", points.Count);
        }
        else
            _logger.LogWarning("Points can not be saved, wrong instance type");
    }
}