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
        public static bool spec_breath_lightning(CharacterInstance ch)
        {
            return Dragon.dragon(ch, "lightning breath");
        }
    }
}
