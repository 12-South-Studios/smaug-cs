using System;

namespace Library.Network.Tcp;

/// <summary>
/// 
/// </summary>
public interface ITcpServer
{
    /// <summary>
    /// 
    /// </summary>
    event EventHandler<TcpNetworkEventArgs> OnTcpUserStatusChanged;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    void Shutdown(string message);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    bool DisconnectUser(string id);
}