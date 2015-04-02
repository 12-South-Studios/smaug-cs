using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.SpecFuns
{
    public static class BreathAny
    {
        private static readonly Dictionary<int, string> BreathTable = new Dictionary<int, string>
        {
            {0, "fire breath"},
            {1, "lightning breath"},
            {2, "lightning breath"},
            {4, "acid breath"}
        };

        public static bool DoSpecBreathAny(MobileInstance ch)
        {
            if (!ch.IsInCombatPosition())
                return false;

            var bits = SmaugRandom.Bits(3);
            return bits == 2
                ? Dragon.DoSpecDragon(ch, "lightning breath")
                : Dragon.DoSpecDragon(ch, BreathTable.ContainsKey(bits) ? BreathTable[bits] : "frost breath");
        }
    }
}
