using System.Collections.Generic;

namespace Library.Lua;

/// <summary>
///
/// </summary>
public class LuaResponse
{
    /// <summary>
    ///
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<object> ResponseData { get; set; }
}