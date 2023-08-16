using HomeData.Tasks.Solax.Extensions;
using HomeData.Tasks.Solax.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace HomeData.Tasks.Solax;

public class SolaxX3G4JobTask : IJobTask
{
    private const string RequestPath = "http://{0}/";
    private const string OptTypeBodyParam = "optType";
    private const string RealTimeOptValue = "ReadRealTimeData";
    private const string PwdBodyParam = "pwd";
    
    private readonly HttpClient _httpClient;
    private readonly Dictionary<string, string> _realTimeBody;
    private ILogger _logger;
    

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

        using (var request = new HttpRequestMessage(HttpMethod.Post, _url))
        {
            request.Content = new FormUrlEncodedContent(_realTimeBody);
            using (var response = await _httpClient.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var rawData = JsonConvert.DeserializeObject<SolaxInvertedRawData>(data);
                    var processedData = Process(rawData);
                }
            }

            
        }
        _logger.LogInformation("Solax task finished");
    }

    public bool IsInit { get; private set; }

    public void Init(ILogger logger)
    {
        IsInit = true;
        _logger = logger;
        _ipAddress = "192.168.88.238";
        _pass = "SVNUKHYLSA";
        _url = string.Format(RequestPath, _ipAddress);
        _realTimeBody.Add(PwdBodyParam, _pass);
    }

    private SolaxX3G4Data Process(SolaxInvertedRawData data)
    {
        return new SolaxX3G4Data
        {
            Version = data.Version,
            SerialNumber = data.SerialNumber,
            Grid1Voltage = data.Data.ToDecimal(0, 1) ?? 0.0M,
            Grid2Voltage = data.Data.ToDecimal(1, 1) ?? 0.0M,
            Grid3Voltage = data.Data.ToDecimal(2, 1) ?? 0.0M,

            Grid1Current = data.Data.ToDecimal(3, 1, true) ?? 0.0M,
            Grid2Current = data.Data.ToDecimal(4, 1, true) ?? 0.0M,
            Grid3Current = data.Data.ToDecimal(5, 1, true) ?? 0.0M,

            Grid1Power = data.Data.ToInt(6,  true) ?? 0,
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