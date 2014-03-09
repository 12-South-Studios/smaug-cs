using Realm.Library.Common;
using SmaugCS.Data;

namespace SmaugCS.Weather
{
    public sealed class WeatherManager : GameSingleton
    {
        private static WeatherManager _instance;
        private static readonly object Padlock = new object();

        public WeatherMap Weather { get; set; }

        private WeatherManager()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static WeatherManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new WeatherManager());
                }
            }
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
