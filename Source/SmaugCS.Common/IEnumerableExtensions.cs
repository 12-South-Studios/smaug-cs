using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Common
{
    public static class IEnumerableExtensions
    {
        public static T GetValue<T>(this IEnumerable<object> objects, int index)
        {
            if (index >= objects.Count()) return default;
            if (objects.ToList()[index] == null) return default;
            return (T)objects.ToList()[index];
        }
    }
}
