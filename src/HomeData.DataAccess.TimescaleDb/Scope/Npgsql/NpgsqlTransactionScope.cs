using Npgsql;
using System.Data;
using HomeData.DataAccess.TimescaleDb.Scope;
using HomeData.DataAccess.TimescaleDb.Scope.Npgsql;

namespace HomeData.DataAccess.TimescaleDb.Scope.Npgsql;

public class NpgsqlTransactionScope : NpgsqlCommandScope, ITransactionScope
{
    private bool disposed;

    public NpgsqlTransactionScope(NpgsqlConnection connection, int commandTimeout)
        : base(connection, commandTimeout)
    {
        disposed = false;
        CommandTimeout = commandTimeout;
    }

    public void Begin()
    {
        Transaction = Connection.BeginTransaction();
    }

    public void Commit()
    {
        if (Transaction != null) Transaction.Commit();

        Transaction = null;
    }

    public void Rollback()
    {
        if (Transaction != null) Transaction.Rollback();

        Transaction = null;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Rollback();
                if (Connection != null) Connection.Close();
            }

            disposed = true;
        }
    }
}