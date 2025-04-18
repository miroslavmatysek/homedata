using System.Data;
using Npgsql;

namespace HomeData.DataAccess.TimescaleDb.Scope.Npgsql;

public class NpgsqlCommandScope : ICommandScope
{
    public NpgsqlCommandScope(NpgsqlConnection connection, int commandTimeout)
    {
        Connection = connection;
        Transaction = null;
        CommandTimeout = commandTimeout;
    }

    protected NpgsqlConnection Connection { get; }

    protected NpgsqlTransaction Transaction { get; set; }
    
    protected int CommandTimeout { get; set; }

    public async Task<int> ExecuteAsync(string query, params NpgsqlParameter[] parameters)
    {
        await using var command = Connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddRange(parameters);
        return await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }
    public IBulkInsertScope CreateBulkInsert(string insertQuery)
    {
        return new NpgsqlCopyBulkInsertScope(Connection.BeginBinaryImport(insertQuery));
    }
}