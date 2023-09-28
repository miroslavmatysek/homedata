using HomeData.Service;

namespace HomeData.Tasks.Usb.Service;

public interface IUsbSerialTextService : ITaskService
{
    bool IsRunning { get; }

    void Start();

    void Stop();

    string[] GetData();
}