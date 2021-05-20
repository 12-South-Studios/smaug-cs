using System.IO;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.MudProgs
{
    public static class CheckFunctions
    {
        public static void CheckIfExecute(MobileInstance mob, MudProgTypes type)
        {
            foreach (var mprog in mob.MobIndex.MudProgs)
            {
                int chance;
                if (!int.TryParse(mprog.ArgList, out chance))
                    throw new InvalidDataException();

                if (mprog.Type != type || SmaugRandom.D100() > chance) continue;

                mprog.Execute(mob);

                if (type != MudProgTypes.Greet && type != MudProgTypes.GreetAll)
                    break;
            }
        }

        public static void CheckIfExecuteText(MobileInstance mob, MudProgData mudProg, string txt)
        {
            if (mudProg.ArgList.StartsWith("p "))
            {
                if (txt.ContainsIgnoreCase(mudProg.ArgList))
                    CheckIfExecute(mob, mudProg.Type);
                return;
            }

            var words = mudProg.ArgList.Split(' ');
            foreach (var word in words.Where(txt.ContainsIgnoreCase))
                CheckIfExecute(mob, mudProg.Type);
        }
    }
}
