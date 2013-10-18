using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common.Extensions;

namespace SmaugCS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compareTo"></param>
        /// <returns></returns>
        public static bool NotEquals(this string value, string compareTo)
        {
            return !value.Equals(compareTo);
        }

        /// <summary>
        /// Simple extension wrapper around the String.IsNullOrEmpty function
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhitespace(this string value)
        {
            return String.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Returns true if the string is completely numeric
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumber(this string value)
        {
            try
            {
                int val;
                return Int32.TryParse(value, out val);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Appends an 'a' or an 'an' to the front of a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AOrAn(this string value)
        {
            return value[0].IsVowel() ? "an " + value : "a " + value;
        }

        /// <summary>
        /// Returns true if a string contains only alphanumeric characters
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsAlphaNum(this string str)
        {
            return !string.IsNullOrEmpty(str)
                && (str.ToCharArray().All(c => Char.IsLetter(c)
                    || Char.IsNumber(c)));
        }

        /// <summary>
        /// Removes the hash-mark from the end of the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimHash(this string value)
        {
            return value.TrimEnd(new[] { '~' });
        }

        /// <summary>
        /// Given a string like 14.foo, returns 14 and foo as a tuple
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Tuple where Item1 is the number and Item2 is the remainder</returns>
        public static Tuple<int, string> NumberArgument(this string value)
        {
            string[] words = value.Split('.');

            if (words.Length >= 2)
            {
                if (IsNumber(words[0]))
                {
                    int val;
                    Int32.TryParse(words[0], out val);

                    string outVal = string.Empty;
                    for (int i = 1; i < words.Length; i++)
                        outVal += words[i];

                    return new Tuple<int, string>(val, outVal);
                }
            }

            return new Tuple<int, string>(1, value);
        }

        /// <summary>
        /// Pick off one argument from a string and return a tuple
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Tuple where Item1 is the first word and Item2 is the remainder</returns>
        /// <remarks>Was formerly known as one_argument</remarks>
        public static Tuple<string, string> FirstArgument(this string value)
        {
            string arg1 = value.FirstWord();
            string arg2 = value.RemoveWord(1);
            return new Tuple<string, string>(arg1, arg2);
        }

        /// <summary>
        /// Picks off one argument from a string and returns the rest.  Uses quotes
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Tuple where Item1 is the first word and Item2 is the remainder</returns>
        /// <remarks>Was formerly known as one_argument2</remarks>
        public static Tuple<string, string> FirstArgumentWithQuotes(this string value)
        {
            if (!value.Contains("\"") && !value.Contains("\'"))
                return value.FirstArgument();

            Tuple<string, string> returnVal = value.FirstArgument();

            // First argument starts with a quote so find end and make that the entire "first argument"
            if (returnVal.Item1.StartsWith("\"") || returnVal.Item1.StartsWith("\'"))
            {
                int index = returnVal.Item2.IndexOfAny(new[] { '\"', '\'' });
                string newStr = returnVal.Item1 + " " + returnVal.Item2.Substring(0, index + 1);
                string remainder = returnVal.Item2.Remove(0, index + 1);
                returnVal = new Tuple<string, string>(newStr, remainder);
            }

            return returnVal;
        }

        /// <summary>
        /// Checks the equality of two strings, regardless of case
        /// </summary>
        /// <param name="value"></param>
        /// <param name="compareTo"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string value, string compareTo)
        {
            return value.Equals(compareTo, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the string starts with another starts, regardless of case
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startsWith"></param>
        /// <returns></returns>
        public static bool StartsWithIgnoreCase(this string value, string startsWith)
        {
            return value.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the string contains another string, regardless of case
        /// </summary>
        /// <param name="value"></param>
        /// <param name="contains"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this string value, string contains)
        {
            return value.Contains(contains, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Converts the given string to a bitvector
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static ExtendedBitvector ToBitvector(this string argument)
        {
            ExtendedBitvector bit = new ExtendedBitvector();
            int x = 0;

            List<string> untrimmed = argument.TrimEnd('~').Split(new[] { '&' }).ToList();

            List<string> numbers = new List<string>();
            untrimmed.ForEach(s => numbers.Add(s.Trim()));

            foreach (string number in numbers)
            {
                int num;
                Int32.TryParse(number, out num);

                if (x < ExtendedBitvector.XBI)
                    bit.Bits[x] = num;
                ++x;
            }

            return bit;
        }

        /// <summary>
        /// Converts the string to a 32-bit integer value
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static int ToInt32(this string argument)
        {
            int val;
            try
            {
                val = Convert.ToInt32(argument);
            }
            catch
            {
                val = 0;
            }

            return val;
        }

        /// <summary>
        /// Converts the string to a boolean value
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string argument)
        {
            return argument.EqualsIgnoreCase("true")
                   || argument.ToInt32() == 1
                   || argument.EqualsIgnoreCase("t");
        }

        private static readonly char[] WordListDelimiters = new[] { ' ', '-' };

        /// <summary>
        /// Determines if the given string is equal to any values in the passed string list
        /// </summary>
        /// <param name="value"></param>
        /// <param name="wordList"></param>
        /// <returns></returns>
        /// <remarks>Was formerly known as is_name</remarks>
        public static bool IsEqual(this string value, string wordList)
        {
            string[] words = wordList.Split(WordListDelimiters);
            return words.Any(word => word.EqualsIgnoreCase(value));
        }

        /// <summary>
        /// Determines if the given string is equal to any of the prefix values in the passed string list
        /// </summary>
        /// <param name="value"></param>
        /// <param name="wordList"></param>
        /// <returns></returns>
        /// <remarks>Was formerly known as is_name2</remarks>
        public static bool IsEqualPrefix(this string value, string wordList)
        {
            string[] words = wordList.Split(WordListDelimiters);
            return words.Any(word => word.StartsWithIgnoreCase(value));
        }

        /// <summary>
        /// Checks all words in the given string against the passed word list
        /// </summary>
        /// <param name="value"></param>
        /// <param name="wordList"></param>
        /// <returns></returns>
        /// <remarks>Was formerly known as nifty_is_name</remarks>
        public static bool IsAnyEqual(this string value, string wordList)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            string[] wordsToCheck = value.Split(WordListDelimiters);
            return wordsToCheck.Any(word => word.IsEqual(wordList));
        }

        /// <summary>
        /// Checks all words in the given string for a prefix match against the passed word list
        /// </summary>
        /// <param name="value"></param>
        /// <param name="wordList"></param>
        /// <returns></returns>
        /// <remarks>Was formerly known as nifty_is_name_prefix</remarks>
        public static bool IsAnyEqualPrefix(this string value, string wordList)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            string[] wordsToCheck = value.Split(WordListDelimiters);
            return wordsToCheck.Any(word => word.IsEqualPrefix(wordList));
        }

        /// <summary>
        /// Repeats a character a specified number of times
        /// </summary>
        /// <param name="chatToRepeat"></param>
        /// <param name="repeat"></param>
        /// <returns></returns>
        public static string Repeat(this char chatToRepeat, int repeat)
        {
            return new string(chatToRepeat, repeat);
        }

        /// <summary>
        /// Repeats a string a specified number of times
        /// </summary>
        /// <param name="stringToRepeat"></param>
        /// <param name="repeat"></param>
        /// <returns></returns>
        public static string Repeat(this string stringToRepeat, int repeat)
        {
            var builder = new StringBuilder(repeat * stringToRepeat.Length);
            for (int i = 0; i < repeat; i++)
            {
                builder.Append(stringToRepeat);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Replaces tildes in the given string with the replacement character
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultChar"></param>
        /// <returns></returns>
        public static string SmashTilde(this string str, string defaultChar = "-")
        {
            return str.HideTilde(defaultChar);
        }

        /// <summary>
        /// Hides the tilde in teh given string, replacing it with a given character
        /// </summary>
        /// <param name="str"></param>
        /// <param name="hiddenChar"></param>
        /// <returns></returns>
        public static string HideTilde(this string str, string hiddenChar = "*")
        {
            return str.Replace("~", hiddenChar);
        }

        /// <summary>
        /// Unhides the tilde in a string, replacing the given character with a tilde
        /// </summary>
        /// <param name="str"></param>
        /// <param name="hiddenChar"></param>
        /// <returns></returns>
        public static string UnhideTilde(this string str, string hiddenChar = "*")
        {
            return str.Replace(hiddenChar, "~");
        }
    }
}
