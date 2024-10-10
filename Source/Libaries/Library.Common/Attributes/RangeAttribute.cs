using System;

namespace Library.Common.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class RangeAttribute : Attribute
{
  /// <summary>
  /// 
  /// </summary>
  public int Minimum { get; set; }

  /// <summary>
  /// 
  /// </summary>
  public int Maximum { get; set; }

  /// <summary>
  /// 
  /// </summary>
  public RangeAttribute()
  {
    Minimum = int.MinValue;
    Maximum = int.MaxValue;
  }
}