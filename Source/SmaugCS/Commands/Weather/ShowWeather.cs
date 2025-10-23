using Library.Common.Extensions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Weather;
using System.Text;

namespace SmaugCS.Commands.Weather;

public static class ShowWeather
{
  public static void do_showweather(CharacterInstance ch, string argument)
  {
    if (ch.IsNpc())
    {
      ch.SendTo("Mob's can't showweather.");
      return;
    }

    // descriptor?

    if (string.IsNullOrEmpty(argument))
    {
      ch.SendTo("Syntax: showweather <x> <y>");
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

    StringBuilder sb = new();
    sb.AppendLine("Current Weather State for:");
    sb.AppendLine($"&WCell (&w{x}&W, &w{y}&W)&D");
    sb.AppendLine($"&WClimate:           &w{cell.Climate.GetName()}&D");
    sb.AppendLine($"&WHemisphere:        &w{cell.Hemisphere.GetName()}&D");
    sb.AppendLine($"&WCloud Cover:       &w{cell.CloudCover}&D");
    sb.AppendLine($"&WEnergy:            &w{cell.Energy}&D");
    sb.AppendLine($"&WTemperature:       &w{cell.Temperature}&D");
    sb.AppendLine($"&WPressure:          &w{cell.Pressure}&D");
    sb.AppendLine($"&WHumidity:          &w{cell.Humidity}&D");
    sb.AppendLine($"&WPrecipitation:     &w{cell.Precipitation}&D");
    sb.AppendLine($"&WWind Speed XAxis:  &w{cell.WindSpeedX}&D");
    sb.AppendLine($"&WWind Speed YAxis:  &w{cell.WindSpeedY}&D");
    ch.SendTo(sb.ToString());
  }
}