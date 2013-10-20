using System.IO;
using System.Xml.Serialization;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///
    /// </summary>
    public static class XMLExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXML<T>(this T obj)
        {
            Validation.IsNotNull(obj, "obj");

            var s = new XmlSerializer(obj.GetType());
            using (var writer = new StringWriter())
            {
                s.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FromXML<T>(this string obj)
        {
            Validation.IsNotNullOrEmpty(obj, "obj");

            var s = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(obj))
            {
                return s.Deserialize(reader).CastAs<T>();
            }
        }
    }
}