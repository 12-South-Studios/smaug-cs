using System;

namespace SmaugCS.Constants
{
    public class ThirstAttribute : Attribute
    {
        public int ModValue { get; set; }

        public ThirstAttribute(int ModValue = 0)
        {
            this.ModValue = ModValue;
        }
    }
}
