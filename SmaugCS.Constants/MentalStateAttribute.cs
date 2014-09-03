using System;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class MentalStateAttribute : Attribute
    {
        public int ModValue { get; set; }
        public ConditionTypes Condition { get; set; }

        public MentalStateAttribute(int ModValue = 0, ConditionTypes Condition = 0)
        {
            this.ModValue = ModValue;
            this.Condition = Condition;
        }
    }
}
