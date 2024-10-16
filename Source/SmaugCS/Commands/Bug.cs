﻿using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using System;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands;

public static class Bug
{
  public static void do_bug(CharacterInstance ch, string argument)
  {
    DateTime tm = DateTime.Now;

    ch.SetColor(ATTypes.AT_PLAIN);
    if (CheckFunctions.CheckIfEmptyString(ch, argument,
          "Usage:  'bug <message>'  (your location is automatically recorded)")) return;

    // TODO
    //db.append_file(ch, SystemConstants.GetSystemFile(SystemFileTypes.PBug),
    //    $"({tm.ToLongDateString()}):  {argument}");
    ch.SendTo("Thanks, your bug notice has been recorded.");
  }
}