﻿using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;

namespace SmaugCS.Weather
{
    public static class WeatherConstants
    {
        public static readonly List<WeatherRangeData> WeatherData = new List<WeatherRangeData>();

        private static readonly Dictionary<PrecipitationTypes, List<string>> WeatherMessages =
            new Dictionary<PrecipitationTypes, List<string>>();

        public static List<string> GetWeatherMessages(PrecipitationTypes precip)
        {
            return WeatherMessages.ContainsKey(precip) ? WeatherMessages[precip] : new List<string>();
        }

        public static int GetHemisphere(string type)
        {
            HemisphereTypes hemisphere = Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<HemisphereTypes>(type);
            return (int)hemisphere;
        }

        public static int GetClimate(string type)
        {
            ClimateTypes climate = Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ClimateTypes>(type);
            return (int)climate;
        }

        public static void InitializeWeatherMessages(string filename)
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(filename)))
            {
                List<TextSection> sections = proxy.ReadSections(new[] { "#" }, null, null, "#END");
                foreach (TextSection section in sections)
                {
                    List<string> lines = new List<string>();
                    section.Lines.ForEach(x => lines.Add(x.TrimEnd(new[] { '~' })));

                    WeatherMessages.Add(EnumerationExtensions.GetEnum<PrecipitationTypes>(section.Header), lines);
                }
            }
        }

    }
}