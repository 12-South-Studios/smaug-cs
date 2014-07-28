using System;

namespace SmaugCS.Constants
{
    public class ArmorClassAttribute : Attribute
    {
        public int ModValue { get; set; }

        public ArmorClassAttribute(int ModValue = 0)
        {
            this.ModValue = ModValue;
        }
    }
}
