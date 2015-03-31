using System;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class CharacterColorAttribute : Attribute
    {
        public Enums.ATTypes ATType { get; set; }
    }
}
