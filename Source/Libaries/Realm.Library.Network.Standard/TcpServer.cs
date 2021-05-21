using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Realm.Library.Common;
using Realm.Library.Common.Exceptions;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Logging;

namespace Realm.Library.Network
{
    /// <inheritdoc />
    /// <summary>
    /// Tcp Server class handles the listening socket and management of Tcp Users
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class TcpServer : ITcpServer
    {
        private TcpListener _tcpListener;
        private Thread _listenerThread;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log"></param>
        /// <param name="repository"></param>
        /// <param name="formatters"></param>
        public TcpServer(ILogWrapper log, ITcpUserRepository repository, IEnumerable<IFormatter> formatters)
        {
            Log = log;
            Repository = repository;
            Formatters = formatters;

            Log.Debug("TcpServer initialized.");
        }

        private IEnumerable<IFormatter> Formatters { get; }

        private ILogWrapper Log { get; }

        private ITcpUserRepository Repository { get; }

        /// <summary>
        /// 
        /// </summary>
        public IPAddress Host { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; private set; }

        /// <inheritdoc />
        ///  <summary>
        ///  </summary>
        public TcpServerStatus Status { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Event on a TcpUser status change
        /// </summary>
        public event EventHandler<NetworkEventArgs> OnTcpUserStatusChanged;

        /// <inheritdoc />
        /// <summary>
        /// Event on a TcpServer status change
        /// </summary>
        public event EventHandler<NetworkEventArgs> OnTcpServerStatusChanged;

        /// <inheritdoc />
        /// <summary>
        /// Event on a TcpServer status change
        /// </summary>
        public event EventHandler<NetworkEventArgs> OnNetworkMessageReceived;

        /// <summary>
        /// Gets a tcpUser by Ip address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public ITcpUser GetTcpUser(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress)) throw new ArgumentNullException();
            return Repository.Count == 0 ? null : Repository.Get(ipAddress);
        }

        /// <summary>
        /// Attempts to start the server on the given port and host address
        /// </summary>
        /// <param name="port"></param>
        /// <param name="host"></param>
        public void Startup(int port, IPAddress host)
        {
            if (port <= 1) throw new ArgumentOutOfRangeException();
            if (host == null) throw new ArgumentNullException();

            try
            {
                //// configure the listener thread on the pre-defined port
                Log.InfoFormat("TcpServer starting up on port {0}", port);
                Status = TcpServerStatus.Starting;
                OnTcpServerStatusChanged?.Invoke(this, new NetworkEventArgs { ServerStatus = Status });

                Host = host;
                Port = port;

                _tcpListener = new TcpListener(host, port);
                _listenerThread = new Thread(ListenForClients);
                _listenerThread.Start();
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

        /// <summary>
        /// Shuts the server down
        /// </summary>
        /// <param name="message"></param>
        public void Shutdown(string message)
        {
            Status = TcpServerStatus.ShuttingDown;
            OnTcpServerStatusChanged?.Invoke(this, new NetworkEventArgs { ServerStatus = Status });

            Parallel.ForEach(Repository.Values, user =>
            {
                var tcp = user as ITcpClientWrapper;
                tcp?.WriteToBuffer(message);
                user.Disconnect();
            });

            _listenerThread?.Abort();
            if (_tcpListener != null)
            {
                _tcpListener.Stop();
                _tcpListener = null;
            }

            Log.Info("TcpServer Shutdown.");
            Status = TcpServerStatus.Shutdown;

            OnTcpServerStatusChanged?.Invoke(this, new NetworkEventArgs { ServerStatus = Status });
        }

        /// <summary>
        /// Disconnects a given user by unique id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DisconnectUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            var user = GetTcpUser(id);
            if (user == null) return false;
            user.Disconnect();
            return true;
        }

        private void ListenForClients()
        {
            try
            {
                _tcpListener.Start();
                Log.InfoFormat("TcpServer Listening on {0}", _tcpListener.LocalEndpoint);
                Status = TcpServerStatus.Listening;

                OnTcpServerStatusChanged?.Invoke(this, new NetworkEventArgs { ServerStatus = Status });

                while (true)
                {
                    //// blocks until a client has connected to the server
                    if (_listenerThread.ThreadState != ThreadState.Running)
                    {
                        Log.InfoFormat("Listener Thread state {0}", _listenerThread.ThreadState);
                        break;
                    }

                    var user = new TcpUser(Log, _tcpListener.AcceptTcpClient(), Formatters);
                    user.OnConnect();
                    Repository.Add(user.Id, user);

                    OnTcpUserStatusChanged?.Invoke(user, new NetworkEventArgs { SocketStatus = TcpSocketStatus.Connected });

                    //// create a thread to handle communication with connected client
                    var clientThread = new Thread(HandleClientCommunication);
                    clientThread.Start(user);
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
                if (!(client is TcpUser user))
                    throw new InstanceNotFoundException("TcpUser was not found");

                var clientStream = user.ClientStream;

                var message = new byte[32768];

                while (true)
                {
                    int bytesRead;

                    try
                    {
                        //// blocks until a client sends a message
                        bytesRead = clientStream.Read(message, 0, 32768);
                    }
                    catch (IOException ex)
                    {
                        ex.Handle(ExceptionHandlingOptions.RecordOnly, Log);
                        break;
                    }
                    catch (ObjectDisposedException ex)
                    {
                        ex.Handle(ExceptionHandlingOptions.RecordOnly, Log);
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        //// the client has disconnected from the server
                        Log.InfoFormat("Client[{0}, {1}] disconnected.", user.Id, user.IpAddress);
                        break;
                    }

                    //// message has successfully been received
                    var encoder = new ASCIIEncoding();
                    OnNetworkMessageReceived?.Invoke(user, new NetworkEventArgs { Message = encoder.GetString(message, 0, bytesRead) });
                }

                OnTcpUserStatusChanged?.Invoke(user, new NetworkEventArgs { SocketStatus = TcpSocketStatus.Disconnected });

                //// lost the user
                Repository.Delete(user.Id);
                Log.InfoFormat("Connection Lost from {0}", user.IpAddress);
            }
            catch (InstanceNotFoundException ex)
            {
                ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
            }
        }
    }
}