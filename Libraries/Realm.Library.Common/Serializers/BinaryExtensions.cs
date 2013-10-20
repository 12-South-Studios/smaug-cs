using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///
    /// </summary>
    public static class BinaryExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T FromBinary<T>(this byte[] bytes) where T : class
        {
            Validation.IsNotNull(bytes, "bytes");
            Validation.Validate(bytes.Length > 0);

            using (var memoryStream = new MemoryStream(bytes))
            {
                var binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToBinary<T>(this T obj) where T : class
        {
            Validation.IsNotNull(obj, "obj");

            byte[] buffer = new byte[0x9c4];
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, obj);
                memoryStream.Seek(0, SeekOrigin.Begin);

                // Reallocate buffer as needed
                if (memoryStream.Length > buffer.Length)
                    // ReSharper disable RedundantAssignment
                    buffer = new byte[memoryStream.Length];
                // ReSharper restore RedundantAssignment

                buffer = memoryStream.ToArray();
            }
            return buffer;
        }
    }
}