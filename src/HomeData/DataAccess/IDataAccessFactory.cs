namespace HomeData.DataAccess;

public interface IDataAccessFactory
{
    IMonitoringDataAccess Create(string bucket, string measurement);
}