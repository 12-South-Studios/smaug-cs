using System.Collections.Generic;
using System.Linq;

namespace Realm.Library.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }

        /// <summary>
        /// Gets the index of the given object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> obj, T value)
        {
            return obj.IndexOf(value, null);
        }

        /// <summary>
        /// Gets the index of the given object using the provided equality comparer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> obj, T value, IEqualityComparer<T> comparer)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            var found = obj
                .Select((a, i) => new { a, i })
                .FirstOrDefault(x => comparer.Equals(x.a, value));
            return found?.i ?? -1;
        }

        /// <summary>
        /// Peeks at the next object using the given object as the source
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="currentObject"></param>
        /// <returns></returns>
        public static T Peek<T>(this IEnumerable<T> obj, T currentObject)
        {
            Validation.IsNotNull(obj, "obj");

            IList<T> objList = obj.ToList();
            int index = objList.IndexOf(currentObject);
            if (index == -1)
                return objList.First();
            return index >= objList.Count - 1 ? objList.Last() : objList.ElementAt(index + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="compareTo"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this IEnumerable<string> values, string compareTo)
        {
            return values.Any(value => value.EqualsIgnoreCase(compareTo));
        }
    }
}
