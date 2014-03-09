using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Class that handles extension functions to objects
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts to object to the given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T CastAs<T>(this object value)
        {
            Validation.IsNotNull(value, "value");

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException)
            {
                return (T)value;
            }
        }

        /// <summary>
        /// Attempts to convert the object to the given type, but if it fails
        /// rather than throwing an exception it returns the default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T TryCastAs<T>(this object value, T defaultValue)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException)
            {
                return defaultValue;
            }
            catch (FormatException)
            {
                return defaultValue;
            }
            catch (OverflowException)
            {
                return defaultValue;
            }
            catch (ArgumentNullException)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Converts the object to a nullable of the same type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? ToNullable<T>(this object value) where T : struct
        {
            T? result = null;
            if (value.IsNotNull())
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T?));
                result = (T?)converter.ConvertFrom(value);
            }
            return result;
        }

        /// <summary>
        /// Quick check for null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T obj)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            return (obj == null);
            // ReSharper restore CompareNonConstrainedGenericWithNull
        }

        /// <summary>
        /// Quick check for not-null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull<T>(this T obj)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            return (obj != null);
            // ReSharper restore CompareNonConstrainedGenericWithNull
        }

        /// <summary>
        /// Gets if the object is null or dbnull
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrDBNull<T>(this T obj)
        {
            return obj.IsNull() || obj is DBNull;
        }

        /// <summary>
        /// In case a value is null (happens only in production ;-)) then this extension method tries one of the
        /// other values that it receives as arguments and returns the first value that is not null.
        /// </summary>
        public static T OrElse<T>(this T obj, params T[] otherValues)
        {
            return obj.IsNotNull() ? obj : otherValues.FirstOrDefault(value => value.IsNotNull());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return (list is ICollection<T>)
                ? ((ICollection<T>)list).Count == 0
                : !list.Any();
        }
    }
}