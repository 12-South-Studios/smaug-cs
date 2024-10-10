using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ImmortalHelpCategoryAttribute : Attribute
{
    public string Value { get; set; }
}