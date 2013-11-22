using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns
{
    class BreathAcid
    {
        public static bool spec_breath_acid(CharacterInstance ch)
        {
            return Dragon.dragon(ch, "acid breath");
        }
    }
}
