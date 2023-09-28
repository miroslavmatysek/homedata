namespace HomeData.Tasks.Usb;

public static class UsbConstants
{
    public const string BaudRateParamName = "baudRate";
    public const string PortNameParamName = "portName";
    public const string PortParityParamName = "portParity";
    public const string PortDataBitsParamName = "portDataBits";
    public const string PortHandshakeParamName = "portHandshake";
    public const string PortStopBitsParamName = "portStopBits";

    public const int BaudRateDefaultValue = 115200;
    public const int PortParityDefaultValue = 0;
    public const int PortDataBitsDefaultValue = 8;
    public const int PortHandshakeDefaultValue = 0;
    public const int PortStopBitsDefaultValue = 1;

}