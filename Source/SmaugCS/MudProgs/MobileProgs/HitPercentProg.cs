using System;
using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class HitPercentProg
    {
        public static void Execute(MobileInstance mob, CharacterInstance ch)
        {
            if (!mob.IsNpc() || !mob.MobIndex.HasProg(MudProgTypes.HitPercent)) return;
            
            foreach (var mprog in mob.MobIndex.MudProgs.Where(x => x.Type == MudProgTypes.HitPercent))
            {
                int chance;
                if (!Int32.TryParse(mprog.ArgList, out chance))
                {
                    // TODO Exception, log it
                    return;
                }

                if ((100*mob.CurrentHealth/mob.MaximumHealth) < chance)
                {
                    mprog.Execute(mob);
                    break;
                }
            }
        }
    }
}
