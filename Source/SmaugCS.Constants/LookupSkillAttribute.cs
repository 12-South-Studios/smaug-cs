using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class LookupSkillAttribute : Attribute
{
    public string Skill { get; set; }
}