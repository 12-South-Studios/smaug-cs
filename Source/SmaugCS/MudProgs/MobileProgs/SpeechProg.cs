﻿using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class SpeechProg
    {
        public static void Execute(string txt, CharacterInstance actor)
        {
            foreach (var mob in actor.CurrentRoom.Persons.OfType<MobileInstance>().Where(x => x.IsNpc()))
            {
                var mudProg = mob.MobIndex.MudProgs.FirstOrDefault(x => x.Type == MudProgTypes.Speech);
                if (mudProg == null) continue;

                if (actor.IsNpc() && ((MobileInstance)actor).MobIndex == mob.MobIndex)
                    continue;

                mud_prog.CheckIfExecuteText(mob, mudProg, txt);
            }
        }
    }
}
