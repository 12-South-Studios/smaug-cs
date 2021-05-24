using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using System.IO;
using System.Linq;

namespace SmaugCS.MudProgs.Mobile
{
    public static class HitPercentProg
    {
        public static bool Execute(object[] args)
        {
            var mob = (MobileInstance)args[0];
            var ch = (CharacterInstance)args[1];

            if (!mob.IsNpc() || !mob.MobIndex.HasProg(MudProgTypes.HitPercent)) return false;

            foreach (var mprog in mob.MobIndex.MudProgs.Where(x => x.Type == MudProgTypes.HitPercent))
            {
                int chance;
                if (!int.TryParse(mprog.ArgList, out chance))
                    throw new InvalidDataException();

                if (100 * mob.CurrentHealth / mob.MaximumHealth < chance)
                    return mprog.Execute(mob);
            }

            return false;
        }
    }
}
