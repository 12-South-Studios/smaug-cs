using System;
using System.Collections.Generic;

namespace Realm.Library.Common
{
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;

        public GenericEqualityComparer(Func<T, T, bool> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
