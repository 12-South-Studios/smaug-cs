using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Realm.Library.Common.Extensions
{
    /// <summary>
    /// Extension class for String objects
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convert a string to a byte array.
        /// </summary>
        public static byte[] ToByteArray(this string value)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// Gets the first word in the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstWord(this string value)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            return value.ParseWord(1, DelimiterType.Whitespace.ValueOf());
        }

        /// <summary>
        /// Gets the second word in the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SecondWord(this string value)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            return value.ParseWord(2, DelimiterType.Whitespace.ValueOf());
        }

        /// <summary>
        /// Gets the third word in the string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ThirdWord(this string value)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            return value.ParseWord(3, DelimiterType.Whitespace.ValueOf());
        }

        /// <summary>
        /// Allows a contains check to be passed a string comparison value (allows for case-insensitive contains)
        /// </summary>
        public static bool Contains(this string value, string toCheck, StringComparison comp)
        {
            Validation.IsNotNullOrEmpty(value, "value");
            Validation.IsNotNullOrEmpty(toCheck, "toCheck");

            return value.IndexOf(toCheck, comp) >= 0;
        }

        /// <summary>
        /// Replaces the first occurrence of the string
        /// </summary>
        public static string ReplaceFirst(this string value, string search, string replace)
        {
            Validation.IsNotNullOrEmpty(value, "value");
            Validation.IsNotNullOrEmpty(search, "search");
            Validation.IsNotNullOrEmpty(replace, "replace");

            var loc = value.IndexOf(search, StringComparison.Ordinal);
            if (loc != -1)
            {
                var firstHalf = value.Substring(0, loc);
                var secondHalf = value.Substring(loc + search.Length, value.Length - (loc + search.Length));
                value = firstHalf + replace + secondHalf;
            }
            return value;
        }

        /// <summary>
        /// Capitalizes the first letter of the string
        /// </summary>
        public static string CapitalizeFirst(this string value)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            return char.ToUpper(value[0]) + value.Substring(1);
        }

        /// <summary>
        /// Parses the given word from the string
        /// </summary>
        public static string ParseWord(this string value, int wordNumber, string delimiter)
        {
            Validation.IsNotNullOrEmpty(value, "value");
            Validation.Validate<ArgumentOutOfRangeException>(wordNumber >= 0 && wordNumber < int.MaxValue);
            Validation.IsNotNullOrEmpty(delimiter, "delimiter");

            var strArray = value.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return strArray.Length >= wordNumber ? strArray[wordNumber - 1] : string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="padChar"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        public static string PadStringToFront(this string value, string padChar, int totalLength)
        {
            return PadString(value, padChar, totalLength, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="padChar"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        public static string PadString(this string value, string padChar, int totalLength)
        {
            return PadString(value, padChar, totalLength, false);
        }

        /// <summary>
        /// Pads characters to the front or back of the string
        /// </summary>
        private static string PadString(this string value, string padChar, int totalLength, bool toFront)
        {
            Validation.IsNotNullOrEmpty(value, "value");
            Validation.IsNotNullOrEmpty(padChar, "padChar");
            Validation.Validate<ArgumentOutOfRangeException>(totalLength > 0 && totalLength <= int.MaxValue);

            var newstring = string.Empty;
            string returnString;

            var length = totalLength - value.Length;
            if (!toFront)
            {
                for (var i = 0; i < length; i++)
                    newstring += padChar;
                returnString = value + newstring;
            }
            else
            {
                for (var i = 0; i < length; i++)
                    newstring += padChar;

                returnString = newstring + value;
            }

            return returnString;
        }

        /// <summary>
        /// Removes the word from the string at the given index
        /// </summary>
        public static string RemoveWord(this string value, int wordNumber)
        {
            Validation.IsNotNullOrEmpty(value, "value");
            Validation.Validate<ArgumentOutOfRangeException>(wordNumber > 0 && wordNumber < int.MaxValue);

            var strArray = value.Split(DelimiterType.Whitespace.ValueOf().ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var newString = string.Empty;
            int count = 0;
            foreach (string str in strArray)
            {
                if (count == wordNumber - 1)
                {
                    count++;
                    continue;
                }
                newString += str + " ";
                count++;
            }

            // strip the last space off the end
            if (!string.IsNullOrEmpty(newString))
                newString = newString.Substring(0, newString.Length - 1);
            return newString;
        }

        /// <summary>
        /// Trims the delimiter from the given string
        /// </summary>
        public static string Trim(this string value, string delimiter)
        {
            Validation.IsNotNullOrEmpty(value, "value");
            Validation.IsNotNullOrEmpty(delimiter, "delimiter");

            value = value.TrimStart(delimiter.ToCharArray());
            value = value.TrimEnd(delimiter.ToCharArray());
            return value;
        }

        /// <summary>
        /// Converts a string to a list of words by splitting them using
        /// the passed delimiter array.
        /// </summary>
        public static ICollection<string> Split(this string value, char[] delimiters)
        {
            Validation.IsNotNullOrEmpty(value, "value");
            Validation.IsNotNull(delimiters, "delimiters");
            Validation.Validate(delimiters.Length > 0);

            return value.Split(delimiters).ToList();
        }

        /// <summary>
        /// Parses a numerical value out of the string
        /// </summary>
        public static int ParseQuantity(this string value)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            if (!value.Contains("#")) return 0;

            var qty = 0;
            var quantity = value.ParseWord(1, "#");
            try
            {
                qty = Convert.ToInt32(quantity);
                return qty;
            }
            catch (FormatException)
            {
                return qty;
            }
        }

        /// <summary>
        /// Replaces an array of characters in a string
        /// </summary>
        public static string ReplaceAll(this string value, char[] characters, char replacementCharacter)
        {
            Validation.IsNotNullOrEmpty(value, "value");
            Validation.IsNotNull(characters, "characters");
            Validation.Validate(characters.Length > 0);

            return characters.Aggregate(value, (current, c) => current.Replace(c, replacementCharacter));
        }

        /// <summary>
        /// Removes the array of characters from a string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static string RemoveAll(this string value, ICollection<char> characters)
        {
            Validation.IsNotNullOrEmpty(value, "value");
            Validation.IsNotNull(characters, "characters");
            Validation.Validate(characters.Count > 0);

            var sb = new StringBuilder();
            value.Where(t => !characters.Contains(t)).ToList().ForEach(t => sb.Append(t));
            return sb.ToString();
        }

        /// <summary>
        /// Takes a given string and modifies it using the indicated parameters, adding
        /// a new line, or capitalizing the first letter, or appending the/a/an to the front.
        /// </summary>
        public static string AddArticle(this string value, ArticleAppendOptions appendOptions = 0)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            var sb = new StringBuilder(value);
            if (appendOptions.HasFlag(ArticleAppendOptions.TheToFront))
                sb.Insert(0, "the ");
            else
            {
                /*for (int i = 0; i < vowels.Length; i++ )
                {
                    if (value[0] == i)
                        sb.Insert(0, "an ");
                }*/
                if (value[0].IsVowel())
                {
                    sb.Insert(0, "an ");
                }
                else if (value.StartsWith("the"))
                {
                    // do nothing
                }
                else
                {
                    sb.Insert(0, "a ");
                }
            }

            if (appendOptions.HasFlag(ArticleAppendOptions.CapitalizeFirstLetter))
                sb = sb.CapitalizeFirst();

            if (appendOptions.HasFlag(ArticleAppendOptions.NewLineToEnd))
                sb.Insert(0, Environment.NewLine);

            return sb.ToString();
        }

        /// <summary>
        /// Compares two strings and returns an enumerated value of the result.
        /// This function is similar to strcmp or strcasecmp in c++
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toCompare"></param>
        /// <returns></returns>
        public static CaseCompareResult CaseCompare(this string value, string toCompare)
        {
            Validation.IsNotNullOrEmpty(value, "value");

            if (value.Equals(toCompare, StringComparison.OrdinalIgnoreCase))
                return CaseCompareResult.Equal;
            return value.ToLower().GetHashCode() < toCompare.ToLower().GetHashCode()
                ? CaseCompareResult.LessThan
                : CaseCompareResult.GreaterThan;
        }
    }
}