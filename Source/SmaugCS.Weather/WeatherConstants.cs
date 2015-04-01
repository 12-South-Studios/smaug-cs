using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.Weather
{
    public static class WeatherConstants
    {
        public static readonly List<WeatherRangeData> WeatherData = new List<WeatherRangeData>();

        private static readonly Dictionary<PrecipitationTypes, List<string>> WeatherMessages =
            new Dictionary<PrecipitationTypes, List<string>>();

        public static IEnumerable<string> GetWeatherMessages(PrecipitationTypes precip)
        {
            return WeatherMessages.ContainsKey(precip) ? WeatherMessages[precip] : new List<string>();
        }

        public static int GetHemisphere(string type)
        {
            var hemisphere = EnumerationExtensions.GetEnumIgnoreCase<HemisphereTypes>(type);
            return (int)hemisphere;
        }

        public static int GetClimate(string type)
        {
            var climate = EnumerationExtensions.GetEnumIgnoreCase<ClimateTypes>(type);
            return (int)climate;
        }

        public static void InitializeWeatherMessages(string filename)
        {
            using (var proxy = new TextReaderProxy(new StreamReader(filename)))
            {
                IEnumerable<TextSection> sections = proxy.ReadSections(new[] { "#" }, null, null, "#END");
                foreach (var section in sections)
                {
                    var lines = new List<string>();
                    section.Lines.ToList().ForEach(x => lines.Add(x.TrimEnd(new[] { '~' })));

                    WeatherMessages.Add(EnumerationExtensions.GetEnum<PrecipitationTypes>(section.Header), lines);
                }
            }
        }

    }
}
