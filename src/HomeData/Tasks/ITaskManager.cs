namespace HomeData.Tasks;

public interface ITaskManager
{
    Task StartAsync();

    Task StopAsync();

    Task RunAsync(CancellationToken ct);
}