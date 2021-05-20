using System.Linq;

namespace Realm.Library.Common.Extensions

{
    /// <summary>
    ///
    /// </summary>
    public static class CharExtensions
    {
        private static readonly char[] Vowels = { 'a', 'e', 'i', 'o', 'u' };

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsVowel(this char value)
        {
            return Vowels.Any(t => value == t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDigit(this char value)
        {
            return char.IsDigit(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsSpace(this char value)
        {
            return value == '\0';
        }
    }
}