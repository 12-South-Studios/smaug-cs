using System.Diagnostics.CodeAnalysis;
using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Managers;
using SmaugCS.Repository;
using SmaugCS.Weather;

namespace SmaugCS.Spells
{
    public static class SolarFlight
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "vo")]
        public static ReturnTypes spell_solar_flight(int sn, int level, CharacterInstance ch, object vo)
        {
            var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);
            var cell = WeatherManager.Instance.GetWeather(ch.CurrentRoom.Area);

            var victim = ch.GetCharacterInWorld(Cast.TargetName);

            var lvl = GetModdedLevel(level);

            if (CheckFunctions.CheckIfTrueCasting(victim == null || victim == ch, skill, ch, CastingFunctionType.Failed,
                victim)) return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrueCasting(GameManager.Instance.GameTime.Hour > 18 
                || GameManager.Instance.GameTime.Hour < 8, skill, ch, CastingFunctionType.Failed, victim)) 
                return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfNullObjectCasting(victim.CurrentRoom, skill, ch, CastingFunctionType.Failed,
                victim)) return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrueCasting(!ch.IsOutside() && !victim.IsOutside(), skill, ch,
                CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrueCasting(cell.CloudCover >= 20, skill, ch, CastingFunctionType.Failed, victim))
                return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrueCasting(AreInvalidRoomFlagsSet(victim.CurrentRoom), skill, ch,
                CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrueCasting(ch.CurrentRoom.Flags.IsSet(RoomFlags.NoRecall), skill, ch,
                CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrueCasting(victim.Level >= lvl, skill, ch, CastingFunctionType.Failed, victim))
                return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrueCasting(victim.CanPKill() && !ch.IsNpc() && !ch.IsPKill(), skill, ch,
                CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrueCasting(victim.IsNpc() 
                && victim.SavingThrows.CheckSaveVsSpellStaff(level, victim), skill, ch,CastingFunctionType.Failed, 
                victim)) return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrueCasting(!victim.CurrentRoom.Area.IsInHardRange(ch), skill, ch,
                CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;
            if (CheckFunctions.CheckIfTrueCasting(
                victim.CurrentRoom.Area.Flags.IsSet(AreaFlags.NoPlayerVsPlayer) && ch.IsPKill(), skill, ch,
                CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;

            comm.act(ATTypes.AT_MAGIC, "$n disappears in a blinding flash of light!", ch, null, null, ToTypes.Room);
            ch.CurrentRoom.RemoveFrom(ch);
            victim.CurrentRoom.AddTo(ch);
            comm.act(ATTypes.AT_MAGIC, "$n appears in a blinding flash of light!", ch, null, null, ToTypes.Room);
            Look.do_look(ch, "auto");
            return ReturnTypes.None;
        }

        private static bool AreInvalidRoomFlagsSet(RoomTemplate room)
        {
            return room.Flags.IsSet(RoomFlags.Private)
                   || room.Flags.IsSet(RoomFlags.Solitary)
                   || room.Flags.IsSet(RoomFlags.NoAstral)
                   || room.Flags.IsSet(RoomFlags.Death)
                   || room.Flags.IsSet(RoomFlags.Prototype);
        }

        private static int GetModdedLevel(int level)
        {
            checked
            {
                return level + 15;
            }
        }
    }
}
