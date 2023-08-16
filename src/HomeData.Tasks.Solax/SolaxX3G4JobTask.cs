using HomeData.DataAccess;
using HomeData.Tasks.Solax.Extensions;
using HomeData.Tasks.Solax.Model;
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
    private ILogger _logger;
    private IMonitoringDataAccess _monitoringDataAccess;


    private string _ipAddress;
    private string _pass;
    private string _url;

    public SolaxX3G4JobTask()
    {
        _httpClient = new HttpClient();
        _realTimeBody = new Dictionary<string, string>();
        _realTimeBody.Add(OptTypeBodyParam, RealTimeOptValue);
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
                        var now = DateTime.Now;
                        var data = await response.Content.ReadAsStringAsync();
                        var rawData = JsonConvert.DeserializeObject<SolaxInvertedRawData>(data);
                        var processedData = Process(rawData, now);
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

    private async Task SaveData(SolaxX3G4Data processedData)
    {
        var fields = _monitoringDataAccess.Create(processedData.Time);
        fields.With(nameof(processedData.Grid1Voltage), processedData.Grid1Voltage)
            .With(nameof(processedData.Grid2Voltage), processedData.Grid2Voltage)
            .With(nameof(processedData.Grid3Voltage), processedData.Grid3Voltage)
            .With(nameof(processedData.Grid1Current), processedData.Grid1Current)
            .With(nameof(processedData.Grid2Current), processedData.Grid2Current)
            .With(nameof(processedData.Grid3Current), processedData.Grid3Current)
            .With(nameof(processedData.Grid1Frequency), processedData.Grid1Frequency)
            .With(nameof(processedData.Grid2Frequency), processedData.Grid2Frequency)
            .With(nameof(processedData.Grid3Frequency), processedData.Grid3Frequency)
            .With(nameof(processedData.Grid1Power), processedData.Grid1Power)
            .With(nameof(processedData.Grid2Power), processedData.Grid2Power)
            .With(nameof(processedData.Grid3Power), processedData.Grid3Power)
            .With(nameof(processedData.PowerPv1), processedData.PowerPv1)
            .With(nameof(processedData.PowerPv2), processedData.PowerPv2);
        await _monitoringDataAccess.WritePointAsync(fields);
    }

    public bool IsInit { get; private set; }

    public void Init(ILogger logger, IMonitoringDataAccess monitoringDataAccess, Dictionary<string, string> taskParams)
    {
        IsInit = true;
        _logger = logger;
        _monitoringDataAccess = monitoringDataAccess;

        if (!taskParams.ContainsKey(HostParamName) || string.IsNullOrEmpty(taskParams[HostParamName]))
            throw new ArgumentNullException(
                $"Task: {nameof(SolaxX3G4JobTask)} - wrong config - config param: {HostParamName} missing");
        _ipAddress = taskParams[HostParamName];

        if (!taskParams.ContainsKey(PasswordParamName) || string.IsNullOrEmpty(taskParams[PasswordParamName]))
            throw new ArgumentNullException(
                $"Task: {nameof(SolaxX3G4JobTask)} - wrong config - config param: {PasswordParamName} missing");
        _pass = taskParams[PasswordParamName];

        _url = string.Format(RequestPath, _ipAddress);
        _realTimeBody.Add(PwdBodyParam, _pass);
    }

    private SolaxX3G4Data Process(SolaxInvertedRawData data, DateTime time)
    {
        return new SolaxX3G4Data
        {
            Time = time,
            Version = data.Version,
            SerialNumber = data.SerialNumber,
            Grid1Voltage = data.Data.ToDecimal(0, 1) ?? 0.0M,
            Grid2Voltage = data.Data.ToDecimal(1, 1) ?? 0.0M,
            Grid3Voltage = data.Data.ToDecimal(2, 1) ?? 0.0M,

            Grid1Current = data.Data.ToDecimal(3, 1, true) ?? 0.0M,
            Grid2Current = data.Data.ToDecimal(4, 1, true) ?? 0.0M,
            Grid3Current = data.Data.ToDecimal(5, 1, true) ?? 0.0M,

            Grid1Power = data.Data.ToInt(6, true) ?? 0,
            Grid2Power = data.Data.ToInt(7, true) ?? 0,
            Grid3Power = data.Data.ToInt(8, true) ?? 0,

            Grid1Frequency = data.Data.ToDecimal(16, 2),
            Grid2Frequency = data.Data.ToDecimal(17, 2),
            Grid3Frequency = data.Data.ToDecimal(18, 2),


            PowerPv1 = data.Data.ToInt(14) ?? 0,
            PowerPv2 = data.Data.ToInt(15) ?? 0
        };
    }
}