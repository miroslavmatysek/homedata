using HomeData.DataAccess;
using HomeData.Model;
using HomeData.Provider;
using HomeData.Tasks.Solax.Extensions;
using HomeData.Tasks.Solax.Model;
using HomeData.Util;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace HomeData.Tasks.Solax;

public class SolaxX3G4JobTask : IJobTask
{
    private const string HostParamName = "host";
    private const string PasswordParamName = "password";

    private const string RequestPath = "http://{0}/";
    private const string OptTypeBodyParam = "optType";
    private const string RealTimeOptValue = "ReadRealTimeData";
    private const string PwdBodyParam = "pwd";

    private readonly HttpClient _httpClient;
    private readonly Dictionary<string, string> _realTimeBody;
    private readonly Dictionary<string, string> _deltaNameMap;
    private readonly TimeSpan _minDataInterval = TimeSpan.FromSeconds(60);
    private ILogger _logger;
    private IMonitoringDataAccess _monitoringDataAccess;
    private ITimeProvider _timeProvider;


    private string _ipAddress;
    private string _pass;
    private string _url;
    private MeasureContainer _lastContainer;

    public SolaxX3G4JobTask()
    {
        _httpClient = new HttpClient();
        _realTimeBody = new Dictionary<string, string>();
        _deltaNameMap = new Dictionary<string, string>();
        _realTimeBody.Add(OptTypeBodyParam, RealTimeOptValue);
        _lastContainer = null;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Solax task started");

        try
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, _url))
            {
                request.Content = new FormUrlEncodedContent(_realTimeBody);
                using (var response = await _httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var now = _timeProvider.GetNow();
                        var data = await response.Content.ReadAsStringAsync();
                        var rawData = JsonConvert.DeserializeObject<SolaxInvertedRawData>(data);
                        var processedData = Process(rawData, now);
                        processedData = ProcessDelta(processedData);
                        await SaveData(processedData);
                    }
                }
            }

            _logger.LogInformation("Solax task finished");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Solax task finished by error");
        }
    }

    private MeasureContainer ProcessDelta(MeasureContainer processedData)
    {
        var aggregateData = processedData.Data.Where(i => i.AggregateType).ToList();
        if (_lastContainer == null)
        {
            foreach (var ai in aggregateData)
            {
                var na = ai.CreateSimilar(GetDeltaName(ai.Field));
                na.SetZero();
                na.Changed = true;
                na.Type = MeasureItemType.Delta;
                processedData.Add(na);
            }
        }
        else
        {
            var lastData = _lastContainer.Data.Where(i => i.AggregateType).ToDictionary(i => i.Field, i => i);
            foreach (var ai in aggregateData)
            {
                var na = ai.CreateSimilar(GetDeltaName(ai.Field));
                na.SetZero();
                na.Changed = true;
                na.Type = MeasureItemType.Delta;

                if (lastData.TryGetValue(ai.Field, out var lastItem) && ai.IsGreatOrEquals(lastItem))
                    na.ItemValue = ai.Subtract(lastItem.ItemValue);
                processedData.Add(na);
            }
        }

        return processedData;
    }

    private string GetDeltaName(string itemName)
    {
        if (!_deltaNameMap.ContainsKey(itemName))
            _deltaNameMap.Add(itemName, $"{itemName}_delta");
        return _deltaNameMap[itemName];
    }

    private async Task SaveData(MeasureContainer processedData)
    {
        var toSave = _lastContainer == null ? processedData : _lastContainer.Merge(processedData);

        var fields = _monitoringDataAccess.Create(processedData.Time);
        foreach (var item in toSave.Data)
        {
            if (item.Changed || item.ChangedInterval >= _minDataInterval)
            {
                fields.With(item);
            }
        }

        await _monitoringDataAccess.WritePointAsync(fields);
        _lastContainer = processedData;
    }

    public bool IsInit { get; private set; }

    public void Init(ILogger logger, IMonitoringDataAccess monitoringDataAccess, Dictionary<string, string> taskParams,
        ITimeProvider timeProvider)
    {
        IsInit = true;
        _logger = logger;
        _monitoringDataAccess = monitoringDataAccess;
        _timeProvider = timeProvider;

        _ipAddress = taskParams.GetOrDefaultString(HostParamName, string.Empty);
        if (string.IsNullOrEmpty(_ipAddress))
            throw new ArgumentNullException(
                $"Task: {nameof(SolaxX3G4JobTask)} - wrong config - config param: {HostParamName} missing");

        _pass = taskParams.GetOrDefaultString(PasswordParamName, string.Empty);
        if (!taskParams.ContainsKey(PasswordParamName) || string.IsNullOrEmpty(_pass))
            throw new ArgumentNullException(
                $"Task: {nameof(SolaxX3G4JobTask)} - wrong config - config param: {PasswordParamName} missing");

        _url = string.Format(RequestPath, _ipAddress);
        _realTimeBody.Add(PwdBodyParam, _pass);
    }

    private MeasureContainer Process(SolaxInvertedRawData data, DateTime time)
    {
        var result = new MeasureContainer(time);

        result.AddString(SolaxConstants.Attributes.Version, data.Version);
        result.AddString(SolaxConstants.Attributes.SerialNumber, data.SerialNumber);

        result.AddDecimal(SolaxConstants.Attributes.Grid1Voltage, data.Data.ToDecimal(0, 1) ?? 0.0M);
        result.AddDecimal(SolaxConstants.Attributes.Grid2Voltage, data.Data.ToDecimal(1, 1) ?? 0.0M);
        result.AddDecimal(SolaxConstants.Attributes.Grid3Voltage, data.Data.ToDecimal(2, 1) ?? 0.0M);

        result.AddDecimal(SolaxConstants.Attributes.Grid1Current, data.Data.ToDecimal(3, 1, true) ?? 0.0M);
        result.AddDecimal(SolaxConstants.Attributes.Grid2Current, data.Data.ToDecimal(4, 1, true) ?? 0.0M);
        result.AddDecimal(SolaxConstants.Attributes.Grid3Current, data.Data.ToDecimal(5, 1, true) ?? 0.0M);

        result.AddInt(SolaxConstants.Attributes.Grid1Power, data.Data.ToInt(6, true) ?? 0);
        result.AddInt(SolaxConstants.Attributes.Grid2Power, data.Data.ToInt(7, true) ?? 0);
        result.AddInt(SolaxConstants.Attributes.Grid3Power, data.Data.ToInt(8, true) ?? 0);

        result.AddInt(SolaxConstants.Attributes.InverterPower, data.Data.ToInt(9, true));
        result.AddInt(SolaxConstants.Attributes.CurrentHousePower, data.Data.ToInt(47, true));

        result.AddDecimal(SolaxConstants.Attributes.Grid1Frequency, data.Data.ToDecimal(16, 2));
        result.AddDecimal(SolaxConstants.Attributes.Grid2Frequency, data.Data.ToDecimal(17, 2));
        result.AddDecimal(SolaxConstants.Attributes.Grid3Frequency, data.Data.ToDecimal(18, 2));

        result.AddInt(SolaxConstants.Attributes.PowerPv1, data.Data.ToInt(14) ?? 0);
        result.AddInt(SolaxConstants.Attributes.PowerPv2, data.Data.ToInt(15) ?? 0);

        result.AddInt64(SolaxConstants.Attributes.FeedInPower, (data.Data.ToAccumulatedInt(34, 35) ?? 0).ToSigned32());
        result.AddDecimal(SolaxConstants.Attributes.FeedInEnergy,
            (data.Data.ToAccumulatedInt(86, 87) ?? 0).ToDecimal(2) ?? 0.0m);
        result.AddDecimal(SolaxConstants.Attributes.ConsumedEnergy,
            (data.Data.ToAccumulatedInt(88, 89) ?? 0).ToDecimal(2) ?? 0.0m, type: MeasureItemType.Aggregate);

        result.AddDecimal(SolaxConstants.Attributes.YieldTotal,
            (data.Data.ToAccumulatedInt(68, 69) ?? 0).ToDecimal(1) ?? 0.0M);
        result.AddDecimal(SolaxConstants.Attributes.YieldToday, data.Data.ToDecimal(70, 1) ?? 0.0m,
            type: MeasureItemType.Aggregate);

        result.AddInt(SolaxConstants.Attributes.RadiatorTemperature, data.Data.ToInt(54, true));

        result.AddInt(SolaxConstants.Attributes.BatteryCapacity, data.Data.ToInt(103));
        result.AddInt(SolaxConstants.Attributes.BatteryTemperature, data.Data.ToInt(105, true));
        result.AddInt(SolaxConstants.Attributes.BatteryPower, data.Data.ToInt(41, true));

        result.AddDecimal(SolaxConstants.Attributes.TodayGridInEnergy, data.Data.ToDecimal(92, 2),
            type: MeasureItemType.Aggregate);
        result.AddDecimal(SolaxConstants.Attributes.TodayGridOutEnergy, data.Data.ToDecimal(90, 2),
            type: MeasureItemType.Aggregate);

        result.AddDecimal(SolaxConstants.Attributes.EnergyToday, data.Data.ToDecimal(82, 1),
            type: MeasureItemType.Aggregate);

        result.AddString(SolaxConstants.Attributes.BatteryOperationMode, ToBatteryOperationMode(data.Data.ToInt(168)));
        result.AddString(SolaxConstants.Attributes.InverterOperationMode, ToInverterOperationMode(data.Data.ToInt(19)));

        return result;
    }

    private static string ToBatteryOperationMode(int? value)
    {
        if (value == null)
            return SolaxConstants.BatteryOperationModes.Unknown;

        return value switch
        {
            0 => SolaxConstants.BatteryOperationModes.SelfUseMode,
            1 => SolaxConstants.BatteryOperationModes.ForceTimeUse,
            2 => SolaxConstants.BatteryOperationModes.BackUpMode,
            4 => SolaxConstants.BatteryOperationModes.FeedInPriority,
            _ => SolaxConstants.BatteryOperationModes.Unknown
        };
    }

    private static string ToInverterOperationMode(int? value)
    {
        if (value == null)
            return SolaxConstants.InverterOperationModes.Unknown;
        return value switch
        {
            0 => SolaxConstants.InverterOperationModes.Waiting,
            1 => SolaxConstants.InverterOperationModes.Checking,
            2 => SolaxConstants.InverterOperationModes.Normal,
            3 => SolaxConstants.InverterOperationModes.Off,
            4 => SolaxConstants.InverterOperationModes.PermanentFault,
            5 => SolaxConstants.InverterOperationModes.Updating,
            6 => SolaxConstants.InverterOperationModes.EpsCheck,
            7 => SolaxConstants.InverterOperationModes.EpsMode,
            8 => SolaxConstants.InverterOperationModes.SelfTest,
            9 => SolaxConstants.InverterOperationModes.Idle,
            10 => SolaxConstants.InverterOperationModes.Standby,
            _ => SolaxConstants.InverterOperationModes.Unknown
        };
    }
}