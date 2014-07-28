using System;

namespace SmaugCS.Constants
{
    public class LookupSkillAttribute : Attribute
    {
        public string Skill { get; set; }

        public LookupSkillAttribute(string Skill = "")
        {
            this.Skill = Skill;
        }
    }
}
