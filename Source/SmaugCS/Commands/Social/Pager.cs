using System;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Social;

public static class Pager
{
  public static void do_pager(CharacterInstance ch, string argument)
  {
    if (CheckFunctions.CheckIfNpc(ch, ch)) return;

    ch.SetColor(ATTypes.AT_NOTE);
    string firstArg = argument.FirstWord();

    if (string.IsNullOrEmpty(firstArg))
    {
      TogglePager(ch);
      return;
    }

    if (CheckFunctions.CheckIfTrue(ch, !firstArg.IsNumber(), "Set page pausing to how many lines?")) return;

    ((PlayerInstance)ch).PlayerData.PagerLineCount = Convert.ToInt32(firstArg);
    if (((PlayerInstance)ch).PlayerData.PagerLineCount < 5)
      ((PlayerInstance)ch).PlayerData.PagerLineCount = 5;

    ch.Printf("Page pausing set to {0} lines.", ((PlayerInstance)ch).PlayerData.PagerLineCount);
  }

  private static void TogglePager(CharacterInstance ch)
  {
    if (((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.PagerOn))
    {
      ch.SendTo("Pager disabled.");
      Config.do_config(ch, "-pager");
    }
    else
    {
      ch.Printf("Pager is now enabled at {0} lines.", ((PlayerInstance)ch).PlayerData.PagerLineCount);
      Config.do_config(ch, "+pager");
    }
  }
}