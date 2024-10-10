using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using System.IO;
using System.Linq;
using SmaugCS.Extensions;

namespace SmaugCS.MudProgs;

public static class CheckFunctions
{
  public static void CheckIfExecute(MobileInstance mob, MudProgTypes type)
  {
    foreach (MudProgData mprog in mob.MobIndex.MudProgs)
    {
      if (!int.TryParse(mprog.ArgList, out int chance))
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

    string[] words = mudProg.ArgList.Split(' ');
    foreach (string word in words.Where(txt.ContainsIgnoreCase))
      CheckIfExecute(mob, mudProg.Type);
  }
}