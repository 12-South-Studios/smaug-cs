using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Objects;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Weather
{
    public sealed class WeatherManager : GameSingleton
    {
        private static WeatherManager _instance;
        private static readonly object Padlock = new object();

        public WeatherMap Weather { get; private set; }

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

        public void InitializeWeatherMap()
        {
            Weather = new WeatherMap(Program.WEATHER_SIZE_X, Program.WEATHER_SIZE_Y);
            Weather.Initialize();
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

        public static void WeatherMessage(string txt, int x, int y)
        {
            IEnumerable<DescriptorData> players = db.DESCRIPTORS.Where(c => c.ConnectionStatus == ConnectionTypes.Playing
                                                                    && c.Character != null
                                                                    && c.Character.IsOutside()
                                                                    && c.Character.IsAwake()
                                                                    && c.Character.CurrentRoom != null
                                                                    && c.Character.CurrentRoom.Area != null
                                                                    && c.Character.CurrentRoom.Area.WeatherX == x
                                                                    && c.Character.CurrentRoom.Area.WeatherY == y);

            players.ToList().ForEach(p => color.send_to_char(txt, p.Character));
        }
    }
}
