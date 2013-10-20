using System;

namespace Realm.Library.Common
{
    /// <summary>
    /// Flag values that describe a Property object's options
    /// </summary>
    [Flags]
    public enum PropertyTypeOptions
    {
        /// <summary>
        /// The property has no options
        /// </summary>
        None = 0,

        /// <summary>
        /// The property is persisted and saved to the database
        /// </summary>
        Persistable = 1,

        /// <summary>
        /// The property is volatile and can be changed
        /// </summary>
        Volatile = 2,

        /// <summary>
        /// The property is visible
        /// </summary>
        Visible = 4
    }
}