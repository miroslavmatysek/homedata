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
}