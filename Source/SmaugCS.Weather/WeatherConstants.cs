using Library.Common;
using Library.Common.Extensions;
using SmaugCS.Common.Enumerations;
using SmaugCS.Weather.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmaugCS.Weather;

public static class WeatherConstants
{
  public static readonly List<WeatherRangeData> WeatherData = [];

  private static readonly Dictionary<PrecipitationTypes, List<string>> WeatherMessages = [];
    private static readonly string[] HeaderChars = ["#"];

    public static IEnumerable<string> GetWeatherMessages(PrecipitationTypes precip)
    => WeatherMessages.TryGetValue(precip, out List<string> value) ? value : [];

  public static int GetHemisphere(string type)
    => (int)EnumerationExtensions.GetEnumIgnoreCase<HemisphereTypes>(type);

  public static int GetClimate(string type) => (int)EnumerationExtensions.GetEnumIgnoreCase<ClimateTypes>(type);

  public static void InitializeWeatherMessages(string filename)
  {
    using TextReaderProxy proxy = new(new StreamReader(filename));
    IEnumerable<TextSection> sections = proxy.ReadSections(HeaderChars, null, null, "#END");
    foreach (TextSection section in sections)
    {
      List<string> lines = [];
      section.Lines.ToList().ForEach(x => lines.Add(x.TrimEnd('~')));

      WeatherMessages.Add(EnumerationExtensions.GetEnum<PrecipitationTypes>(section.Header), lines);
    }
  }
}