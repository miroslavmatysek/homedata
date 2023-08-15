using HomeData.Tasks;

namespace HomeData.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITaskManager _taskManager;

    public Worker(ILogger<Worker> logger, ITaskManager taskManager)
    {
        _logger = logger;
        _taskManager = taskManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Worker start up at: {Time}", DateTimeOffset.Now);
            await _taskManager.RunAsync(stoppingToken);
            _logger.LogInformation("Worker canceled at: {Time}", DateTimeOffset.Now);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Worker failed at: {Time}", DateTimeOffset.Now);
        }
    }
}