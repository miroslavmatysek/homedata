using HomeData.Model.TaskSetup;

namespace HomeData.Service;

public interface ITaskService : IDisposable
{
    void Init(ITaskSetup setup);
}