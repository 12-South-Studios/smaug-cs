using System;
using System.Net.Sockets;

namespace Library.Network;

/// <summary>
/// 
/// </summary>
public interface INetworkUser : IClient
{
    /// <summary>
    /// 
    /// </summary>
    void OnConnect();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="socketError"></param>
    void Disconnect(SocketError socketError);

    /// <summary>
    /// 
    /// </summary>
    string Id { get; }

    /// <summary>
    /// 
    /// </summary>
    event EventHandler<NetworkEventArgs> OnNetworkMessageReceived;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void NotifyNetworkMessageReceived(object sender, NetworkEventArgs args);

    /// <summary>
    /// 
    /// </summary>
    DateTime LastMessage { get; }
}