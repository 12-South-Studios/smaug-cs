using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Library.Common.Comparers;

/// <summary>
/// Natural Sort comparer implementation that allows you to take strings
/// that contain numeric data and sort them in a natural manner.
/// Obtained from: http://zootfroot.blogspot.com/2009/09/natural-sort-compare-with-linq-orderby.html
/// </summary>
public class NaturalSortStringComparer : IComparer<string>, IDisposable
{
  private readonly bool _isAscending;
  private Dictionary<string, string[]> _table = new();

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="inAscendingOrder"></param>
  public NaturalSortStringComparer(bool inAscendingOrder = true)
  {
    _isAscending = inAscendingOrder;
  }
  
  /// <summary>
  ///
  /// </summary>
  /// <param name="x"></param>
  /// <param name="y"></param>
  /// <returns></returns>
  /// <exception cref="NotImplementedException"></exception>
  [Obsolete("This compare function is not used")]
  public int Compare(string x, string y)
  {
    throw new NotImplementedException();
  }
  
  int IComparer<string>.Compare(string x, string y)
  {
    if (x == y)
      return 0;

    if (!_table.TryGetValue(x, out string[] x1))
    {
      x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
      _table.Add(x, x1);
    }

    if (!_table.TryGetValue(y, out string[] y1))
    {
      y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
      _table.Add(y, y1);
    }

    int returnVal;

    for (int i = 0; i < x1.Length && i < y1.Length; i++)
    {
      if (x1[i] == y1[i]) continue;

      returnVal = PartCompare(x1[i], y1[i]);
      return _isAscending ? returnVal : -returnVal;
    }

    if (y1.Length > x1.Length)
      returnVal = 1;
    else if (x1.Length > y1.Length)
      returnVal = -1;
    else
      returnVal = 0;

    return _isAscending ? returnVal : -returnVal;
  }

  private static int PartCompare(string left, string right)
  {
    if (!int.TryParse(left, out int x))
      return string.Compare(left, right, StringComparison.Ordinal);

    return !int.TryParse(right, out int y)
      ? string.Compare(left, right, StringComparison.Ordinal)
      : x.CompareTo(y);
  }
  
  /// <summary>
  /// Overrides the base Dispose to make this object disposable
  /// </summary>
  public void Dispose()
  {
    Dispose(true);

    // Use SupressFinalize in case a subclass
    // of this type implements a finalizer.
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Dispose of any internal resources
  /// </summary>
  /// <param name="disposing"></param>
  protected virtual void Dispose(bool disposing)
  {
    if (!disposing) return;
    _table.Clear();
    _table = null;
  }
}