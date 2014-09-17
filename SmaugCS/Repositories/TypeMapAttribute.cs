using System;

namespace SmaugCS.Repositories
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class TypeMapAttribute : Attribute
    {
        public Type Repository { get; set; }
        public Type Object { get; set; }
    }
}
