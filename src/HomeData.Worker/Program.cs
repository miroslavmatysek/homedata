namespace HomeData.Worker;

public static class Program
{
    public static void Main(params string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddOptions();
                services.AddHostedService<Worker>();
            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddJsonFile("appsettings.json", false, true);
                builder.AddJsonFile(
                    $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                    true,
                    true);
                builder.AddEnvironmentVariables();
            })
            .ConfigureServices((context, collection) =>
            {
                collection.AddNLog()
                    .AddConfiguration(context.Configuration, out var config)
                    .AddQuartz()
                    .AddServices(config)
                    .AddSingleton<Worker>();
            })
            .Build();

        host.Run();
    }
}