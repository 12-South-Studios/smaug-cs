using System;
using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;


namespace SmaugCS.Weather
{
    public class WeatherRangeData
    {
        public HemisphereTypes Hemisphere { get; private set; }
        public SeasonTypes Season { get; private set; }
        public ClimateTypes Climate { get; private set; }

        public int[] Temperature { get; set; }
        public int[] Pressure { get; set; }
        public int[] CloudCover { get; set; }
        public int[] Humidity { get; set; }
        public int[] Precipitation { get; set; }
        public int[] Energy { get; set; }
        public int[] WindSpeedX { get; set; }
        public int[] WindSpeedY { get; set; }

        public WeatherRangeData(HemisphereTypes hemisphere, SeasonTypes season, ClimateTypes climate)
        {
            Temperature = new int[2];
            Pressure = new int[2];
            CloudCover = new int[2];
            Humidity = new int[2];
            Precipitation = new int[2];
            Energy = new int[2];
            WindSpeedX = new int[2];
            WindSpeedY = new int[2];

            Hemisphere = hemisphere;
            Season = season;
            Climate = climate;
        }

        public void SetData(int tempLo, int tempHi, int presLo, int presHi,
                            int cloudLo, int cloudHi, int humidLo, int humidHi, int precipLo,
                            int precipHi, int energyLo, int energyHi, int windXLo, int windXHi,
                            int windYLo, int windYHi)
        {
            Temperature[0] = tempLo;
            Temperature[1] = tempHi;
            Pressure[0] = presLo;
            Pressure[1] = presHi;
            CloudCover[0] = cloudLo;
            CloudCover[1] = cloudHi;
            Humidity[0] = humidLo;
            Humidity[1] = humidHi;
            Precipitation[0] = precipLo;
            Precipitation[1] = precipHi;
            Energy[0] = energyLo;
            Energy[1] = energyHi;
            WindSpeedX[0] = windXLo;
            WindSpeedX[1] = windXHi;
            WindSpeedY[0] = windYLo;
            WindSpeedY[1] = windYHi;
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
                        .Equals("NORTH", StringComparison.OrdinalIgnoreCase) ? 0 : 1);
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
