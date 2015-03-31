using System;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class CommandAttribute : Attribute
    {
        public bool NoNpc { get; set; }
    }
}
