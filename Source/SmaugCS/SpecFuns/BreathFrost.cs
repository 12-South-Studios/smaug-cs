using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns
{
    public static class BreathFrost
    {
        public static bool DoSpecBreathFrost(MobileInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "frost breath");
        }
    }
}
