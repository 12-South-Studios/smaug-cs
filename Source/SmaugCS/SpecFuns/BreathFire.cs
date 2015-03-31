using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns
{
    public static class BreathFire
    {
        public static bool DoSpecBreathFire(MobileInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "fire breath");
        }
    }
}
