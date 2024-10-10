using System.ComponentModel;
using System.Reflection;

namespace Library.Common.Extensions;

/// <summary>
/// Extension class for the DelimiterType enum
/// </summary>
public static class DelimiterTypeExtensions
{
  /// <summary>
  /// Gets the string value of the enum
  /// </summary>
  /// <param name="type">enum reference</param>
  /// <returns>Returns the value string</returns>
  public static string ValueOf(this DelimiterType type)
  {
    FieldInfo fi = type.GetType().GetField(type.ToString());

    DescriptionAttribute[] attributes = (DescriptionAttribute[])fi?.GetCustomAttributes(typeof(DescriptionAttribute), false);

    return attributes is { Length: > 0 } ? attributes[0].Description : type.ToString();
  }
}