using Ninject;
using Realm.Library.Common;
using SmaugCS.Data;

namespace SmaugCS.Weather
{
    public sealed class WeatherManager : IWeatherManager
    {
        public WeatherMap Weather { get; set; }
        private static IKernel _kernel;

        public WeatherManager(IKernel kernel)
        {
            _kernel = kernel;
        }

        public static IWeatherManager Instance
        {
            get { return _kernel.Get<IWeatherManager>(); }
        }

        public WeatherCell GetWeather(AreaData area)
        {
            return Weather.GetCellFromMap(area.WeatherX, area.WeatherY);
        }

        public static bool ExceedsThreshold(int initial, int delta, int threshold)
        {
            return ((initial < threshold) && (initial + delta >= threshold));
        }

        public static bool DropsBelowThreshold(int initial, int delta, int threshold)
        {
            return ((initial >= threshold) && (initial + delta < threshold));
        }
    }
}
