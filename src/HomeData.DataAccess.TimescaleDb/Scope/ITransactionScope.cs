namespace HomeData.DataAccess.TimescaleDb.Scope;

public interface ITransactionScope: ICommandScope, IDisposable
{
    void Begin();

    void Commit();

    void Rollback();
}