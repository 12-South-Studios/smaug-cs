using System;
using System.Collections.Generic;

namespace Realm.Library.Lua
{
    /// <inheritdoc />
    /// <summary>
    /// Defines an attribute for a Lua Function
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class LuaFunctionAttribute : Attribute
    {
        private readonly IList<string> _parameters;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the LuaFunctionAttribute class.
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="description">Description of the function</param>
        /// <param name="parameters">String array of parameters</param>
        public LuaFunctionAttribute(string name, string description, params string[] parameters)
        {
            Name = name;
            Description = description;
            _parameters = parameters;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the LuaFunctionAttribute class.
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="description">Description of the function</param>
        public LuaFunctionAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets the name of the function
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the documentation for the function
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the parameters for the function
        /// </summary>
        public ICollection<string> Parameters => _parameters;
    }
}