using SmaugCS.Data;

namespace SmaugCS.Weather
{
    public interface IWeatherManager
    {
        WeatherMap Weather { get; set; }
        WeatherCell GetWeather(AreaData area);
    }
}
