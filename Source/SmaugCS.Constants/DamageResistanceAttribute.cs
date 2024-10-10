using SmaugCS.Constants.Enums;
using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class DamageResistanceAttribute : Attribute
{
    public ResistanceTypes ResistanceType { get; set; }
}