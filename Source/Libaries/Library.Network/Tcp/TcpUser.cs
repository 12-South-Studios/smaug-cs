using Library.Common.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;

namespace Library.Network.Tcp;

[ExcludeFromCodeCoverage]
public class TcpUser : TcpClientWrapper, INetworkUser
{
  public event EventHandler<NetworkEventArgs> OnNetworkMessageReceived;

  [ExcludeFromCodeCoverage]
  public TcpUser(ILogWrapper log, TcpClient tcpClient, IEnumerable<IFormatter> formatters)
    : base(log, tcpClient, formatters)
  {
    Guid guid = Guid.NewGuid();
    Id = guid.ToString();
  }

  public string Id { get; }
  public DateTime LastMessage { get; private set; }

  public void OnConnect()
  {
    foreach (IFormatter formatter in Formatters)
    {
      formatter.Enable(this, ClientStream);
    }

    Log.Info($"TcpClient[{Id}, {IpAddress}] connected.");
  }

  [ExcludeFromCodeCoverage]
  public void Disconnect(SocketError socketError)
  {
    SocketAsyncEventArgs args = new();
    args.Completed += OnDisconnectCompleted;
    args.DisconnectReuseSocket = true;
    args.SocketError = socketError;
    TcpClient.Client.DisconnectAsync(args);
  }

  private void OnDisconnectCompleted(object sender, SocketAsyncEventArgs e)
  {
    IpAddress = string.Empty;
    ClientStream = null;
    ConnectedOn = DateTime.MinValue;
  }

  public void NotifyNetworkMessageReceived(object sender, NetworkEventArgs args)
  {
    LastMessage = DateTime.UtcNow;
    OnNetworkMessageReceived?.Invoke(sender, args);
  }
}