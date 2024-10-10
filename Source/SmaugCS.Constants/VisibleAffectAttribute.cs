using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class VisibleAffectAttribute : Attribute
{
    public Enums.ATTypes ATType { get; set; }
    public string Description { get; set; }
}