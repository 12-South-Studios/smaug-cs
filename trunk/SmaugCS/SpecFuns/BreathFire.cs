using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Data;

namespace SmaugCS.SpecFuns
{
    class BreathFire
    {
        public static bool DoSpecBreathFire(CharacterInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "fire breath");
        }
    }
}
