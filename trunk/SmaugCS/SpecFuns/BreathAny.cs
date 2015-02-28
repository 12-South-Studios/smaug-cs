using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.SpecFuns
{
    public static class BreathAny
    {
        public static bool DoSpecBreathAny(MobileInstance ch)
        {
            if (!ch.IsInCombatPosition())
                return false;

            switch (Common.SmaugRandom.Bits(3))
            {
                case 0:
                    return Dragon.DoSpecDragon(ch, "fire breath");
                case 1:
                case 2:
                    return Dragon.DoSpecDragon(ch, "lightning breath");
                case 3:
                    return BreathGas.DoSpecBreathGas(ch);
                case 4:
                    return Dragon.DoSpecDragon(ch, "acid breath");
                default:
                    return Dragon.DoSpecDragon(ch, "frost breath");
            }
        }
    }
}
