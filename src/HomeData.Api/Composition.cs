using HomeData.DataAccess;
using HomeData.DataAccess.TimescaleDb;
using HomeData.DataAccess.TimescaleDb.Dao;
using HomeData.DataAccess.TimescaleDb.Dao.Npgsql;
using HomeData.DataAccess.TimescaleDb.Scope;
using HomeData.DataAccess.TimescaleDb.Scope.Npgsql;
using HomeData.Model.Config;
using HomeData.Service;

namespace HomeData.Api;

internal static class Composition
{
    
    public static IServiceCollection AddDataAccess(this IServiceCollection sc)
    {
        sc.AddSingleton<IScopeProvider>(provider =>
            new NpgsqlScopeProvider(provider.GetRequiredService<DataAccessConfig>().ConnectionString));
        sc.AddTransient<IDataAccessFactory, TimeScaleDbDataAccessFactory>();
        sc.AddTransient<IMeasurementDao, NpgsqlMeasurementDao>();
        return sc;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection sc)
    {
        sc.AddSingleton<IWsLinkWeatherDataService, WsLinkWeatherDataService>();
        return sc;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection sc, ConfigurationManager configuration)
    {
        var dataAccessConfig = new DataAccessConfig();
        configuration.Bind("DataAccess", dataAccessConfig);
        sc.AddSingleton(dataAccessConfig);
        
        return sc;
    }
}