using System;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants
{
    public class DamageResistanceAttribute : Attribute
    {
        public ResistanceTypes ResistanceType { get; set; }

        public DamageResistanceAttribute(ResistanceTypes ResistanceType = ResistanceTypes.Unknown)
        {
            this.ResistanceType = ResistanceType;
        }
    }
}
