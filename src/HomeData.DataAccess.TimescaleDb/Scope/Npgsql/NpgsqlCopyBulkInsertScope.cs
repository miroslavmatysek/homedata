using Npgsql;
using System.Threading.Tasks;

namespace HomeData.DataAccess.TimescaleDb.Scope.Npgsql;

public class NpgsqlCopyBulkInsertScope : IBulkInsertScope
{

    private readonly NpgsqlBinaryImporter importer;
    
    public NpgsqlCopyBulkInsertScope(NpgsqlBinaryImporter importer)
    {
        this.importer = importer;
    }

    public async Task InsertRowAsync(params object[] values)
    {
        await importer.StartRowAsync();
        for (int i = 0; i < values.Length; i++)
        {
            await importer.WriteAsync(values[i]);
        }
        
    }


    public async ValueTask DisposeAsync()
    {
        await importer.CompleteAsync();
        await importer.DisposeAsync();
    }
}