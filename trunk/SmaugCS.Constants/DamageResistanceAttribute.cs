using System;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DamageResistanceAttribute : Attribute
    {
        public ResistanceTypes ResistanceType { get; set; }
    }
}
