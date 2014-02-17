using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns
{
    class BreathLightning
    {
        public static bool DoSpecBreathLightning(CharacterInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "lightning breath");
        }
    }
}
