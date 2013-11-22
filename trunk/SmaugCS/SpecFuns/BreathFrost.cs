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
        public static bool spec_breath_frost(CharacterInstance ch)
        {
            return Dragon.dragon(ch, "frost breath");
        }
    }
}
