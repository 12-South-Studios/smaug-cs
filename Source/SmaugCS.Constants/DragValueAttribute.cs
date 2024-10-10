using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class DragValueAttribute : Attribute
{
    public int ModValue { get; set; }
}