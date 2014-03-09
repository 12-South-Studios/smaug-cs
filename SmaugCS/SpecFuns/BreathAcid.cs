using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Data;

namespace SmaugCS.SpecFuns
{
    public static class BreathAcid
    {
        public static bool DoSpecBreathAcid(CharacterInstance ch)
        {
            return Dragon.DoSpecDragon(ch, "acid breath");
        }
    }
}
