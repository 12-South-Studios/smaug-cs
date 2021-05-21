using System;
using System.Diagnostics.CodeAnalysis;
using LuaInterface;

namespace Realm.Library.Lua
{
    /// <summary>
    /// Static class that overrides the Lua Virtual Machine with some
    /// useful functions.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class LuaExtension
    {
        /// <summary>
        /// Returns a <see cref="LuaInterface.LuaTable"/> from the stack with the given name
        /// </summary>
        /// <param name="lua">Reference to the <see cref="LuaInterface.Lua"/> VM</param>
        /// <param name="tableName">Name of the table</param>
        /// <returns>Returns a reference to a <see cref="LuaInterface.LuaTable"/></returns>
        [ExcludeFromCodeCoverage]
        public static LuaTable GetTable(this LuaInterface.Lua lua, string tableName)
        {
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));

            return lua[tableName] as LuaTable;
        }

        /// <summary>
        /// Creates a new <see cref="LuaInterface.LuaTable"/> on the stack with the given name
        /// </summary>
        /// <param name="lua">Reference to the <see cref="LuaInterface.Lua"/> VM</param>
        /// <param name="tableName">Name of the table</param>
        /// <returns>Returns a reference to a <see cref="LuaInterface.LuaTable"/></returns>
        [ExcludeFromCodeCoverage]
        public static LuaTable CreateTable(this LuaInterface.Lua lua, string tableName)
        {
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));

            lua.NewTable(tableName);
            return lua.GetTable(tableName);
        }

        /// <summary>
        /// Creates a new <see cref="LuaInterface.LuaTable"/> on the stack
        /// </summary>
        /// <param name="lua">Reference to the <see cref="LuaInterface.Lua"/> VM</param>
        /// <returns>Returns a reference to a <see cref="LuaInterface.LuaTable"/></returns>
        [ExcludeFromCodeCoverage]
        public static LuaTable CreateTable(this LuaInterface.Lua lua)
        {
            lua.NewTable("my");
            return lua.GetTable("my");
        }
    }
}