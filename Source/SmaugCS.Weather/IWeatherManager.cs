using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Data;

namespace SmaugCS.Weather
{
    public interface IWeatherManager
    {
        WeatherMap Weather { get; set; }
        WeatherCell GetWeather(AreaData area);
    }
}
