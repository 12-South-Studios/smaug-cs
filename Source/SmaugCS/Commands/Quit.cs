using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SmaugCS.Commands.Combat;
using SmaugCS.Commands.Skills;
using SmaugCS.Data;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands;

public static class Quit
{
  [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "argument")]
  public static void do_quit(CharacterInstance ch, string argument)
  {
    if (CheckFunctions.CheckIfNpc(ch, ch)) return;

    if (CheckFunctions.CheckIf(ch, HelperFunctions.IsInFightingPosition,
          "No way! You are fighting.", [ch], ATTypes.AT_RED)) return;

    if (CheckFunctions.CheckIf(ch, args => ((CharacterInstance)args[0]).CurrentPosition == PositionTypes.Stunned,
          "You're not DEAD yet.", [ch], ATTypes.AT_BLOOD)) return;

    TimerData timer = ch.Timers.FirstOrDefault(x => x.Type == TimerTypes.RecentFight);
    if (timer != null && !ch.IsImmortal())
    {
      ch.SetColor(ATTypes.AT_RED);
      ch.SendTo("Your adrenaline is pumping too hard to quit now!");
      return;
    }

    // TODO: auction
    if (CheckFunctions.CheckIf(ch, args =>
        {
          CharacterInstance actor = (CharacterInstance)args[0];
          return actor.IsPKill() && actor.wimpy > actor.MaximumHealth / 2.25f;
        }, "Your wimpy has been adjusted to the maximum level for deadlies.", [ch]))
      Wimpy.do_wimpy(ch, "max");

    if (ch.CurrentPosition == PositionTypes.Mounted)
      Dismount.do_dismount(ch, string.Empty);

    ch.SetColor(ATTypes.AT_WHITE);
    ch.SendTo(
      "Your surroundings begin to fade as a mystical swirling vortex of colors\r\nenvelops your body... When you come to, things are not as they were.\r\n\r\n");
    comm.act(ATTypes.AT_SAY, "A strange voice says, 'We await your return, $n...'", ch, null, null, ToTypes.Character);
    comm.act(ATTypes.AT_BYE, "$n has left the game.", ch, null, null, ToTypes.CanSee);
    ch.SetColor(ATTypes.AT_GREY);

    // TODO quitting_char = ch;
    save.save_char_obj(ch);

    if (Program.GameManager.SystemData.SavePets && ((PlayerInstance)ch).PlayerData.Pet != null)
    {
      comm.act(ATTypes.AT_BYE, "$N follows $S master into the Void.", ch, null, ((PlayerInstance)ch).PlayerData.Pet,
        ToTypes.Room);
      ((PlayerInstance)ch).PlayerData.Pet.Extract(true);
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

    Program.LogManager.Info(
      $"{ch.Name} has quit (Room {ch.CurrentRoom?.Id ?? -1}).",
      LogTypes.Info, ch.Trust);

    ch.Extract(true);
  }
}