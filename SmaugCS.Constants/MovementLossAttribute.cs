using System;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class MovementLossAttribute : Attribute
    {
        public int ModValue { get; set; }
    }
}
