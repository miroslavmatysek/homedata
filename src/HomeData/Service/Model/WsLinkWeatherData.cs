namespace HomeData.Service.Model;

public class WsLinkWeatherData
{
    public DateTime DateTime { get; set; }
    
    public float RelativeAirPressure { get; set; }
    
    public float AbsoluteAirPressure { get; set; }
    
    public float IndoorTemperature { get; set; }
    
    public float IndoorHumidity { get; set; }
    
    public float OutdoorTemperature { get; set; }
    
    public float OutdoorHumidity { get; set; }
}