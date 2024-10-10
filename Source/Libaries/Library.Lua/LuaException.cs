using System;

namespace Library.Lua;

/// <summary>
/// 
/// </summary>
public class LuaException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public LuaException(string message) : base(message) { }
}