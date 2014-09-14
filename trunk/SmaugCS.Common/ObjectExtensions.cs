using System;
using System.Linq;
using System.Reflection;

namespace SmaugCS.Common
{
    public static class ObjectExtensions
    {
        public static T GetAttribute<T>(this object value, string memberName = "") where T : Attribute
        {
            object[] attributes;
            if (string.IsNullOrEmpty(memberName))
            {
                attributes = value.GetType().GetCustomAttributes(typeof(T), false);
                return (T)attributes.ToList().FirstOrDefault(x => x.GetType() == typeof (T));
            }

            MemberInfo[] members = value.GetType().GetMember(memberName);
            if (!members.Any())
                return null;

            attributes = members.First().GetCustomAttributes(typeof (T), false);
            return (T)attributes.ToList().FirstOrDefault(x => x.GetType() == typeof (T));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool HasAttribute<T>(this object value, string memberName = "") where T : Attribute
        {
            object[] attributes;
            if (string.IsNullOrEmpty(memberName))
            {
                attributes = value.GetType().GetCustomAttributes(typeof(T), false);
                return attributes.Any(x => x.GetType() == typeof (T));
            }

            MemberInfo[] members = value.GetType().GetMember(memberName);
            if (!members.Any())
                return false;

            attributes = members.First().GetCustomAttributes(typeof(T), false);
            return attributes.Any(x => x.GetType() == typeof(T));
        }
    }
}
