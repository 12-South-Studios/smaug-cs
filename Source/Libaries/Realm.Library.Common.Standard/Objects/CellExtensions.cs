using System;
using System.Globalization;

namespace Realm.Library.Common.Objects

{
    /// <summary>
    /// Class that handles extension functions to Cell objects
    /// </summary>
    public static class CellExtensions
    {
        /// <summary>
        /// Performs a case-insensitive compare of the string to the name or ID of the entity
        /// </summary>
        public static bool CompareName(this ICell value, string compareTo)
        {
            Validation.IsNotNull(value, "value");
            Validation.IsNotNullOrEmpty(compareTo, "compareTo");

            bool returnVal = false;
            if (value.Name.Equals(compareTo, StringComparison.OrdinalIgnoreCase)) returnVal = true;
            else if (value.Name.IndexOf(compareTo, StringComparison.OrdinalIgnoreCase) > -1) returnVal = true;
            else if (value.ID.ToString(CultureInfo.InvariantCulture).Equals(compareTo, StringComparison.OrdinalIgnoreCase)) returnVal = true;
            return returnVal;
        }
    }
}