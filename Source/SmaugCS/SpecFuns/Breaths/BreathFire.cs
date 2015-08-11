using SmaugCS.Common;
using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns.Breaths
{
    public static class BreathFire
    {
        public static bool Execute(MobileInstance ch, IManager dbManager)
        {
            return Dragon.Execute(ch, "fire breath", dbManager);
        }
    }
}
