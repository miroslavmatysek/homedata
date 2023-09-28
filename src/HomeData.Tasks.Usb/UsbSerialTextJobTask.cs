using HomeData.DataAccess;
using HomeData.Model;
using HomeData.Tasks.Usb.Model;
using HomeData.Tasks.Usb.Processors;
using HomeData.Util;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HomeData.Tasks.Usb;

public class UsbSerialTextJobTask : IJobTask
{
    private readonly ISerialTextProcessor _serialTextProcessor;
    private readonly TimeSpan _minDataInterval;

    private UsbSerialTextTaskSetup _setup;
    private ILogger _logger;
    private IMonitoringDataAccess _dataAccess;

    private MeasureContainer _lastContainer;

    public UsbSerialTextJobTask(ISerialTextProcessor serialTextProcessor)
    {
        _serialTextProcessor = serialTextProcessor;
        _minDataInterval = TimeSpan.FromSeconds(30);
    }

    public bool IsInit
    {
        get => _setup != null && _serialTextProcessor.IsInit;
    }

    public void Init(ILogger logger, IMonitoringDataAccess da, Dictionary<string, string> taskParams)
    {
        if (IsInit)
            throw new Exception("Task is already initialized");

        _logger = logger;
        _dataAccess = da;
        _setup = SetupConvert(taskParams);
        _serialTextProcessor.Init(_setup);
    }

    public async Task Execute(IJobExecutionContext context)
    {
        if (!IsInit)
            throw new Exception("Task is not initialized");

        var data = _serialTextProcessor.Process();
        if (data != null)
        {
            await SaveData(data);
        }
    }

    private async Task SaveData(MeasureContainer processedData)
    {
        var toSave = _lastContainer == null ? processedData : _lastContainer.Merge(processedData);

        var fields = _dataAccess.Create(processedData.Time);
        foreach (var item in toSave.Data)
        {
            if (item.Changed || item.ChangedInterval >= _minDataInterval)
            {
                fields.With(item);
            }
        }

        await _dataAccess.WritePointAsync(fields);
        _lastContainer = processedData;
    }

    private static UsbSerialTextTaskSetup SetupConvert(Dictionary<string, string> taskParams)
    {
        return new UsbSerialTextTaskSetup()
        {
            BaudRate = taskParams.GetOrDefaultInt(UsbConstants.BaudRateParamName, UsbConstants.BaudRateDefaultValue),
            PortName = taskParams.GetOrDefaultString(UsbConstants.PortNameParamName, string.Empty),
            PortParity = taskParams.GetOrDefaultInt(UsbConstants.PortParityParamName, UsbConstants.PortParityDefaultValue),
            PortDataBits = taskParams.GetOrDefaultInt(UsbConstants.PortDataBitsParamName, UsbConstants.PortDataBitsDefaultValue),
            PortHandshake = taskParams.GetOrDefaultInt(UsbConstants.PortHandshakeParamName, UsbConstants.PortHandshakeDefaultValue),
            PortStopBits = taskParams.GetOrDefaultInt(UsbConstants.PortStopBitsParamName, UsbConstants.PortStopBitsDefaultValue),
        };
    }
}