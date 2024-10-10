using System;
using System.Collections.Generic;

namespace Library.Common;

public class GenericEqualityComparer<T>(Func<T, T, bool> comparer) : IEqualityComparer<T>
{
  private readonly Func<T, T, bool> _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

  public bool Equals(T x, T y)
  {
    return _comparer(x, y);
  }

  public int GetHashCode(T obj)
  {
    return obj.GetHashCode();
  }
}