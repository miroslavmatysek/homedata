namespace HomeData.Worker;

public static class Program
{
    public static void Main(params string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services => { services.AddHostedService<Worker>(); })
            .Build();

        host.Run();
    }
}