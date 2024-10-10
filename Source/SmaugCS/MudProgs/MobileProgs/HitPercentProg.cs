using System.IO;
using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.MudProgs.MobileProgs;

public static class HitPercentProg
{
  public static bool Execute(object[] args)
  {
    MobileInstance mob = (MobileInstance)args[0];
    CharacterInstance ch = (CharacterInstance)args[1];

    if (!mob.IsNpc() || !mob.MobIndex.HasProg(MudProgTypes.HitPercent)) return false;

    foreach (MudProgData mprog in mob.MobIndex.MudProgs.Where(x => x.Type == MudProgTypes.HitPercent))
    {
      if (!int.TryParse(mprog.ArgList, out int chance))
        throw new InvalidDataException();

      if (100 * mob.CurrentHealth / mob.MaximumHealth < chance)
        return mprog.Execute(mob);
    }

    return false;
  }
}