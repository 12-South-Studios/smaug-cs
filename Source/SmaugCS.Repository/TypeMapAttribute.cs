using System;

namespace SmaugCS.Repository
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class TypeMapAttribute : Attribute
    {
        public Type Repository { get; set; }
        public Type Object { get; set; }
    }
}
