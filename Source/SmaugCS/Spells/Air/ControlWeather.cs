using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Helpers;
using SmaugCS.Repository;
using SmaugCS.Weather;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SmaugCS.Spells.Air
{
    public static class ControlWeather
    {
        private static readonly Dictionary<string, Action<WeatherCell, int>> WeatherChangeTable = new Dictionary
            <string, Action<WeatherCell, int>>
        {
            {"warmer", IncreaseTemp},
            {"colder", DecreaseTemp},
            {"wetter", IncreasePrecip},
            {"drier", DecreasePrecip},
            {"stormier", IncreaseEnergy},
            {"calmer", DecreaseEnergy}
        };

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "level"),
        SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "vo")]
        public static ReturnTypes spell_control_weather(int sn, int level, CharacterInstance ch, object vo)
        {
            if (CheckFunctions.CheckIfTrue(ch, !WeatherChangeTable.ContainsKey(Cast.TargetName.ToLower()),
                "What do you want to change about the weather?"))
                return ReturnTypes.SpellFailed;

            var cell = WeatherManager.Instance.GetWeather(ch.CurrentRoom.Area);
            var change = 5.GetNumberThatIsBetween(SmaugRandom.Between(5, 15) + ch.Level / 10, 15);

            WeatherChangeTable[Cast.TargetName.ToLower()].Invoke(cell, change);

            var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);
            ch.SuccessfulCast(skill);

            return ReturnTypes.None;
        }

        private static void IncreaseTemp(WeatherCell cell, int change)
        {
            cell.ChangeTemperature(change);
        }

        private static void DecreaseTemp(WeatherCell cell, int change)
        {
            cell.ChangeTemperature(-1 * change);
        }

        private static void IncreasePrecip(WeatherCell cell, int change)
        {
            cell.ChangePrecip(change);
        }

        private static void DecreasePrecip(WeatherCell cell, int change)
        {
            cell.ChangePrecip(-1 * change);
        }

        private static void IncreaseEnergy(WeatherCell cell, int change)
        {
            cell.ChangeEnergy(change);
        }

        private static void DecreaseEnergy(WeatherCell cell, int change)
        {
            cell.ChangeEnergy(-1 * change);
        }
    }
}
