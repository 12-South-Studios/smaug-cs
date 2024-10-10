using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ShoveValueAttribute : Attribute
{
    public int ModValue { get; set; }
}