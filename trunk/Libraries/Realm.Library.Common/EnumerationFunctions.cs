using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Realm.Library.Common
{
    /// <summary>
    /// Static class for helper functions of Enumerations
    /// </summary>
    public static class EnumerationFunctions
    {
        /// <summary>
        /// Determines if the given type of an Enum or not
        /// </summary>
        /// <param name="t"></param>
        private static void ValidateEnumType(Type t)
        {
            if (t.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");
        }

        /// <summary>
        /// Attempts to match a member of the given Enum with the name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static Enum GetEnumByName<T>(string name)
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<Enum>().FirstOrDefault(value => value.GetName().EqualsIgnoreCase(name));
        }

        /// <summary>
        /// Returns all values of the given Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetAllEnumValues<T>()
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Gets the maximum value from the given Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static int MaximumEnumValue<T>()
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<int>().Max();
        }

        /// <summary>
        /// Gets the minimum value from the given Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static int MinimumEnumValue<T>()
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<int>().Min();
        }
    }
}
