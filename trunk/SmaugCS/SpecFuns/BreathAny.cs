using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns
{
    class BreathAny
    {
        public static bool spec_breath_any(CharacterInstance ch)
        {
            if (ch.CurrentPosition != PositionTypes.Fighting
                && ch.CurrentPosition != PositionTypes.Evasive
                && ch.CurrentPosition != PositionTypes.Defensive
                && ch.CurrentPosition != PositionTypes.Aggressive
                && ch.CurrentPosition != PositionTypes.Berserk)
                return false;

            switch (Common.SmaugRandom.Bits(3))
            {
                case 0:
                    return Dragon.dragon(ch, "fire breath");
                case 1:
                case 2:
                    return Dragon.dragon(ch, "lightning breath");
                case 3:
                    return BreathGas.spec_breath_gas(ch);
                case 4:
                    return Dragon.dragon(ch, "acid breath");
                default:
                    return Dragon.dragon(ch, "frost breath");
            }
        }
    }
}
