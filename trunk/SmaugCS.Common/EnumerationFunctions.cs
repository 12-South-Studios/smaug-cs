using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;

namespace SmaugCS.Common
{
    public static class EnumerationFunctions
    {
        private static void ValidateEnumType(Type t)
        {
            if (t.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");
        }

        /// <summary>
        /// Returns the matching enumeration value 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Enum GetEnumByName<T>(string name)
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<Enum>().FirstOrDefault(value => value.GetName().EqualsIgnoreCase(name));
        }

        /// <summary>
        /// Gets the values of the given enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T>()
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Returns the maximum value of an enumeration
        /// </summary>
        /// <returns></returns>
        public static int Max<T>()
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<int>().Max();
        }

        /// <summary>
        /// REturns the minimum value of an enumeration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int Min<T>()
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<int>().Min();
        }
    }
}
