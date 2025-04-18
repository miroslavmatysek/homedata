using HomeData.DataAccess.Model;
using HomeData.DataAccess.TimescaleDb.Dao;
using HomeData.DataAccess.TimescaleDb.Model;
using HomeData.DataAccess.TimescaleDb.Scope;
using HomeData.Model;
using Microsoft.Extensions.Logging;

namespace HomeData.DataAccess.TimescaleDb;

public class TimescaleDbMonitoringDataAccess : IMonitoringDataAccess
{
    private const string DateTimeMeasureItemField = "timestamp";
    
    private readonly ILogger<TimescaleDbMonitoringDataAccess> _logger;
    private readonly IScopeProvider _scopeProvider;
    private readonly IMeasurementDao _dao;
    private readonly string _tableName;

    public TimescaleDbMonitoringDataAccess(string bucket, string measurement,
        IScopeProvider scopeProvider, IMeasurementDao dao, ILogger<TimescaleDbMonitoringDataAccess> logger)
    {
        _logger = logger;
        _scopeProvider = scopeProvider;
        _dao = dao;
        _tableName = $"{bucket}_{measurement}";
    }


    public IMeasurementFieldContainer Create(DateTime time)
    {
        return new TimeScaleDbMeasurementFieldContainer(_tableName).With(new DateTimeMeasureItem(DateTimeMeasureItemField)
        {
            DateTime = time
        });
    }

    public async Task WritePointAsync(IMeasurementFieldContainer points)
    {
        if (points is TimeScaleDbMeasurementFieldContainer pointsData)
        {

            using var scope = _scopeProvider.CreateTransaction();
            await _dao.WriteMeasurementAsync(scope,_tableName, pointsData.Data);
            scope.Commit();
        }
        else
            _logger.LogWarning("Points can not be saved, wrong instance type");
    }
}