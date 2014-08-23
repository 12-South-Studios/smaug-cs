using System;

namespace SmaugCS.Constants
{
    public class ShoveValueAttribute : Attribute
    {
        public int ModValue { get; set; }

        public ShoveValueAttribute(int ModValue = 0)
        {
            this.ModValue = ModValue;
        }
    }
}
