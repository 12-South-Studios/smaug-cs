using System;

namespace Realm.Library.Common
{
    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum ArticleAppendOptions
    {
        /// <summary>
        /// No options
        /// </summary>
        None = 0,

        /// <summary>
        /// Append a New Line (Carriage Return) to the end of the string
        /// </summary>
        NewLineToEnd = 1,

        /// <summary>
        /// Append "The" to the front of the string
        /// </summary>
        TheToFront = 2,

        /// <summary>
        /// Capitalize the first letter of the string
        /// </summary>
        CapitalizeFirstLetter = 4
    }
}