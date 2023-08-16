using HomeData.DataAccess;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HomeData.Tasks;

public interface IJobTask : IJob
{
    bool IsInit { get; }

    void Init(ILogger logger, IMonitoringDataAccess da, Dictionary<string, string> taskParams);
}