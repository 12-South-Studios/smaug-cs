using System;

namespace SmaugCS.Constants
{
    public class DragValueAttribute : Attribute
    {
        public int ModValue { get; set; }

        public DragValueAttribute(int ModValue = 0)
        {
            this.ModValue = ModValue;
        }
    }
}
