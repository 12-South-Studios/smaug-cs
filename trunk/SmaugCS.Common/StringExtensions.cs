using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Realm.Library.Common;

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
        /// <returns></returns>
        public static bool IsAllUpper(this string value)
        {
            return Regex.IsMatch(value, @"^[A-Z ]+$");
        }

        /// <summary>
        /// 
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

            if (words.Length >= 2 && words[0].IsNumber())
            {
                int val;
                Int32.TryParse(words[0], out val);

                string outVal = string.Empty;
                for (int i = 1; i < words.Length; i++)
                    outVal += words[i];

                return new Tuple<int, string>(val, outVal);
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
            return new Tuple<string, string>(value.FirstWord(), value.RemoveWord(1));
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
                UInt64 num;
                UInt64.TryParse(number, out num);

                bit.SetBit(num);
                ++x;
            }

            return bit;
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
