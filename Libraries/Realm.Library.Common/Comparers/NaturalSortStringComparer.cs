﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    /// <summary>
    /// Natural Sort comparer implementation that allows you to take strings
    /// that contain numeric data and sort them in a natural manner.
    /// Obtained from: http://zootfroot.blogspot.com/2009/09/natural-sort-compare-with-linq-orderby.html
    /// </summary>
    public class NaturalSortStringComparer : IComparer<string>, IDisposable
    {
        private readonly bool _isAscending;
        private Dictionary<string, string[]> _table = new Dictionary<string, string[]>();

        public NaturalSortStringComparer(bool inAscendingOrder = true)
        {
            _isAscending = inAscendingOrder;
        }

        #region IComparer<string> Members

        [Obsolete("This compare function is not used")]
        [SuppressMessage("Microsoft.Performance", "CA1822")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "x")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "y")]
        public int Compare(string x, string y)
        {
            throw new NotImplementedException();
        }

        #endregion IComparer<string> Members

        #region IComparer<string> Members

        int IComparer<string>.Compare(string x, string y)
        {
            if (x == y)
                return 0;

            string[] x1, y1;

            if (!_table.TryGetValue(x, out x1))
            {
                x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                _table.Add(x, x1);
            }

            if (!_table.TryGetValue(y, out y1))
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
            int x, y;
            if (!int.TryParse(left, out x))
                return String.Compare(left, right, StringComparison.Ordinal);

            return !int.TryParse(right, out y)
                ? String.Compare(left, right, StringComparison.Ordinal)
                : x.CompareTo(y);
        }

        #endregion IComparer<string> Members

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _table.Clear();
            _table = null;
        }

        #endregion IDisposable
    }
}