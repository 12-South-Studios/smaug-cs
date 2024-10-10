using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ThirstAttribute : Attribute
{
    public int ModValue { get; set; }
}