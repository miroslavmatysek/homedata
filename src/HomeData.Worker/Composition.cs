using System.Reflection;
using DryIoc;
using HomeData.DataAccess;
using HomeData.DataAccess.Influxdb;
using HomeData.Model.Config;
using HomeData.Tasks;
using HomeData.Tasks.Manager;
using HomeData.Tasks.Solax;
using NLog;
using NLog.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace HomeData.Worker;

public static class Composition
{
    public static Container AddNLog(this Container container)
    {
        LogManager.Setup().LoadConfigurationFromFile("nlog.config");

        LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging")));
        LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Abstractions")));
        LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("NLog.Extensions.Logging")));

        var loggerProvider =
            new NLogLoggerProvider(new NLogProviderOptions(), LogManager.LogFactory);
        ILoggerFactory loggerFactory = new NLogLoggerFactory(loggerProvider);
        container.Use(loggerFactory);
        container.Use(loggerProvider);


        var loggerFactoryMethod =
            typeof(LoggerFactoryExtensions).GetMethod("CreateLogger", new[] { typeof(ILoggerFactory) });
        container.Register(typeof(ILogger<>), made: Made.Of(
            req => loggerFactoryMethod?.MakeGenericMethod(req.ServiceType.GenericTypeArguments[0])));
        var commonLogger = loggerFactory.CreateLogger("common");
        container.RegisterInstance(typeof(Microsoft.Extensions.Logging.ILogger), commonLogger);
        commonLogger.LogInformation("NLog init");
        return container;
    }

    public static Container AddQuartz(this Container container)
    {
        ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

        container.RegisterInstance(schedulerFactory);

        IJobFactory jobFactory = new QuartzJobFactory(container);
        container.RegisterInstance(jobFactory);
        return container;
    }

    public static Container AddServices(this Container container)
    {
        container.Register<ITaskManager, QuartzTaskManager>(Reuse.Singleton);
        container.Register<SolaxX3G4JobTask>(Reuse.Singleton);
        container.Register<IDataAccessFactory, InfluxDataAccessFactory>(Reuse.Singleton);
        return container;
    }

    public static Container AddConfiguration(this Container container, IConfiguration configuration)
    {
        var tasksConfig = new TasksConfig();
        configuration.Bind("tasks", tasksConfig);
        container.Use(tasksConfig);

        DataAccessConfig dataAccessConfig = new DataAccessConfig();
        configuration.Bind("dataAccess", dataAccessConfig);
        container.Use(dataAccessConfig);
        return container;
    }
}