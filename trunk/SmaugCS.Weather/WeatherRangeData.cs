﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Weather
{
    public class WeatherRangeData
    {
        public HemisphereTypes Hemisphere { get; private set; }
        public SeasonTypes Season { get; private set; }
        public ClimateTypes Climate { get; private set; }

        public IEnumerable<int> Temperature { get; private set; }
        public IEnumerable<int> Pressure { get; private set; }
        public IEnumerable<int> CloudCover { get; private set; }
        public IEnumerable<int> Humidity { get; private set; }
        public IEnumerable<int> Precipitation { get; private set; }
        public IEnumerable<int> Energy { get; private set; }
        public IEnumerable<int> WindSpeedX { get; private set; }
        public IEnumerable<int> WindSpeedY { get; private set; }

        public WeatherRangeData(HemisphereTypes hemisphere, SeasonTypes season, ClimateTypes climate)
        {
            Temperature = new List<int>();
            Pressure = new List<int>();
            CloudCover = new List<int>();
            Humidity = new List<int>();
            Precipitation = new List<int>();
            Energy = new List<int>();
            WindSpeedX = new List<int>();
            WindSpeedY = new List<int>();

            Hemisphere = hemisphere;
            Season = season;
            Climate = climate;
        }

        public void SetData(int tempLo, int tempHi, int presLo, int presHi,
                            int cloudLo, int cloudHi, int humidLo, int humidHi, int precipLo,
                            int precipHi, int energyLo, int energyHi, int windXLo, int windXHi,
                            int windYLo, int windYHi)
        {
            Temperature.ToList()[0] = tempLo;
            Temperature.ToList()[1] = tempHi;
            Pressure.ToList()[0] = presLo;
            Pressure.ToList()[1] = presHi;
            CloudCover.ToList()[0] = cloudLo;
            CloudCover.ToList()[1] = cloudHi;
            Humidity.ToList()[0] = humidLo;
            Humidity.ToList()[1] = humidHi;
            Precipitation.ToList()[0] = precipLo;
            Precipitation.ToList()[1] = precipHi;
            Energy.ToList()[0] = energyLo;
            Energy.ToList()[1] = energyHi;
            WindSpeedX.ToList()[0] = windXLo;
            WindSpeedX.ToList()[1] = windXHi;
            WindSpeedY.ToList()[0] = windYLo;
            WindSpeedY.ToList()[1] = windYHi;
        }

        public static void InitializeWeatherData(string filename)
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(filename)))
            {
                while (!proxy.EndOfStream)
                {
                    string line = proxy.ReadLine().TrimEnd(new[] { '~' });
                    string[] words = line.Split(new[] { ',' });

                    HemisphereTypes hemisphere = EnumerationExtensions.GetEnum<HemisphereTypes>(words[0]
                        .EqualsIgnoreCase("north") ? 0 : 1);
                    SeasonTypes season = EnumerationExtensions.GetEnum<SeasonTypes>(words[1]);
                    ClimateTypes climate = EnumerationExtensions.GetEnum<ClimateTypes>(words[2]);

                    WeatherRangeData data = new WeatherRangeData(hemisphere, season, climate);

                    // 3/4 = Temperature Lo/HI
                    // 5/6 = Pressure Lo/HI
                    // 7/8 = CloudCover Lo/Hi
                    // 9/10 = Humidity Lo/Hi
                    // 11/12 = Precipitation Lo/HI
                    // 13/14 = Energy Lo/HI
                    // 15/16 = WindSpeed X Lo/HI
                    // 17/18 = WindSpeed Y Lo/HI

                    WeatherConstants.WeatherData.Add(data);
                }
            }
        }

        private static WeatherRangeData CreateWeatherData(HemisphereTypes hemisphere, SeasonTypes season, ClimateTypes climate,
                                                          int tempLo, int tempHi, int presLo, int presHi,
                                                          int cloudLo, int cloudHi, int humidLo, int humidHi,
                                                          int precipLo,
                                                          int precipHi, int energyLo, int energyHi, int windXLo,
                                                          int windXHi,
                                                          int windYLo, int windYHi)
        {
            WeatherRangeData data = new WeatherRangeData(hemisphere, season, climate);
            data.SetData(tempLo, tempHi, presLo, presHi, cloudLo, cloudHi, humidLo, humidHi, precipLo, precipHi,
                         energyLo, energyHi, windXLo, windXHi, windYLo, windYHi);
            return data;
        }
    }
}
