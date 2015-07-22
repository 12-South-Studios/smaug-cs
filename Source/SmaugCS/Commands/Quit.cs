using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SmaugCS.Commands.Combat;
using SmaugCS.Commands.Skills;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.Commands
{
    public static class Quit
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "argument")]
        public static void do_quit(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNpc(ch, ch)) return;

            if (CheckFunctions.CheckIf(ch, HelperFunctions.IsInFightingPosition,
                "No way! You are fighting.", new List<object> {ch}, ATTypes.AT_RED)) return;

            if (CheckFunctions.CheckIf(ch, args => ((CharacterInstance)args[0]).CurrentPosition == PositionTypes.Stunned,
                "You're not DEAD yet.", new List<object> {ch}, ATTypes.AT_BLOOD)) return;

            var timer = ch.Timers.FirstOrDefault(x => x.Type == TimerTypes.RecentFight);
            if (timer != null && !ch.IsImmortal())
            {
               ch.SetColor(ATTypes.AT_RED);
               ch.SendTo("Your adrenaline is pumping too hard to quit now!");
                return;
            }

            // TODO: auction
            if (CheckFunctions.CheckIf(ch, args =>
            {
                var actor = (CharacterInstance) args[0];
                return actor.IsPKill() && actor.wimpy > (actor.MaximumHealth/2.25f);
            }, "Your wimpy has been adjusted to the maximum level for deadlies.", new List<object> {ch}))
                Wimpy.do_wimpy(ch, "max");

            if (ch.CurrentPosition == PositionTypes.Mounted)
                Dismount.do_dismount(ch, string.Empty);

           ch.SetColor(ATTypes.AT_WHITE);
           ch.SendTo("Your surroundings begin to fade as a mystical swirling vortex of colors\r\nenvelops your body... When you come to, things are not as they were.\r\n\r\n");
            comm.act(ATTypes.AT_SAY, "A strange voice says, 'We await your return, $n...'", ch, null, null, ToTypes.Character);
            comm.act(ATTypes.AT_BYE, "$n has left the game.", ch, null, null, ToTypes.CanSee);
           ch.SetColor(ATTypes.AT_GREY);

            // TODO quitting_char = ch;
            save.save_char_obj(ch);

            if (GameManager.Instance.SystemData.SavePets && ((PlayerInstance)ch).PlayerData.Pet != null)
            {
                comm.act(ATTypes.AT_BYE, "$N follows $S master into the Void.", ch, null, ((PlayerInstance)ch).PlayerData.Pet, ToTypes.Room);
                ((PlayerInstance)ch).PlayerData.Pet.Extract(true);
            }

            //if (ch.PlayerData.Clan != null)
            //   ch.PlayerData.Clan.Save();

            // TODO saving_char = null;

            for (var x = 0; x < GameConstants.MaximumWearLocations; x++)
            {
                for (var y = 0; y < GameConstants.MaximumWearLayers; y++)
                {
                    // TODO Save equipment
                }
            }

            LogManager.Instance.Info(
                string.Format("{0} has quit (Room {1}).", ch.Name, ch.CurrentRoom != null ? ch.CurrentRoom.Vnum : -1),
                LogTypes.Info, ch.Trust);

            ch.Extract(true);
        }
    }
}
