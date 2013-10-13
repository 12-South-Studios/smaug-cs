using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;

using SmaugCS.Weather;

namespace SmaugCS.Constants
{
    public static class WeatherConstants
    {
        public static readonly List<WeatherRangeData> WeatherData = new List<WeatherRangeData>();

        public static readonly Dictionary<PrecipitationTypes, List<string>> WeatherMessages = new Dictionary<PrecipitationTypes, List<string>>();

        public static List<string> GetWeatherMessages(PrecipitationTypes precip)
        {
            return WeatherMessages.ContainsKey(precip) ? WeatherMessages[precip] : new List<string>();
        }

        public static readonly List<string> HemisphereNames = new List<string> { "northern", "southern" };
        public static int GetHemisphere(string type)
        {
            return HemisphereNames.Contains(type)
                       ? HemisphereNames.IndexOf(type)
                       : -1;
        }

        public static readonly List<string> ClimateNames = new List<string>
                                                       {
                                                           "rainforest",
                                                           "savanna",
                                                           "desert",
                                                           "steppe",
                                                           "chapparal",
                                                           "grasslands",
                                                           "deciduous_forest",
                                                           "taiga",
                                                           "tundra",
                                                           "alpine",
                                                           "arctic"
                                                       };
        public static int GetClimate(string type)
        {
            return ClimateNames.Contains(type)
                       ? ClimateNames.IndexOf(type)
                       : -1;
        }

        public static void InitializeeWeatherMessages(string filename)
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
