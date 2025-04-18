using HomeData.DataAccess.TimescaleDb.Model;
using HomeData.DataAccess.TimescaleDb.Scope;
using HomeData.Model;
using Npgsql;

namespace HomeData.DataAccess.TimescaleDb.Dao.Npgsql;

public class NpgsqlMeasurementDao : IMeasurementDao
{
    private const string InsertQuery = "INSERT INTO {0} ({1}) VALUES ({2})";
    
    public async Task WriteMeasurementAsync(ICommandScope scope, string table, MeasureItem[] data)
    {
        string columns = string.Join(",", data.Select(d => d.Field));
        string parametersNames = string.Join(",", data.Select(d => $"@{d.Field}"));
        string query = string.Format(InsertQuery, table, columns, parametersNames);
        var parameters = data.Select(i => new NpgsqlParameter(i.Field, i.ItemValue)).ToArray();
        await scope.ExecuteAsync(query, parameters);
    }
}