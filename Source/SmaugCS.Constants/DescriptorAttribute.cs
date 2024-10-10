using System;
using System.Collections.Generic;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
public sealed class DescriptorAttribute(params string[] messages) : Attribute
{
    public IEnumerable<string> Messages { get; private set; } = messages;
}