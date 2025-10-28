using HomeData.Api.Model;
using HomeData.Service;
using HomeData.Service.Model;
using Microsoft.AspNetCore.Mvc;

namespace HomeData.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WsLinkController : ControllerBase
{
    private readonly ILogger<WsLinkController> _logger;
    private readonly IWsLinkWeatherDataService _wsLinkWeatherDataService;

    public WsLinkController(ILogger<WsLinkController> logger, IWsLinkWeatherDataService wsLinkWeatherDataService)
    {
        _logger = logger;
        _wsLinkWeatherDataService = wsLinkWeatherDataService;
    }

    [HttpGet("upload-data")]
    public async Task UploadData([FromQuery] WsLinkRequestParam requestParam)
    {
        _logger.LogDebug("Try to upload data: {@RequestParam}", requestParam);

        try
        {
            await _wsLinkWeatherDataService.SaveDataAsync(new WsLinkWeatherData
            {
                DateTime = requestParam.DateTime.ToUniversalTime(),
                RelativeAirPressure = requestParam.Rbar,
                IndoorTemperature = requestParam.Intem,
                IndoorHumidity = requestParam.Inhum,
                OutdoorHumidity = requestParam.T1hum,
                OutdoorTemperature = requestParam.T1temp
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while upload data");
        }
    }
}