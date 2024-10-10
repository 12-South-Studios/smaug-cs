using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ArmorClassAttribute : Attribute
{
    public int ModValue { get; set; }
}