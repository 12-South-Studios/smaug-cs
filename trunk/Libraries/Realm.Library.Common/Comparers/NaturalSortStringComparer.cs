using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Natural Sort comparer implementation that allows you to take strings
    /// that contain numeric data and sort them in a natural manner.
    /// Obtained from: http://zootfroot.blogspot.com/2009/09/natural-sort-compare-with-linq-orderby.html
    /// </summary>
    public class NaturalSortStringComparer : IComparer<string>, IDisposable
    {
        private readonly bool isAscending;
        private Dictionary<string, string[]> table = new Dictionary<string, string[]>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inAscendingOrder"></param>
        public NaturalSortStringComparer(bool inAscendingOrder = true)
        {
            isAscending = inAscendingOrder;
        }

        #region IComparer<string> Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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

            if (!table.TryGetValue(x, out x1))
            {
                x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                table.Add(x, x1);
            }

            if (!table.TryGetValue(y, out y1))
            {
                y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
                table.Add(y, y1);
            }

            int returnVal;

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] == y1[i]) continue;

                returnVal = PartCompare(x1[i], y1[i]);
                return isAscending ? returnVal : -returnVal;
            }

            if (y1.Length > x1.Length)
            {
                returnVal = 1;
            }
            else if (x1.Length > y1.Length)
            {
                returnVal = -1;
            }
            else
            {
                returnVal = 0;
            }

            return isAscending ? returnVal : -returnVal;
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
            table.Clear();
            table = null;
        }

        #endregion IDisposable
    }
}