using HomeData.Model.TaskSetup;

namespace HomeData.Service;

public interface ITaskServiceProvider : IDisposable
{
    T Get<T>(ITaskSetup setup) where T : ITaskService;
}