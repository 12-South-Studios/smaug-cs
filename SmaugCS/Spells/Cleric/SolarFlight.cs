using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;
using SmaugCS.Weather;

namespace SmaugCS.Spells
{
    public static class SolarFlight
    {
        public static ReturnTypes spell_solar_flight(int sn, int level, CharacterInstance ch, object vo)
        {
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);
            WeatherCell cell = WeatherManager.Instance.GetWeather(ch.CurrentRoom.Area);

            CharacterInstance victim = ch.GetCharacterInWorld(Cast.TargetName);
            if (victim == null || victim == ch
                || (GameManager.Instance.GameTime.Hour > 18 || GameManager.Instance.GameTime.Hour < 8)
                || victim.CurrentRoom == null
                || !ch.IsOutside()
                || !victim.IsOutside()
                || cell.CloudCover >= 20 // what does this value mean?
                || victim.CurrentRoom.Flags.IsSet(RoomFlags.Private)
                || victim.CurrentRoom.Flags.IsSet(RoomFlags.Solitary)
                || victim.CurrentRoom.Flags.IsSet(RoomFlags.NoAstral)
                || victim.CurrentRoom.Flags.IsSet(RoomFlags.Death)
                || victim.CurrentRoom.Flags.IsSet(RoomFlags.Prototype)
                || ch.CurrentRoom.Flags.IsSet(RoomFlags.NoRecall)
                || victim.Level >= level + 15
                || (victim.CanPKill() && !ch.IsNpc() && !ch.IsPKill())
                || (victim.IsNpc() && victim.Act.IsSet(ActFlags.Prototype))
                || (victim.IsNpc() && victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
                || !victim.CurrentRoom.Area.InHardRange(ch)
                || (victim.CurrentRoom.Area.Flags.IsSet(AreaFlags.NoPKill) && ch.IsPKill()))
            {
                ch.FailedCast(skill, victim);
                return ReturnTypes.SpellFailed;
            }

            comm.act(ATTypes.AT_MAGIC, "$n disappears in a blinding flash of light!", ch, null, null, ToTypes.Room);
            ch.CurrentRoom.FromRoom(ch);
            victim.CurrentRoom.ToRoom(ch);
            comm.act(ATTypes.AT_MAGIC, "$n appears in a blinding flash of light!", ch, null, null, ToTypes.Room);
            Look.do_look(ch, "auto");
            return ReturnTypes.None;
        }
    }
}
