using System;

namespace Realm.Library.Common.Attributes

{
    /// <summary>
    /// Class definining an attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ValueAttribute : Attribute
    {
        /// <summary>
        /// Read-Only value of the attribute
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameAttribute"/> class
        /// </summary>
        /// <param name="value">value of the attribute</param>
        public ValueAttribute(int value)
        {
            Value = value;
        }
    }
}