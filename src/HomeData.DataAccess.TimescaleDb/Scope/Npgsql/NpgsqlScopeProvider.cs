using HomeData.DataAccess.TimescaleDb.Scope;
using Npgsql;

namespace HomeData.DataAccess.TimescaleDb.Scope.Npgsql;

public class NpgsqlScopeProvider : IScopeProvider
{
    public NpgsqlScopeProvider(string connectionString)
        : this(connectionString, 30)
    {
    }

    public NpgsqlScopeProvider(string connectionString, int defaultCommandTimeout)
    {
        ConnectionString = connectionString;
        DefaultCommandTimeout = defaultCommandTimeout;
        if (!string.IsNullOrEmpty(connectionString))
        {
            var csb = new NpgsqlConnectionStringBuilder(connectionString);
            CatalogName = csb.Database;
        }
    }

    private int DefaultCommandTimeout { get; }

    private string ConnectionString { get; }

    public ITransactionScope Create(int commendTimeout)
    {
        var result = new NpgsqlTransactionScope(GetConnection(), commendTimeout);
        return result;
    }

    public ITransactionScope CreateTransaction(int commandTimeout)
    {
        var result = Create(commandTimeout);
        result.Begin();
        return result;
    }

    public string CatalogName { get; }

    public ITransactionScope Create()
    {
        return Create(DefaultCommandTimeout);
    }

    public ITransactionScope CreateTransaction()
    {
        return CreateTransaction(DefaultCommandTimeout);
    }

    private NpgsqlConnection GetConnection()
    {
        var result = new NpgsqlConnection(ConnectionString);
        result.Open();
        return result;
    }
}