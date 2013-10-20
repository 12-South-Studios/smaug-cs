using System.Linq;

namespace Realm.Library.Common.Extensions
{
    /// <summary>
    ///
    /// </summary>
    public static class CharExtensions
    {
        private static readonly char[] Vowels = new[] { 'a', 'e', 'i', 'o', 'u' };

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsVowel(this char value)
        {
            return Vowels.Any(t => value == t);
        }
    }
}