using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Library.Common.Serializers;

/// <summary>
///
/// </summary>
public static class JsonExtensions
{
  /// <summary>
  ///
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="obj"></param>
  /// <returns></returns>
  public static string ToJson<T>(this T obj)
  {
    Validation.IsNotNull(obj, "obj");

    using MemoryStream stream = new();
    DataContractJsonSerializer ser = new(obj.GetType());

    ser.WriteObject(stream, obj);

    return Encoding.UTF8.GetString(stream.ToArray());
  }

  /// <summary>
  ///
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="obj"></param>
  /// <returns></returns>
  public static T FromJson<T>(this string obj)
  {
    Validation.IsNotNullOrEmpty(obj, "obj");

    using MemoryStream stream = new(Encoding.UTF8.GetBytes(obj));
    DataContractJsonSerializer ser = new(typeof(T));
    return (T)ser.ReadObject(stream);
  }
}