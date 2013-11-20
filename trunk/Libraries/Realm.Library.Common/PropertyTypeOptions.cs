using System;

namespace Realm.Library.Common
{
    /// <summary>
    /// Flag values that describe a Property object's options
    /// </summary>
    [Flags]
    public enum PropertyTypeOptions
    {
        None = 0,
        Persistable = 1,
        Volatile = 2,
        Visible = 4
    }
}