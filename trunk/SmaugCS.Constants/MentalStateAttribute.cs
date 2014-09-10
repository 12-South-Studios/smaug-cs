using System;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class MentalStateAttribute : Attribute
    {
        public int ModValue { get; set; }
        public ConditionTypes Condition { get; set; }
    }
}
