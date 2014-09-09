using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns
{
    public static class BreathLightning
    {
        public static bool DoSpecBreathLightning(MobileInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "lightning breath");
        }
    }
}
