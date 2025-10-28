using HomeData.Service.Model;

namespace HomeData.Service;

public interface IWsLinkWeatherDataService
{
    Task SaveDataAsync(WsLinkWeatherData data);
}