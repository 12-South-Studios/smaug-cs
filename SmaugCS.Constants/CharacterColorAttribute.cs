using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Constants
{
    public class CharacterColorAttribute : Attribute
    {
        public Enums.ATTypes ATType { get; private set; }

        public CharacterColorAttribute(Enums.ATTypes atType)
        {
            ATType = atType;
        }
    }
}
