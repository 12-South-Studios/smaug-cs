using System;

namespace Realm.Library.Common
{
    /// <summary>
    /// Enumeration used to determine what type of article/action to 
    /// perform on a given string.
    /// </summary>
    [Flags]
    public enum ArticleAppendOptions
    {
        /// <summary>
        /// No action
        /// </summary>
        None = 0,

        /// <summary>
        /// Add a newline to the end (\n) of the string
        /// </summary>
        NewLineToEnd = 1,

        /// <summary>
        /// Add "The" to the front of the string
        /// </summary>
        TheToFront = 2,

        /// <summary>
        /// Capitalize the first letter of the string
        /// </summary>
        CapitalizeFirstLetter = 4
    }
}