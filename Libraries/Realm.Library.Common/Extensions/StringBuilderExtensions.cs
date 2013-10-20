using System;
using System.Text;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Static class used to extend the <see cref="StringBuilder"/> class
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Capitalizes the first letter of the String in the StringBuilder object
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <returns>Returns a new stringbuilder object</returns>
        public static StringBuilder CapitalizeFirst(this StringBuilder sb)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.Validate(sb.Length > 0);

            return sb.Replace(sb[0], char.ToUpper(sb[0]), 0, 1);
        }

        /// <summary>
        /// Returns the first index of any of the passed characters found in
        /// the String in the StringBuilder object
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="characters">Array of characters to index</param>
        /// <param name="startIndex">Index to begin searching at</param>
        /// <returns>Returns the first index in the stringbuilder class of the characters</returns>
        public static int IndexOfAny(this StringBuilder sb, char[] characters, int startIndex)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.Validate(sb.Length > 0);
            Validation.IsNotNull(characters, "characters");
            Validation.Validate(characters.Length > 0);
            Validation.Validate<ArgumentOutOfRangeException>(startIndex >= 0 && startIndex < Int32.MaxValue);
            Validation.Validate(startIndex < sb.Length);

            return sb.ToString().IndexOfAny(characters, startIndex);
        }

        /// <summary>
        /// Gets a substring from the stringbuilder object
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="startIndex">Index to begin retrieval at</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns>Returns the substring from the stringbuilder object</returns>
        public static string Substring(this StringBuilder sb, int startIndex, int length)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.Validate(sb.Length > 0);
            Validation.Validate<ArgumentOutOfRangeException>(startIndex >= 0 && startIndex < Int32.MaxValue);
            Validation.Validate<ArgumentOutOfRangeException>(length > 0 && length < Int32.MaxValue);
            Validation.Validate(length < sb.Length && startIndex < sb.Length && (length + startIndex) < sb.Length);

            return sb.ToString(startIndex, length);
        }

        /// <summary>
        /// Removes the indicated character from the stringbuilder object
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="value">Character to remove</param>
        /// <returns>Returns a new stringbuilder object</returns>
        public static StringBuilder Remove(this StringBuilder sb, char value)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.Validate(sb.Length > 0);

            for (var i = 0; i < sb.Length; )
            {
                if (sb[i] == value)
                    sb.Remove(i, 1);
                else
                    i++;
            }
            return sb;
        }

        /// <summary>
        /// Removes the number of characters from the end of the StringBuilder object
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="value">Number of characters to remove</param>
        /// <returns>Returns a new stringbuilder object</returns>
        public static StringBuilder RemoveFromEnd(this StringBuilder sb, int value)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.Validate(sb.Length > 0);
            Validation.Validate<ArgumentOutOfRangeException>(value > 0 && value < Int32.MaxValue);
            Validation.Validate(value <= sb.Length);

            return sb.Remove(sb.Length - value, value);
        }

        /// <summary>
        /// Clears the stringbuilder object
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        public static StringBuilder Clear(this StringBuilder sb)
        {
            Validation.IsNotNull(sb, "sb");

            return new StringBuilder();
        }

        /// <summary>
        /// Trim left spaces of string
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <returns>Returns a new stringbuilder object</returns>
        public static StringBuilder LTrim(this StringBuilder sb)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.Validate(sb.Length > 0);

            var length = 0;
            var num2 = sb.Length;
            while ((sb[length] == ' ') && (length < num2))
                length++;

            if (length > 0)
                sb.Remove(0, length);

            return sb;
        }

        /// <summary>
        /// Trim right spaces of string
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <returns>Returns a new stringbuilder object</returns>
        public static StringBuilder RTrim(this StringBuilder sb)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.Validate(sb.Length > 0);

            var length = sb.Length;
            var num2 = length - 1;
            while ((sb[num2] == ' ') && (num2 > -1))
                num2--;

            if (num2 < (length - 1))
                sb.Remove(num2 + 1, (length - num2) - 1);

            return sb;
        }

        /// <summary>
        /// Trim spaces around string
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <returns>Returns a new stringbuilder object</returns>
        public static StringBuilder Trim(this StringBuilder sb)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.Validate(sb.Length > 0);

            var length = 0;
            var num2 = sb.Length;
            while ((sb[length] == ' ') && (length < num2))
                length++;

            if (length > 0)
            {
                sb.Remove(0, length);
                num2 = sb.Length;
            }
            length = num2 - 1;
            while ((sb[length] == ' ') && (length > -1))
                length--;

            if (length < (num2 - 1))
                sb.Remove(length + 1, (num2 - length) - 1);

            return sb;
        }

        /// <summary>
        /// Get index of a char
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="value">Character to search for</param>
        /// <returns>Returns the index of the indicated character, -1 if not found</returns>
        public static int IndexOf(this StringBuilder sb, char value)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.Validate(sb.Length > 0);

            return IndexOf(sb, value, 0);
        }

        /// <summary>
        /// Replaces the first instance of the given string with the new one
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="oldValue">Old string to search for</param>
        /// <param name="newValue">New string to replace with</param>
        /// <returns>Returns a new stringbuilder object</returns>
        public static StringBuilder ReplaceFirst(this StringBuilder sb, String oldValue, String newValue)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.IsNotNullOrEmpty(oldValue, "oldValue");
            Validation.IsNotNullOrEmpty(newValue, "newvalue");

            var loc = sb.IndexOf(oldValue);
            sb.Remove(loc, oldValue.Length);
            sb.Insert(loc, newValue);
            return sb;
        }

        /// <summary>
        /// Get index of a char starting from a given index
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="value">Character to search for</param>
        /// <param name="startIndex">Index to begin at</param>
        /// <returns>Returns the index of the given character, -1 if not found</returns>
        public static int IndexOf(this StringBuilder sb, char value, int startIndex)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.Validate<ArgumentOutOfRangeException>(startIndex >= 0 && startIndex < Int32.MaxValue);
            Validation.Validate(startIndex < sb.Length);

            return sb.ToString().IndexOf(value, startIndex);
        }

        /// <summary>
        /// Get index of a string
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="value">String to search for</param>
        /// <returns>Returns the index of the string, -1 if not found</returns>
        public static int IndexOf(this StringBuilder sb, String value)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.IsNotNullOrEmpty(value, "value");

            return sb.ToString().IndexOf(value, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Get index of a string from a given index
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="value">String to search for</param>
        /// <param name="startIndex">Index to begin at</param>
        /// <returns>Returns the index of the given string, -1 if not found</returns>
        public static int IndexOf(this StringBuilder sb, String value, int startIndex)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.IsNotNullOrEmpty(value, "value");
            Validation.Validate<ArgumentOutOfRangeException>(startIndex >= 0 && startIndex < Int32.MaxValue);
            Validation.Validate(startIndex < sb.Length);

            return sb.ToString().IndexOf(value, startIndex, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Get index of a string with case option
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="value">String to search for</param>
        /// <returns>Returns the index of the given string, -1 if not found</returns>
        public static int IndexOfAndIgnoreCase(this StringBuilder sb, String value)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.IsNotNullOrEmpty(value, "value");

            return sb.ToString().IndexOf(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determine whether a string starts with a given text
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="value">String to search for</param>
        /// <returns>Returns true if the stringbuilder object begins with the given string</returns>
        public static bool StartsWith(this StringBuilder sb, string value)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.IsNotNullOrEmpty(value, "value");

            return sb.ToString().StartsWith(value);
        }

        /// <summary>
        /// Determine whether a string starts with a given text (with case option)
        /// </summary>
        /// <param name="sb">StringBuilder object</param>
        /// <param name="value">String to search for</param>
        /// <returns>Returns true if the stringbuilder object begins with the given string</returns>
        public static bool StartsWithAndIgnoreCase(this StringBuilder sb, string value)
        {
            Validation.IsNotNull(sb, "sb");
            Validation.IsNotNullOrEmpty(value, "value");

            return sb.ToString().ToLower().StartsWith(value);
        }
    }
}