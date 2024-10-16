﻿using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.MobileProgs;

public static class SpeechProg
{
  public static void Execute(string txt, CharacterInstance actor)
  {
    foreach (MobileInstance mob in actor.CurrentRoom.Persons.OfType<MobileInstance>().Where(x => x.IsNpc()))
    {
      MudProgData mudProg = mob.MobIndex.MudProgs.FirstOrDefault(x => x.Type == MudProgTypes.Speech);
      if (mudProg == null) continue;

      if (actor.IsNpc() && ((MobileInstance)actor).MobIndex == mob.MobIndex)
        continue;

      CheckFunctions.CheckIfExecuteText(mob, mudProg, txt);
    }
  }
}