using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns
{
    class BreathFrost
    {
        public static bool DoSpecBreathFrost(CharacterInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "frost breath");
        }
    }
}
