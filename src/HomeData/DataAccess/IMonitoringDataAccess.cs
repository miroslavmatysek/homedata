using HomeData.DataAccess.Model;

namespace HomeData.DataAccess;

public interface IMonitoringDataAccess
{
    IMeasurementFieldContainer Create(DateTime time);
    Task WritePointAsync(string field, int value, DateTime time);

    Task WritePointAsync(IMeasurementFieldContainer points);
}