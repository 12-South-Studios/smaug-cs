using System;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CommandAttribute : Attribute
    {
        public bool NoNpc { get; set; }
    }
}
