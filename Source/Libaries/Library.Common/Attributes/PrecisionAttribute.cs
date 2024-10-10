using System;

namespace Library.Common.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class PrecisionAttribute(double value) : Attribute
{
  public double Value { get; set; } = value;
}