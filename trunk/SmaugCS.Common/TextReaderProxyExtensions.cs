using Realm.Library.Common;

namespace SmaugCS.Common
{
    public static class TextReaderProxyExtensions
    {
        /// <summary>
        /// Reads a string from the TextReader into an Extended BitVector class
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static ExtendedBitvector ReadBitvector(this TextReaderProxy proxy)
        {
            return proxy.ReadString("~").ToBitvector();
        }

        /// <summary>
        /// Does what ReadString does, but ensures that any carriage returns and the hash mark are trimmed
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static string ReadFlagString(this TextReaderProxy proxy)
        {
            return proxy.ReadString().TrimStart(new[] { ' ' }).TrimEnd(new[] { '\n', '\r', '~' });
        }

        /// <summary>
        /// Writes four arguments to the TextWriterProxy
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="value"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        public static void Write(this TextWriterProxy proxy, string value,
            object arg1, object arg2, object arg3, object arg4)
        {
            proxy.Write(value, new[] { arg1, arg2, arg3, arg4 });
        }
    }
}
