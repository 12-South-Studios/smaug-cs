using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///
    /// </summary>
    public static class JSONExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON<T>(this T obj)
        {
            Validation.IsNotNull(obj, "obj");

            using (var stream = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(obj.GetType());

                ser.WriteObject(stream, obj);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FromJSON<T>(this string obj)
        {
            Validation.IsNotNullOrEmpty(obj, "obj");

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(obj)))
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(stream);
            }
        }
    }
}