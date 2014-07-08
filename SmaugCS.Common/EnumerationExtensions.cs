using Realm.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
