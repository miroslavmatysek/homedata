using HomeData.DataAccess;
using HomeData.Service.Model;
using Microsoft.Extensions.Logging;

namespace HomeData.Service;

public class WsLinkWeatherDataService : IWsLinkWeatherDataService
{
    private const string BucketName = "environment";
    private const string MeasurementName = "weather_station";
    
    private readonly ILogger<WsLinkWeatherDataService> _logger;
    private readonly IDataAccessFactory _dataAccessFactory;

    public WsLinkWeatherDataService(IDataAccessFactory dataAccessFactory, ILogger<WsLinkWeatherDataService> logger)
    {
        _dataAccessFactory = dataAccessFactory;
        _logger = logger;
    }

    public async Task SaveDataAsync(WsLinkWeatherData data)
    {
        _logger.LogDebug("Try to write measure data: {@MeasureData}", data);
        
        var dataAccess = _dataAccessFactory.Create(BucketName, MeasurementName);
        
        var measureContainer = dataAccess.Create(data.DateTime);

        measureContainer.With("indoor_temp", data.IndoorTemperature)
            .With("indoor_hum", data.IndoorHumidity)
            .With("outdoor_temp", data.OutdoorTemperature)
            .With("outdoor_hum", data.OutdoorHumidity);
        await dataAccess.WritePointAsync(measureContainer);
        _logger.LogInformation("Measure data was saved");
    }
}