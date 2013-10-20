using System;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Class definining an attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumAttribute : Attribute
    {
        /// <summary>
        /// Read-Only name of the attribute
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Read-Only value of the attribute
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Read-Only short name of the attribute
        /// </summary>
        public string ShortName { get; private set; }

        /// <summary>
        /// Read-Only extra string data of the attribute
        /// </summary>
        public string ExtraData { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumAttribute"/> class
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
        /// <param name="shortName">Short Name of the attribute</param>
        /// <param name="extraData">Extra string data of the attribute</param>
        public EnumAttribute(string name = "", int value = 0, string shortName = "", string extraData = "")
        {
            Name = name;
            Value = value;
            ShortName = shortName;
            ExtraData = extraData;
        }
    }
}