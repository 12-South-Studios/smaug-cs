using System;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Class definining an attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumAttribute : Attribute
    {
        /// <summary>
        /// Read-Only name of the attribute
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Read-Only value of the attribute
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Read-Only short name of the attribute
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Read-Only extra string data of the attribute
        /// </summary>
        public string ExtraData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumAttribute"/> class
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        public EnumAttribute(string name = "")
        {
            Name = name;
            Value = 0;
            ShortName = string.Empty;
            ExtraData = string.Empty;
        }
    }
}