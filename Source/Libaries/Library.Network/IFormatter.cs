using System.IO;

namespace Library.Network;

/// <summary>
/// 
/// </summary>
public interface IFormatter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    string Format(string source);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="stream"></param>
    void Enable(INetworkUser user, Stream stream);
}