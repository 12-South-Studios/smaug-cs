using System;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Declares the contract for an object that can handle properties
    /// </summary>
    public interface IPropertyContext : IContext
    {
        /// <summary>
        /// Gets the indicated property by name
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Returns an object representing the property</returns>
        T GetProperty<T>(string name);

        /// <summary>
        /// Sets the given property with value onto the object
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Object value to set</param>
        /// <param name="bits">Bit field of property bits</param>
        void SetProperty(String name, object value, PropertyTypeOptions bits = 0);

        /// <summary>
        /// Sets the given property with value onto the object
        /// </summary>
        /// <param name="prop">Enumerated property</param>
        /// <param name="value">Object value to set</param>
        /// <param name="bits">Bit field of property bits</param>
        void SetProperty(Enum prop, object value, PropertyTypeOptions bits = 0);

        /// <summary>
        /// Gets whether or not the object has the given property
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Returns whether or not the object has the property</returns>
        bool HasProperty(string name);

        /// <summary>
        /// Determines if the given property can be persisted to the database
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Returns true if the given property is persistable</returns>
        bool IsPersistable(string name);

        /// <summary>
        /// Determines if the given property can be changed
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Returns true if the given property is volatile</returns>
        bool IsVolatile(string name);

        /// <summary>
        /// Determines if the given property is visible
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Returns true if the given property is visible</returns>
        bool IsVisible(string name);

        /// <summary>
        /// Removes a given property from the dictionary
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Returns true if the remove was successful</returns>
        bool RemoveProperty(string name);

        /// <summary>
        /// Gets an enumeration of the property keys from the dictionary
        /// </summary>
        /// <returns>Returns an IEnumerable value of the property keys</returns>
        IEnumerable<string> PropertyKeys { get; }

        /// <summary>
        /// Gets the number of properties on the object
        /// </summary>
        /// <returns>Returns an integer value</returns>
        int Count { get; }

        /// <summary>
        /// Returns a string compilation representing the Persistable, Volatile and Visible flags
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>String of flags (P=Persistable, V=Volatile, I=Visible</returns>
        string GetPropertyBits(string name);
    }
}