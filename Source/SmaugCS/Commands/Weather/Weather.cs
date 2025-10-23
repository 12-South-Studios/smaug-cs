using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Weather;
using SmaugCS.Weather.Enums;
using System.Collections.Generic;
using System.Text;

namespace SmaugCS.Commands.Weather;

public static class Weather
{
  public static void do_weather(CharacterInstance ch, string argument)
  {
    if (ch.IsNpc())
    {
      ch.SendTo("Mob's can't check the weather.");
      return;
    }

    // descriptor

    if (!ch.IsOutside())
    {
      ch.SendTo("You need to be outside to do that!");
      return;
    }

    WeatherCell cell = Program.WeatherManager.Weather.GetCellFromMap(x, y);
    if (cell == null)
    {
      ch.SendTo("Unknown weather cell.");
      return;
    }

    StringBuilder sb = new();
    sb.AppendLine("&wAs you check the weather around you, you notice:");

    HandlePrecipitation(sb, cell);
    HandleCloudCover(sb, cell);
    HandleHumidity(sb, cell);
    HandleWind(sb, cell);
    HandleTemperature(sb, cell);
  }

  private static readonly Dictionary<PrecipitationTypes, string> _freezingPrecipMessages = new()
  {
    { PrecipitationTypes.Torrential, "&WThe snow is creating such a blizzard you can barely see!&D" },
    { PrecipitationTypes.CatsAndDogs, "&WThe snow is creating a near solid wall of white!&D" },
    { PrecipitationTypes.Pouring, "&WThe snow is coming down hard.&D" },
    { PrecipitationTypes.Heavily, "&WThe snow falls heavily.&D" },
    { PrecipitationTypes.Downpour, "&WThe snow is coming down in heavy waves.&D" },
    { PrecipitationTypes.Steadily, "&WThe snow appears to be falling pretty steadily.&D" },
    { PrecipitationTypes.Raining, "&WSnowflakes drift down from the heavens.&D" },
    { PrecipitationTypes.Lightly, "&WA light snow falls around you.&D" },
    { PrecipitationTypes.Drizzling, "&WSnow flurries about you.&D" },
    { PrecipitationTypes.Misting, "&WA few scattered snowflakes can be seen.&D" }
  };

  private static readonly Dictionary<PrecipitationTypes, string> _nonFreezingPrecipMessages = new()
  {
    { PrecipitationTypes.Torrential, "&WThe rain is coming down in torrents!&D" },
    { PrecipitationTypes.CatsAndDogs, "&BThe rain is coming down in big heavy drops!&D" },
    { PrecipitationTypes.Pouring, "&BThe rain is pouring down.&D" },
    { PrecipitationTypes.Heavily, "&BThe rain falls heavily.&D" },
    { PrecipitationTypes.Downpour, "&BThe rain is coming down in sheets.&D" },
    { PrecipitationTypes.Steadily, "&BThe rain appears to be falling pretty steadily.&D" },
    { PrecipitationTypes.Raining, "&BRain falls from the sky.&D" },
    { PrecipitationTypes.Lightly, "&BA light rain patters on the ground around you.&D" },
    { PrecipitationTypes.Drizzling, "&BA light drizzle seems to be falling.&D" },
    { PrecipitationTypes.Misting, "&BA light mist appears to be falling.&D" },
    { PrecipitationTypes.None, "&BThere doesn't appear to be any form of precipitation.&D" }
  };

  private static void HandlePrecipitation(StringBuilder sb, WeatherCell cell)
  {
    PrecipitationTypes value = WeatherCell.GetPrecipitation(cell.Precipitation);
    if (cell.IsFreezing)
      sb.AppendLine(_freezingPrecipMessages[value]);
    else
      sb.AppendLine(_nonFreezingPrecipMessages[value]);
  }

  private static Dictionary<CloudCoverTypes, string> _cloudCoverMessages = new()
  {
    { CloudCoverTypes.Extremely, "&wA blanket of clouds covers the sky.&D" },
    { CloudCoverTypes.Moderately, "&wThere looks to be a good bit of clouds in the sky.&D" },
    { CloudCoverTypes.Partly, "&wIt appears to be a partly cloudy sky.&D" },
    { CloudCoverTypes.Slightly, "&wThere are a few scattered clouds in the sky.&D" },
    { CloudCoverTypes.None, "&wThere don't appear to be any clouds in the sky.&D" }
  };

  private static void HandleCloudCover(StringBuilder sb, WeatherCell cell)
  {
    CloudCoverTypes value = WeatherCell.GetCloudCover(cell.CloudCover);
    sb.AppendLine(_cloudCoverMessages[value]);
  }

  private static Dictionary<HumidityTypes, string> _humidityMessages = new()
  {
    { HumidityTypes.Extremely, "&cYour skin feels sickly sticky with the extreme humidity.&D" },
    { HumidityTypes.Moderately, "&cYou feel slightly sticky because of the moderate humidity.&D" },
    { HumidityTypes.Minorly, "&cThe stickyness of your skin is barely noticeable in the minor humidity.&D" },
    { HumidityTypes.Humid, "&cThe air feels perfect against your skin.&D" },
    { HumidityTypes.None, "&cThe air seems as if to suck the moisture from your skin.&D" }
  };

  private static void HandleHumidity(StringBuilder sb, WeatherCell cell)
  {
    HumidityTypes value = WeatherCell.GetHumidity(cell.Humidity);
    sb.AppendLine(_humidityMessages[value]);
  }

  private static Dictionary<TemperatureTypes, string> _temperatureMessages = new()
  {
    { TemperatureTypes.Sweltering, "&OThe heat is almost unbearable.&D" },
    { TemperatureTypes.VeryHot, "&OIt's very hot...&D" },
    { TemperatureTypes.Hot, "&OIt's hot...&D" },
    { TemperatureTypes.Warm, "&OIt seems to be a bit warm.&D" },
    { TemperatureTypes.Temperate, "&OThe temperature feels just right.&D" },
    { TemperatureTypes.Cool, "&CIt seems to be a bit cool.&D" },
    { TemperatureTypes.Chilly, "&CIt seems a bit chilly.&D" },
    { TemperatureTypes.Cold, "&CIt's cold.&D" },
    { TemperatureTypes.Frosty, "&CThere is visible frost around.&D" },
    { TemperatureTypes.Freezing, "&CYour breath seems to crystalize before your face.&D" },
    { TemperatureTypes.ReallyCold, "&CIt's really cold...&D" },
    { TemperatureTypes.VeryCold, "&CYou think you see ice forming on your clothes.&D" },
    { TemperatureTypes.ExtremelyCold, "&CYou feel ice clinging to your skin. Get inside!&D" }
  };

  private static void HandleTemperature(StringBuilder sb, WeatherCell cell)
  {
    if (cell.Temperature <= -30 || cell.Temperature > 100) return;

    TemperatureTypes value = WeatherCell.GetTemperature(cell.Temperature);
    sb.AppendLine(_temperatureMessages[value]);
  }

  private static void HandleWind(StringBuilder sb, WeatherCell cell)
  {
    WindSpeedTypes windX = WeatherCell.GetWindSpeed(cell.WindSpeedX);
    WindSpeedTypes windY = WeatherCell.GetWindSpeed(cell.WindSpeedY);

    // todo weather.c has this
  }
}