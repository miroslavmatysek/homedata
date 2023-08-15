using Microsoft.Extensions.Logging;
using Quartz;

namespace HomeData.Tasks;

public interface IJobTask : IJob
{
    void Init(ILogger logger);
}