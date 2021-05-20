using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common.Enumerations;
using SmaugCS.Weather.Enums;

namespace SmaugCS.Weather
{
    public static class WeatherConstants
    {
        public static readonly List<WeatherRangeData> WeatherData = new List<WeatherRangeData>();

        private static readonly Dictionary<PrecipitationTypes, List<string>> WeatherMessages =
            new Dictionary<PrecipitationTypes, List<string>>();

        public static IEnumerable<string> GetWeatherMessages(PrecipitationTypes precip)
            => WeatherMessages.ContainsKey(precip) ? WeatherMessages[precip] : new List<string>();

        public static int GetHemisphere(string type)
            => (int) EnumerationExtensions.GetEnumIgnoreCase<HemisphereTypes>(type);

        public static int GetClimate(string type) => (int) EnumerationExtensions.GetEnumIgnoreCase<ClimateTypes>(type);

        public static void InitializeWeatherMessages(string filename)
        {
            using (var proxy = new TextReaderProxy(new StreamReader(filename)))
            {
                IEnumerable<TextSection> sections = proxy.ReadSections(new[] { "#" }, null, null, "#END");
                foreach (var section in sections)
                {
                    var lines = new List<string>();
                    section.Lines.ToList().ForEach(x => lines.Add(x.TrimEnd('~')));

                    WeatherMessages.Add(EnumerationExtensions.GetEnum<PrecipitationTypes>(section.Header), lines);
                }
            }
        }

    }
}
