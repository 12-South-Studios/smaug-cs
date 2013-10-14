﻿using Realm.Library.Common;

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
            return proxy.ReadString(new[] { '~' }).ToBitvector();
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
    }
}
