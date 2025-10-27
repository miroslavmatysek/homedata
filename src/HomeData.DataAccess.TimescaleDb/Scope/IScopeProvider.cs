using HomeData.DataAccess.TimescaleDb.Scope;

namespace HomeData.DataAccess.TimescaleDb.Scope;

public interface IScopeProvider
{
    ITransactionScope Create();

    ITransactionScope CreateTransaction();

    ITransactionScope Create(int commandTimeout);

    ITransactionScope CreateTransaction(int commandTimeout);
}