using Realm.Library.Common;
using Realm.Library.Common.Extensions;
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

        public static bool IsSet(this Enum value, Enum bit) => value.HasFlag(bit);

        public static bool IsSet(this Enum value, int bit) => value.HasBit(bit);

        public static T GetEnum<T>(long value)
        {
            if (Enum.IsDefined(typeof(T), value))
                return (T)Enum.ToObject(typeof(T), value);
            throw new ArgumentException("value");
        }

        public static T GetEnum<T>(int value)
        {
            if (Enum.IsDefined(typeof(T), value))
                return (T)Enum.ToObject(typeof(T), value);
            throw new ArgumentException("value");
        }
    }
}
