using HomeData.Model;
using HomeData.Provider;
using HomeData.Tasks.Usb.Model;

namespace HomeData.Tasks.Usb.Processors;

public interface ISerialTextProcessor
{
    void Init(UsbSerialTextTaskSetup setup, ITimeProvider timeProvider);

    bool IsInit { get; }

    MeasureContainer Process();
}