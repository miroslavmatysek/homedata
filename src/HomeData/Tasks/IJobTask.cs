using HomeData.DataAccess;
using HomeData.Model.Config;
using HomeData.Provider;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HomeData.Tasks;

public interface IJobTask : IJob
{
    bool IsInit { get; }

    void Init(ILogger logger, IMonitoringDataAccess da, TaskConfig config, ITimeProvider timeProvider);
}