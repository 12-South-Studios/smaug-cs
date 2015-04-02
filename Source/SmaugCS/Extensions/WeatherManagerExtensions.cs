using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Extensions.Character;
using SmaugCS.Managers;
using SmaugCS.Weather;

namespace SmaugCS.Extensions
{
    public static class WeatherManagerExtensions
    {
        public static void InitializeWeatherMap(this IWeatherManager manager, int weatherSizeX, int weatherSizeY)
        {
            var newMap = new WeatherMap(GameManager.Instance.GameTime, weatherSizeX, weatherSizeY);
            newMap.LoadMap(SystemFileTypes.StarMap, newMap.StarMap.ToList());
            newMap.LoadMap(SystemFileTypes.SunMap, newMap.SunMap.ToList());
            newMap.LoadMap(SystemFileTypes.MoonMap, newMap.MoonMap.ToList());
            newMap.Initialize();
            manager.Weather = newMap;
        }

        public static void WeatherMessage(this IWeatherManager manager, string txt, int x, int y)
        {
            var players = db.DESCRIPTORS.Where(c => c.ConnectionStatus == ConnectionTypes.Playing
                                                                    && c.Character != null
                                                                    && c.Character.IsOutside()
                                                                    && c.Character.IsAwake()
                                                                    && c.Character.CurrentRoom != null
                                                                    && c.Character.CurrentRoom.Area != null
                                                                    && c.Character.CurrentRoom.Area.WeatherX == x
                                                                    && c.Character.CurrentRoom.Area.WeatherY == y);

            players.ToList().ForEach(p => p.Character.SendTo(txt));
        }
    }
}
