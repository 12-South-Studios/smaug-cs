using Library.Common;

namespace SmaugCS.Common;

/// <summary>
/// 
/// </summary>
public static class TextReaderProxyExtensions
{
    private static readonly char[] TerminatorChars = ['~'];

    /// <summary>
    /// Reads a string from the TextReader into an Extended BitVector class
    /// </summary>
    /// <param name="proxy"></param>
    /// <returns></returns>
    public static ExtendedBitvector ReadBitvector(this TextReaderProxy proxy)
        => proxy.ReadString(TerminatorChars).ToBitvector();

    /// <summary>
    /// Does what ReadString does, but ensures that any carriage returns and the hash mark are trimmed
    /// </summary>
    /// <param name="proxy"></param>
    /// <returns></returns>
    public static string ReadFlagString(this TextReaderProxy proxy)
        => proxy.ReadString().TrimStart(' ').TrimEnd('\n', '\r', '~');
}