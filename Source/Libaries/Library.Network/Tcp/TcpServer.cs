using Library.Common;
using Library.Common.Exceptions;
using Library.Common.Extensions;
using Library.Common.Logging;
using Library.Network.Mxp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Network.Tcp;
// TODO: Consider an asynchronous server ala Microsoft: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example

[ExcludeFromCodeCoverage]
public sealed class TcpServer : ITcpServer, INetworkServer
{
  private TcpListener _tcpListener;
  private Task _listener;
  private readonly NetworkSettings _settings;

  public TcpServer(NetworkSettings settings, ILogWrapper log, IUserRepository<string, TcpUser> repository,
    IEnumerable<IFormatter> formatters)
  {
    _settings = settings;

    Log = log;
    Repository = repository;
    Formatters = formatters;

    Log.Info("TcpServer initialized.");
  }

  private IEnumerable<IFormatter> Formatters { get; }
  private ILogWrapper Log { get; }
  private IUserRepository<string, TcpUser> Repository { get; }

  private IPAddress Host { get; set; }
  private int Port { get; set; }
  public ServerStatus Status { get; private set; }

  public event EventHandler<TcpNetworkEventArgs> OnTcpUserStatusChanged;
  public event EventHandler<NetworkEventArgs> OnServerStatusChanged;
  public event EventHandler<NetworkEventArgs> OnNetworkMessageReceived;

  private INetworkUser GetTcpUser(string ipAddress) => Repository.Count == 0 ? null : Repository.Get(ipAddress);

  public bool HasConnectedUsers => Repository.Count > 0;

  public void Startup()
  {
    try
    {
      //// configure the listener thread on the pre-defined port
      Log.Info($"TcpServer starting up on port {_settings.Port}");
      Status = ServerStatus.Starting;
      OnServerStatusChanged?.Invoke(this, new NetworkEventArgs { ServerStatus = Status });

      Host = _settings.IpAddress.ConvertToIpAddress();
      Port = _settings.Port;

      _tcpListener = new TcpListener(Host, Port);
      _listener = Task.Factory.StartNew(ListenForClients);
    }
    catch (ThreadStateException ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
    }
    catch (OutOfMemoryException ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
    }
  }

  public void Shutdown() => Shutdown("Shutting down");

  public void Shutdown(string message)
  {
    Status = ServerStatus.ShuttingDown;
    OnServerStatusChanged?.Invoke(this, new NetworkEventArgs { ServerStatus = Status });

    Parallel.ForEach(Repository.Values, user =>
    {
      IClient tcp = user;
      tcp?.Write(message);
      user.Disconnect(SocketError.Shutdown);
    });

    _listener?.Dispose();
    if (_tcpListener != null)
    {
      _tcpListener.Stop();
      _tcpListener = null;
    }

    Log.Info("TcpServer Shutdown.");
    Status = ServerStatus.Shutdown;

    OnServerStatusChanged?.Invoke(this, new NetworkEventArgs { ServerStatus = Status });
  }

  public bool DisconnectUser(string id)
  {
    INetworkUser user = GetTcpUser(id);
    if (user == null) return false;

    user.Disconnect(SocketError.Disconnecting);
    return true;
  }

  private void ListenForClients()
  {
    try
    {
      _tcpListener.Start();
      Log.Info($"TcpServer Listening on {_tcpListener.LocalEndpoint}");
      Status = ServerStatus.Listening;

      OnServerStatusChanged?.Invoke(this, new NetworkEventArgs { ServerStatus = Status });

      while (true)
      {
        //// blocks until a client has connected to the server
        if (_listener.Status != TaskStatus.Running)
        {
          Log.Info($"Listener Task state {_listener.Status}");
          break;
        }

        TcpUser user = new(Log, _tcpListener.AcceptTcpClient(), Formatters);
        user.OnConnect();

        if (_settings.AllowMxp)
          user.ClientStream.SendMxpNegotiation();

        Repository.Add(user.Id, user);

        OnTcpUserStatusChanged?.Invoke(user, new TcpNetworkEventArgs { SocketStatus = TcpSocketStatus.Connected });

        //// create a thread to handle communication with connected client
        Task.Factory.StartNew(() => { HandleClientCommunication(user); });
      }
    }
    catch (SocketException ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
    }
  }

  private void HandleClientCommunication(object client)
  {
    try
    {
      if (client is not TcpUser user)
        throw new InstanceNotFoundException("TcpUser was not found");

      string ipAddress = user.IpAddress;
      NetworkStream clientStream = user.ClientStream;

      byte[] message = new byte[32768];

      while (true)
      {
        int bytesRead = 0;

        try
        {
          if (clientStream == null)
          {
            //// the client has disconnected from the server
            Log.Info($"Client[{user.Id}, {ipAddress}] disconnected.");
            break;
          }

          //// blocks until a client sends a message
          bytesRead = clientStream.Read(message, 0, 32768);
        }
        catch (ObjectDisposedException)
        {
          /* do nothing here, user disconnected */
        }
        catch (IOException ex)
        {
          ex.Handle(ExceptionHandlingOptions.RecordOnly, Log);
          break;
        }

        if (bytesRead == 0)
        {
          //// the client has disconnected from the server
          Log.Info($"Client[{user.Id}, {ipAddress}] disconnected.");
          break;
        }

        //// message has successfully been received
        ASCIIEncoding encoder = new();
        NetworkEventArgs eventArgs = new() { Message = encoder.GetString(message, 0, bytesRead) };
        OnNetworkMessageReceived?.Invoke(this, eventArgs);
        user.NotifyNetworkMessageReceived(this, eventArgs);
      }

      OnTcpUserStatusChanged?.Invoke(user, new TcpNetworkEventArgs { SocketStatus = TcpSocketStatus.Disconnected });

      //// lost the user
      Repository.Delete(user.Id);
      Log.Info($"Connection Lost from {ipAddress}");
    }
    catch (InstanceNotFoundException ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
    }
  }
}