using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Realm.Library.Common.Attributes;
using Realm.Library.Common.Exceptions;
using Realm.Library.Common.Objects;
using Realm.Library.Common.Properties;

namespace Realm.Library.Common.Extensions
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetMinimum(this Enum value)
        {
            Validation.IsNotNull(value, "value");

            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof (RangeAttribute)) as RangeAttribute;
            return attribute?.Minimum ?? int.MinValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetMaximum(this Enum value)
        {
            Validation.IsNotNull(value, "value");

            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(RangeAttribute)) as RangeAttribute;
            return attribute?.Maximum ?? int.MaxValue;
        }

        /// <summary>
        /// Gets the value of the integer value attribute from the enumeration
        /// </summary>
        /// <exception cref="ArgumentNullException">If the value is null, throws an ArgumentNullException</exception>
        public static int GetValue(this Enum value)
        {
            Validation.IsNotNull(value, "value");

            var field = value.GetType().GetField(value.ToString());
            var enumAttrib = Attribute.GetCustomAttribute(field, typeof(EnumAttribute)) as EnumAttribute;
            if (enumAttrib != null) return enumAttrib.Value;
            var valueAttrib = Attribute.GetCustomAttribute(field, typeof (ValueAttribute)) as ValueAttribute;
            return valueAttrib?.Value ?? 0;
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValueInRange<T>(int value, T defaultValue)
        {
            if (typeof(T).BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            IEnumerable<T> values = GetValues<T>();
            foreach (T val in values)
            {
                Enum convertedValue = val.CastAs<Enum>();
                int min = GetMinimum(convertedValue);
                int max = GetMaximum(convertedValue);

                if ((value >= min || min == int.MinValue) && (value <= max || max == int.MaxValue))
                    return val;
            }

            return defaultValue;
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetEnumIgnoreCase<T>(string name)
        {
            foreach (T value in GetValues<T>().Where(value => value.ToString().EqualsIgnoreCase(name)))
                return value;

            throw new InvalidEnumArgumentException($"{name} not found in Enum Type {typeof (T)}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>()
        {
            // Can't use type constraints on value types, so have to do check like this
            if (typeof(T).BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            return (T[])Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetEnumByName<T>(string name)
        {
            foreach (T value in GetValues<T>())
            {
                var field = value.GetType().GetField(value.ToString());
                EnumAttribute enumAttrib = Attribute.GetCustomAttribute(field, typeof(EnumAttribute)) as EnumAttribute;
                if (enumAttrib != null && enumAttrib.Name.Equals(name))
                    return value;

                NameAttribute nameAttrib = Attribute.GetCustomAttribute(field, typeof(NameAttribute)) as NameAttribute;
                if (nameAttrib != null && nameAttrib.Name.Equals(name))
                    return value;
            }

            return GetEnumIgnoreCase<T>(name);
        }

        /// <summary>
        /// Gets if the bit field contains the given enumeration
        /// </summary>
        public static bool HasBit(this Enum value, int bits)
        {
            return (bits & value.GetValue()) != 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetAttributeValue<T>(this Enum value, string propertyName) where T : Attribute
        {
            var field = value.GetType().GetField(value.ToString());
            T attrib = Attribute.GetCustomAttribute(field, typeof(T)) as T;
            if (attrib == null)
                throw new NoCustomAttributeFoundException("Attribute {0} not found on Enumeration {1}", typeof(T),
                    value);

            return attrib.GetValue(propertyName);
        }
    }
}