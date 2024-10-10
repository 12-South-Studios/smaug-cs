using System;

namespace Library.Network;

/// <summary>
/// 
/// </summary>
public interface INetworkServer
{
    /// <summary>
    /// 
    /// </summary>
    event EventHandler<NetworkEventArgs> OnNetworkMessageReceived;
    
    /// <summary>
    /// 
    /// </summary>
    event EventHandler<NetworkEventArgs> OnServerStatusChanged;
    
    /// <summary>
    /// 
    /// </summary>
    ServerStatus Status { get; }
    
    /// <summary>
    /// 
    /// </summary>
    bool HasConnectedUsers { get; }

    /// <summary>
    /// 
    /// </summary>
    void Startup();
    
    /// <summary>
    /// 
    /// </summary>
    void Shutdown();
}