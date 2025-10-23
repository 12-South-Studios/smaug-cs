using SmaugCS.Common.Enumerations;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Weather;
using System.Text;

namespace SmaugCS.Commands.Weather;

public static class SetWeather
{
  public static void do_setweather(CharacterInstance ch, string argument)
  {
    if (ch.IsNpc())
    {
      ch.SendTo("Mob's can't setweather.\r\n");
      return;
    }

    // if !ch.desc (no descriptor)

    if (string.IsNullOrEmpty(argument))
    {
      StringBuilder sb = new();
      sb.AppendLine("Syntax: setweather <x> <y> <field> <value>");
      sb.AppendLine(string.Empty);
      sb.AppendLine("Field being one of:");
      sb.AppendLine("  climate hemisphere");
      sb.AppendLine("Climate value being:");
      sb.AppendLine("  rainforest savanna desert steppe chapparal arctic");
      sb.AppendLine("  grasslands deciduous_forest taiga tundra alpine");
      sb.AppendLine(" Set Help Climates for information on each.");
      sb.AppendLine("Hemisphere value being:");
      sb.AppendLine("  northern southern");
      ch.SendTo(sb.ToString());
      return;
    }

    string[] args = argument.Split(' ');

    if (!int.TryParse(args[0], out var x))
    {
      ch.SendTo($"X value must be between 1 and {WeatherConstants.WeatherSizeX}");
      return;
    }
    if (!int.TryParse(args[1], out var y))
    {
      ch.SendTo($"Y value must be between 1 and {WeatherConstants.WeatherSizeY}");
      return;
    }

    WeatherCell cell = Program.WeatherManager.Weather.GetCellFromMap(x, y);
    if (cell == null)
    {
      ch.SendTo("Unknown weather cell.");
      return;
    }

    string fieldArg = args[3];
    string fieldValue = args[4];

    switch(fieldArg)
    {
      case "climate":
        HandleClimateArg(ch, cell, fieldValue);
        break;

      case "hemisphere":
        HandleHemisphereArg(ch, cell, fieldValue);
        break;

      default:
        ch.SendTo("Unknown weather field argument");
        break;
    }
  }

  private static void HandleClimateArg(CharacterInstance ch, WeatherCell cell, string value)
  {
    if (string.IsNullOrEmpty(value))
    {
      ch.SendTo("Usage: setweather <x> <y> climate <flag>");
      return;
    }

    int climate = WeatherConstants.GetClimate(value);
    if (climate < 0)
    {
      ch.SendTo($"Unknown climate flag: {value}");
      return;
    }

    cell.Climate = Library.Common.Extensions.EnumerationExtensions.GetEnum<ClimateTypes>(climate);
    ch.SendTo("Cell climate set.");
  }

  private static void HandleHemisphereArg(CharacterInstance ch, WeatherCell cell, string value)
  {
    if (string.IsNullOrEmpty(value))
    {
      ch.SendTo("Usage: setweather <x> <y> hemisphere <flag>");
      return;
    }

    int hemisphere = WeatherConstants.GetHemisphere(value);
    if (hemisphere < 0)
    {
      ch.SendTo($"Unknown hemisphere flag: {value}");
      return;
    }

    cell.Hemisphere = Library.Common.Extensions.EnumerationExtensions.GetEnum<HemisphereTypes>(hemisphere);
    ch.SendTo("Cell hemisphere set.");
  }
}