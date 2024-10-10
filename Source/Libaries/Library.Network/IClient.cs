using System;
using System.Threading.Tasks;

namespace Library.Network;

/// <summary>
/// 
/// </summary>
public interface IClient
{
    /// <summary>
    /// 
    /// </summary>
    string IpAddress { get; }
    
    /// <summary>
    /// 
    /// </summary>
    DateTime ConnectedOn { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    Task Write(string msg);
}