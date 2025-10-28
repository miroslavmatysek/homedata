using HomeData.DataAccess;
using HomeData.DataAccess.Influxdb;
using HomeData.DataAccess.TimescaleDb;
using HomeData.DataAccess.TimescaleDb.Dao;
using HomeData.DataAccess.TimescaleDb.Dao.Npgsql;
using HomeData.DataAccess.TimescaleDb.Scope;
using HomeData.DataAccess.TimescaleDb.Scope.Npgsql;
using HomeData.Model.Config;
using HomeData.Service;
using HomeData.Tasks;
using HomeData.Tasks.Manager;
using HomeData.Tasks.Solax;
using HomeData.Tasks.Usb;
using HomeData.Tasks.Usb.Processors;
using HomeData.Tasks.Usb.Service;
using NLog;
using NLog.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace HomeData.Worker;

public static class Composition
{
    public static IServiceCollection AddNLog(this IServiceCollection sc)
    {
        LogManager.Setup().LoadConfigurationFromFile("nlog.config");
        sc.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddNLog();
        });


        return sc;
    }

    public static IServiceCollection AddQuartz(this IServiceCollection sc)
    {
        ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
        sc.AddSingleton(schedulerFactory);
        sc.AddSingleton<IJobFactory>(provider => new QuartzJobFactory(provider));
        return sc;
    }

    public static IServiceCollection AddServices(this IServiceCollection sc, MainConfig config)
    {
        sc.AddSingleton<ITaskManager, QuartzTaskManager>();
        sc.AddSingleton<SolaxX3G4JobTask>();
        sc.AddSingleton<UsbSerialTextJobTask>();
        sc.AddSingleton<IUsbSerialTextService, UsbSerialTextService>();
        sc.AddSingleton<ITaskServiceProvider, TaskServiceProvider>();
        sc.AddSingleton<ISerialTextProcessor, SerialTextProcessor>();

        if (!string.IsNullOrEmpty(config.DataAccess.ConnectionString))
        {
            sc.AddSingleton<IScopeProvider>(provider =>
                new NpgsqlScopeProvider(provider.GetRequiredService<MainConfig>().DataAccess.ConnectionString));
            sc.AddTransient<IDataAccessFactory, TimeScaleDbDataAccessFactory>();
            sc.AddTransient<IMeasurementDao, NpgsqlMeasurementDao>();
        }
        else
        {
            sc.AddSingleton<IDataAccessFactory, InfluxDataAccessFactory>();
        }

        return sc;
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection sc, IConfiguration configuration,
        out MainConfig mainConfig)
    {
        var tasksConfig = new TasksConfig();
        configuration.Bind("tasks", tasksConfig);
        sc.AddSingleton(tasksConfig);

        var dataAccessConfig = new DataAccessConfig();
        configuration.Bind("dataAccess", dataAccessConfig);
        sc.AddSingleton(dataAccessConfig);

        mainConfig = new MainConfig
        {
            Tasks = tasksConfig,
            DataAccess = dataAccessConfig
        };
        sc.AddSingleton(mainConfig);
        return sc;
    }
}