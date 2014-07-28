using Realm.Library.Common;
using System;

namespace SmaugCS.Common
{
    public static class EnumerationExtensions
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            Validation.IsNotNull(value, "value");

            var field = value.GetType().GetField(value.ToString());
            var enumAttrib = Attribute.GetCustomAttribute(field, typeof(T)) as T;
            return enumAttrib;
        }

        public static bool HasAttribute<T>(this Enum value) where T : Attribute
        {
            Validation.IsNotNull(value, "value");

            var field = value.GetType().GetField(value.ToString());
            return Attribute.IsDefined(field, typeof(T));
        }

        public static bool IsSet(this Enum value, Enum bit)
        {
            return value.HasFlag(bit);
        }
    }
}
