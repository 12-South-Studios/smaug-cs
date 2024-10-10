using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class CharacterColorAttribute : Attribute
{
    public Enums.ATTypes ATType { get; set; }
}