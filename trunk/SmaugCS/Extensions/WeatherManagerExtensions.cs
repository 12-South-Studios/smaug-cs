﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;
using SmaugCS.Weather;

// ReSharper disable CheckNamespace
namespace SmaugCS
// ReSharper restore CheckNamespace
{
    public static class WeatherManagerExtensions
    {
        public static void InitializeWeatherMap(this IWeatherManager manager, int weatherSizeX, int weatherSizeY)
        {
            WeatherMap newMap = new WeatherMap(GameManager.Instance.GameTime, weatherSizeX, weatherSizeY);
            newMap.LoadMap(SystemFileTypes.StarMap, newMap.StarMap);
            newMap.LoadMap(SystemFileTypes.SunMap, newMap.SunMap);
            newMap.LoadMap(SystemFileTypes.MoonMap, newMap.MoonMap);
            newMap.Initialize();
            manager.Weather = newMap;
        }

        public static void WeatherMessage(this IWeatherManager manager, string txt, int x, int y)
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
