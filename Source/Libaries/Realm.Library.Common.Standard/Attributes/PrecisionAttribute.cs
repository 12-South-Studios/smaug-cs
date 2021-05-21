using System;

namespace Realm.Library.Common.Attributes
{
    public class PrecisionAttribute : Attribute
    {
        public double Value { get; set; }

        public PrecisionAttribute(double Value)
        {
            this.Value = Value;
        }
    }
}
