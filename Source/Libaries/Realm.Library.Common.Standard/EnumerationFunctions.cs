using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Extensions;

namespace Realm.Library.Common
{
    public static class EnumerationFunctions
    {
        private static void ValidateEnumType(Type t)
        {
            if (t.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");
        }

        public static Enum GetEnumByName<T>(string name)
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<Enum>().FirstOrDefault(value => value.GetName().EqualsIgnoreCase(name));
        }

        public static IEnumerable<T> GetAllEnumValues<T>()
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static int MaximumEnumValue<T>()
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<int>().Max();
        }

        public static int MinimumEnumValue<T>()
        {
            ValidateEnumType(typeof(T));
            return Enum.GetValues(typeof(T)).Cast<int>().Min();
        }
    }
}
