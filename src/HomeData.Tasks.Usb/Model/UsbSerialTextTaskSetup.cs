using HomeData.Model.TaskSetup;

namespace HomeData.Tasks.Usb.Model;

public class UsbSerialTextTaskSetup : ITaskSetup
{
    public UsbSerialTextTaskSetup()
    {
        Name = nameof(UsbSerialTextTaskSetup);
    }

    public Guid Id { get; set; }

    public string Name { get; }

    public string PortName { get; set; }

    public int BaudRate { get; set; }

    public int PortParity { get; set; }

    public int PortStopBits { get; set; }

    public int PortDataBits { get; set; }

    public int PortHandshake { get; set; }
}