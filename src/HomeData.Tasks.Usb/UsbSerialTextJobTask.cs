using HomeData.DataAccess;
using HomeData.Model;
using HomeData.Model.Config;
using HomeData.Provider;
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
    private ITimeProvider _timeProvider;

    public UsbSerialTextJobTask(ISerialTextProcessor serialTextProcessor)
    {
        _serialTextProcessor = serialTextProcessor;
        _minDataInterval = TimeSpan.FromSeconds(30);
    }

    public bool IsInit
    {
        get => _setup != null && _serialTextProcessor.IsInit;
    }

    public void Init(ILogger logger, IMonitoringDataAccess da, TaskConfig config, ITimeProvider timeProvider)
    {
        if (IsInit)
            throw new Exception("Task is already initialized");

        _logger = logger;
        _dataAccess = da;
        _timeProvider = timeProvider;
        _setup = SetupConvert(config);
        _serialTextProcessor.Init(_setup, timeProvider);
        _logger.LogInformation("Task: {TaskName} was initialized", nameof(UsbSerialTextJobTask));
    }

    public async Task Execute(IJobExecutionContext context)
    {
        if (!IsInit)
            throw new Exception("Task is not initialized");

        _logger.LogInformation("Task: {TaskName} was started", nameof(UsbSerialTextJobTask));
        var data = _serialTextProcessor.Process();
        if (data != null)
        {
            await SaveData(data);
        }
        _logger.LogInformation("Task: {TaskName} was finished", nameof(UsbSerialTextJobTask));
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

    private static UsbSerialTextTaskSetup SetupConvert(TaskConfig config)
    {
        return new UsbSerialTextTaskSetup()
        {
            Id = config.Id,
            BaudRate = config.Params.GetOrDefaultInt(UsbConstants.BaudRateParamName, UsbConstants.BaudRateDefaultValue),
            PortName = config.Params.GetOrDefaultString(UsbConstants.PortNameParamName, string.Empty),
            PortParity = config.Params.GetOrDefaultInt(UsbConstants.PortParityParamName, UsbConstants.PortParityDefaultValue),
            PortDataBits = config.Params.GetOrDefaultInt(UsbConstants.PortDataBitsParamName, UsbConstants.PortDataBitsDefaultValue),
            PortHandshake = config.Params.GetOrDefaultInt(UsbConstants.PortHandshakeParamName, UsbConstants.PortHandshakeDefaultValue),
            PortStopBits = config.Params.GetOrDefaultInt(UsbConstants.PortStopBitsParamName, UsbConstants.PortStopBitsDefaultValue),
        };
    }
}