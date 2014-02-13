using System;

namespace Realm.Library.Common.Attributes
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
