using System;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class RangeAttribute : Attribute
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public RangeAttribute()
        {
            Minimum = Int32.MinValue;
            Maximum = Int32.MaxValue;
        }
    }
}
