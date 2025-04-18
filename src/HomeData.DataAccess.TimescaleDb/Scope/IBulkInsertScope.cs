using System;
using System.Threading.Tasks;

namespace HomeData.DataAccess.TimescaleDb.Scope;

public interface IBulkInsertScope : IAsyncDisposable
{
    Task InsertRowAsync(params object[] values);
}