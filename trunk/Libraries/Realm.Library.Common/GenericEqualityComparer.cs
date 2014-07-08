using System;
using System.Collections.Generic;

namespace Realm.Library.Common
{
    /// <summary>
    /// Class for determining equality comparisons of the given type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="comparer"></param>
        public GenericEqualityComparer(Func<T, T, bool> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            _comparer = comparer;
        }

        /// <summary>
        /// Override of Equals
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        /// <summary>
        /// Gets the hash code of the given type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
