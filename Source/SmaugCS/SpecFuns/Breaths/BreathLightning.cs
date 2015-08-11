using SmaugCS.Common;
using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns.Breaths
{
    public static class BreathLightning
    {
        public static bool Execute(MobileInstance ch, IManager dbManager)
        {
            return Dragon.Execute(ch, "lightning breath", dbManager);
        }
    }
}
