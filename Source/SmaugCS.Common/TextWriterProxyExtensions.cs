using Library.Common;

namespace SmaugCS.Common;

/// <summary>
/// 
/// </summary>
public static class TextWriterProxyExtensions
{
    /// <summary>
    /// Writes four arguments to the TextWriterProxy
    /// </summary>
    /// <param name="proxy"></param>
    /// <param name="value"></param>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <param name="arg3"></param>
    /// <param name="arg4"></param>
    public static void Write(this TextWriterProxy proxy, string value,
        object arg1, object arg2, object arg3, object arg4) => proxy.Write(value, [arg1, arg2, arg3, arg4]);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="proxy"></param>
    /// <param name="value"></param>
    /// <param name="args"></param>
    public static void Write(this TextWriterProxy proxy, string value, params object[] args)
        => proxy.Write(string.Format(value, args));
}