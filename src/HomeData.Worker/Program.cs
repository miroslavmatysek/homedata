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