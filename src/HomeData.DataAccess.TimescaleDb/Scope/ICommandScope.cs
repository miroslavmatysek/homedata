using System.Data;
using Npgsql;

namespace HomeData.DataAccess.TimescaleDb.Scope;

public interface ICommandScope
{
    Task<int> ExecuteAsync(string query, params NpgsqlParameter[] parameters);


    IBulkInsertScope CreateBulkInsert(string insertQuery);


}