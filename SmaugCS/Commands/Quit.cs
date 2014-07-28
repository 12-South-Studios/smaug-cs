using System.Collections.Generic;
using System.Linq;
using SmaugCS.Commands.Combat;
using SmaugCS.Commands.Skills;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.Commands
{
    public static class Quit
    {
        public static void do_quit(CharacterInstance ch, string argument)
        {
            if (Helpers.CheckFunctions.CheckIfNpc(ch, ch)) return;

            if (Helpers.CheckFunctions.CheckIf(ch, Helpers.HelperFunctions.IsInFightingPosition,
                "No way! You are fighting.", new List<object> {ch}, ATTypes.AT_RED)) return;

            if (Helpers.CheckFunctions.CheckIf(ch, args => ((CharacterInstance)args[0]).CurrentPosition == PositionTypes.Stunned,
                "You're not DEAD yet.", new List<object> {ch}, ATTypes.AT_BLOOD)) return;

            TimerData timer = ch.Timers.FirstOrDefault(x => x.Type == TimerTypes.RecentFight);
            if (timer != null && !ch.IsImmortal())
            {
                color.set_char_color(ATTypes.AT_RED, ch);
                color.send_to_char("Your adrenaline is pumping too hard to quit now!\r\n", ch);
                return;
            }

            // TODO: auction
            if (Helpers.CheckFunctions.CheckIf(ch, args =>
            {
                CharacterInstance actor = (CharacterInstance) args[0];
                return actor.IsPKill() && actor.wimpy > (actor.MaximumHealth/2.25f);
            }, "Your wimpy has been adjusted to the maximum level for deadlies.", new List<object> {ch}))
                Wimpy.do_wimpy(ch, "max");

            if (ch.CurrentPosition == PositionTypes.Mounted)
                Dismount.do_dismount(ch, string.Empty);

            color.set_char_color(ATTypes.AT_WHITE, ch);
            color.send_to_char("Your surroundings begin to fade as a mystical swirling vortex of colors\r\nenvelops your body... When you come to, things are not as they were.\r\n\r\n", ch);
            comm.act(ATTypes.AT_SAY, "A strange voice says, 'We await your return, $n...'", ch, null, null, ToTypes.Character);
            comm.act(ATTypes.AT_BYE, "$n has left the game.", ch, null, null, ToTypes.CanSee);
            color.set_char_color(ATTypes.AT_GREY, ch);

            // TODO quitting_char = ch;
            save.save_char_obj(ch);

            if (GameManager.Instance.SystemData.SavePets && ch.PlayerData.Pet != null)
            {
                comm.act(ATTypes.AT_BYE, "$N follows $S master into the Void.", ch, null, ch.PlayerData.Pet, ToTypes.Room);
                handler.extract_char(ch.PlayerData.Pet, true);
            }

            //if (ch.PlayerData.Clan != null)
            //   ch.PlayerData.Clan.Save();

            // TODO saving_char = null;

            for (int x = 0; x < GameConstants.MaximumWearLocations; x++)
            {
                for (int y = 0; y < GameConstants.MaximumWearLayers; y++)
                {
                    // TODO Save equipment
                }
            }

            LogManager.Instance.Log(
                string.Format("{0} has quit (Room {1}).", ch.Name, ch.CurrentRoom != null ? ch.CurrentRoom.Vnum : -1),
                LogTypes.Comm, ch.Trust);

            handler.extract_char(ch, true);
        }
    }
}
