using Library.Common.Objects;
using System.IO;
using System.Xml.Serialization;

namespace Library.Common.Serializers;

/// <summary>
///
/// </summary>
public static class XmlExtensions
{
  /// <summary>
  ///
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="obj"></param>
  /// <returns></returns>
  public static string ToXml<T>(this T obj)
  {
    Validation.IsNotNull(obj, "obj");

    XmlSerializer s = new(obj.GetType());
    using StringWriter writer = new();
    s.Serialize(writer, obj);
    return writer.ToString();
  }

  /// <summary>
  ///
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="obj"></param>
  /// <returns></returns>
  public static T FromXml<T>(this string obj)
  {
    Validation.IsNotNullOrEmpty(obj, "obj");

    XmlSerializer s = new(typeof(T));
    using StringReader reader = new(obj);
    return s.Deserialize(reader).CastAs<T>();
  }
}