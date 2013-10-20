using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Properties;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Static class used to extend <see cref="System.Enum"/>
    /// </summary>
    public static class EnumerationExtensions
    {
        /// <summary>
        /// Gets the value of the string name attribute from the enumeration
        /// </summary>
        /// <exception cref="ArgumentNullException">If the value is null, throws an ArgumentNullException</exception>
        public static string GetName(this Enum value)
        {
            Validation.IsNotNull(value, "value");

            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(EnumAttribute)) as EnumAttribute;
            return attribute == null ? value.ToString() : attribute.Name;
        }

        /// <summary>
        /// Gets the value of the integer value attribute from the enumeration
        /// </summary>
        /// <exception cref="ArgumentNullException">If the value is null, throws an ArgumentNullException</exception>
        public static int GetValue(this Enum value)
        {
            Validation.IsNotNull(value, "value");

            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(EnumAttribute)) as EnumAttribute;
            return attribute == null ? 0 : attribute.Value;
        }

        /// <summary>
        /// Gets the value of the string short name attribute from the enumeration
        /// </summary>
        /// <exception cref="ArgumentNullException">If the value is null, throws an ArgumentNullException</exception>
        public static string GetShortName(this Enum value)
        {
            Validation.IsNotNull(value, "value");

            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(EnumAttribute)) as EnumAttribute;
            return attribute == null ? string.Empty : attribute.ShortName;
        }

        /// <summary>
        /// Gets the value of the string extra data attribute from the enumeration
        /// </summary>
        /// <exception cref="ArgumentNullException">If the value is null, throws an ArgumentNullException</exception>
        public static string GetExtraData(this Enum value)
        {
            Validation.IsNotNull(value, "value");

            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(EnumAttribute)) as EnumAttribute;
            return attribute == null ? string.Empty : attribute.ExtraData;
        }

        /// <summary>
        /// Gets the value of the string extra data attribute and looks for the delimiter character,
        /// if found splits the data string and returns an enumerable list of values
        /// </summary>
        /// <exception cref="ArgumentNullException">If the value is null, throws an ArgumentNullException</exception>
        public static IEnumerable<string> ParseExtraData(this Enum value, string delimiter)
        {
            Validation.IsNotNull(value, "value");
            Validation.IsNotNullOrEmpty(delimiter, "delimiter");

            var extraData = value.GetExtraData();
            return !extraData.Contains(delimiter)
                ? new List<string> { extraData }
                : extraData.Split(delimiter.ToCharArray()).ToList();
        }

        /// <summary>
        /// Converts an integer value into a member of the enumeration type
        /// </summary>
        /// <exception cref="ArgumentException">If the member is not found, throws an ArgumentException</exception>
        public static T GetEnum<T>(int value)
        {
            if (Enum.IsDefined(typeof(T), value))
                return (T)Enum.ToObject(typeof(T), value);
            throw new ArgumentException(string.Format(Resources.ERR_NO_VALUE, typeof(T), value));
        }

        /// <summary>
        /// Converts a string value into a member of the enumeration type
        /// </summary>
        /// <exception cref="ArgumentException">If the member is not found, throws an ArgumentException</exception>
        public static T GetEnum<T>(string name)
        {
            if (Enum.IsDefined(typeof(T), name))
                return (T)Enum.Parse(typeof(T), name);
            throw new ArgumentException(string.Format(Resources.ERR_NO_VALUE, typeof(T), name));
        }

        /// <summary>
        /// Gets if the bit field contains the given enumeration
        /// </summary>
        public static bool HasBit(this Enum value, int bits)
        {
            return (bits & value.GetValue()) != 0;
        }
    }
}