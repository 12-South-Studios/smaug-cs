using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns
{
    public static class BreathAcid
    {
        public static bool DoSpecBreathAcid(MobileInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "acid breath");
        }
    }
}
