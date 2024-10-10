using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class MovementLossAttribute : Attribute
{
    public int ModValue { get; set; }
}