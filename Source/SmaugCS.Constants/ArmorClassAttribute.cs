using System;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ArmorClassAttribute : Attribute
    {
        public int ModValue { get; set; }
    }
}
