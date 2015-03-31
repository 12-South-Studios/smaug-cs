using System;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DragValueAttribute : Attribute
    {
        public int ModValue { get; set; }
    }
}
