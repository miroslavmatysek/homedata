using DryIoc;
using DryIoc.Microsoft.DependencyInjection;

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
            .UseServiceProviderFactory(new DryIocServiceProviderFactory())
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddJsonFile("appsettings.json", false, true);
                builder.AddJsonFile(
                    $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                    true,
                    true);
                builder.AddEnvironmentVariables();
            })
            .ConfigureContainer<Container>((hostContext, container) =>
            {
                container.AddNLog()
                    .AddConfiguration(hostContext.Configuration)
                    .AddQuartz()
                    .AddServices();
                container.Register<Worker>();
            })
            .Build();

        host.Run();
    }
}