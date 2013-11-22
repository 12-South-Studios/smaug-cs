using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns
{
    class BreathFire
    {
        public static bool spec_breath_fire(CharacterInstance ch)
        {
            return Dragon.dragon(ch, "fire breath");
        }
    }
}
