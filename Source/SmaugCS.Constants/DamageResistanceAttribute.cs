using System;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DamageResistanceAttribute : Attribute
    {
        public ResistanceTypes ResistanceType { get; set; }
    }
}
