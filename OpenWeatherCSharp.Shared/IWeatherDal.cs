using OpenWeatherCSharp.Shared.Models;

namespace OpenWeatherCSharp.Shared
{
    public interface IWeatherDal
    {
        WeatherResposne GetWeather(string lat, string lon, string appId);
    }
}
