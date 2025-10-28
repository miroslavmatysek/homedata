using HomeData.DataAccess.TimescaleDb.Scope;
using HomeData.Model;
using Npgsql;
using NpgsqlTypes;

namespace HomeData.DataAccess.TimescaleDb.Dao.Npgsql;

public class NpgsqlMeasurementDao : IMeasurementDao
{
    private const string InsertQuery = "INSERT INTO {0} ({1}) VALUES ({2})";

    public async Task WriteMeasurementAsync(ICommandScope scope, string table, MeasureItem[] data)
    {
        var columns = string.Join(",", data.Select(d => d.Field));
        var parametersNames = string.Join(",", data.Select(d => $"@{d.Field}"));
        var query = string.Format(InsertQuery, table, columns, parametersNames);
        var parameters = data.Select(i => new NpgsqlParameter(i.Field, ToDbType(i.ValueType))
        {
            Value = i.ItemValue
        }).ToArray();
        await scope.ExecuteAsync(query, parameters);
    }

    private static NpgsqlDbType ToDbType(MeasureItemValueType itemType) =>
        itemType switch
        {
            MeasureItemValueType.DateTime => NpgsqlDbType.TimestampTz,
            MeasureItemValueType.Decimal => NpgsqlDbType.Numeric,
            MeasureItemValueType.Int => NpgsqlDbType.Integer,
            MeasureItemValueType.String => NpgsqlDbType.Varchar,
            MeasureItemValueType.Int64 => NpgsqlDbType.Bigint,
            MeasureItemValueType.Float => NpgsqlDbType.Double,
            _ => throw new ArgumentOutOfRangeException()
        };
}