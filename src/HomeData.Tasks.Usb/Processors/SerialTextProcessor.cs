using HomeData.Model;
using HomeData.Provider;
using HomeData.Service;
using HomeData.Tasks.Usb.Model;
using HomeData.Tasks.Usb.Service;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HomeData.Tasks.Usb.Processors;

public class SerialTextProcessor : ISerialTextProcessor
{
    private readonly ILogger<SerialTextProcessor> _logger;
    private readonly ITaskServiceProvider _taskServiceProvider;

    private IUsbSerialTextService _usbSerialTextService;
    private UsbSerialTextTaskSetup _setup;
    private ITimeProvider _timeProvider;

    public SerialTextProcessor(ILogger<SerialTextProcessor> logger, ITaskServiceProvider taskServiceProvider)
    {
        _logger = logger;
        _taskServiceProvider = taskServiceProvider;
    }

    public void Init(UsbSerialTextTaskSetup setup, ITimeProvider timeProvider)
    {
        if (IsInit)
        {
            throw new NotSupportedException("Service is already initialized");
        }

        _setup = setup;
        _timeProvider = timeProvider;
        _usbSerialTextService = _taskServiceProvider.Get<IUsbSerialTextService>(_setup);
        _usbSerialTextService.Start();
    }

    public bool IsInit
    {
        get => _setup != null && _usbSerialTextService != null;
    }

    public MeasureContainer Process()
    {
        _logger.LogDebug("Process started");
        if (IsInit)
        {
            var data  =_usbSerialTextService.GetData();

            _logger.LogDebug("Loaded data form USB - records count: {Count}", data?.Length ?? 0);

            if (data != null && data.Length > 0)
            {
                var result = new MeasureContainer(_timeProvider.GetNow());
                foreach (var item in data)
                {
                    var itemData = JsonConvert.DeserializeObject<Dictionary<string, double>>(item);
                    foreach (var valueData in itemData)
                    {
                        result.AddOrUpdate(new DecimalMeasureItem(valueData.Key)
                        {
                            DateTime = result.Time,
                            LastChanged = result.Time,
                            ItemValue = Convert.ToDecimal(valueData.Value),
                            Changed = true,
                            Type = MeasureItemType.Value
                        });
                    }
                }
                _logger.LogDebug("Processed data from USB");
                return result;
            }
            _logger.LogDebug("Processed data form USB - no data");
            return null;
        }

        throw new NotSupportedException("Processor is not initialized");
    }
}