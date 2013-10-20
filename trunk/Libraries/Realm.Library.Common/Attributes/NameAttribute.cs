using System;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Class definining an attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class NameAttribute : Attribute
    {
        /// <summary>
        /// Read-Only name of the attribute
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameAttribute"/> class
        /// </summary>
        /// <param name="name">name of the attribute</param>
        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}