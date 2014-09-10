using System;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class LookupSkillAttribute : Attribute
    {
        public string Skill { get; set; }
    }
}
