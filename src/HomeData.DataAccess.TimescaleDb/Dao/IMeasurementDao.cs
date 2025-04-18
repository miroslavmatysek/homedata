using HomeData.DataAccess.TimescaleDb.Scope;
using HomeData.Model;

namespace HomeData.DataAccess.TimescaleDb.Dao;

public interface IMeasurementDao
{
    Task WriteMeasurementAsync(ICommandScope scope, string table, MeasureItem[] data);
}