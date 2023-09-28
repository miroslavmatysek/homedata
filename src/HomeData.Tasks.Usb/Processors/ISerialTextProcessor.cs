using HomeData.Model;
using HomeData.Tasks.Usb.Model;

namespace HomeData.Tasks.Usb.Processors;

public interface ISerialTextProcessor
{
    void Init(UsbSerialTextTaskSetup setup);

    bool IsInit { get; }

    MeasureContainer Process();
}