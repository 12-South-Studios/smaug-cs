using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Network.Properties;

namespace Realm.Library.Network
{
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
        public TcpServer(ILogWrapper log, ITcpUserRepository repository)
        {
            Validation.IsNotNull(log, "log");
            Validation.IsNotNull(repository, "repository");

            Log = log;
            Repository = repository;

            Log.Debug("TcpServer initialized.");
        }

        private ILogWrapper Log { get; set; }

        private ITcpUserRepository Repository { get; set; }

        public IPAddress Host { get; private set; }

        public int Port { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public TcpServerStatus Status { get; private set; }

        /// <summary>
        /// Event on a TcpUser status change
        /// </summary>
        public event EventHandler<NetworkEventArgs> OnTcpUserStatusChanged;

        /// <summary>
        /// Event on a TcpServer status change
        /// </summary>
        public event EventHandler<NetworkEventArgs> OnTcpServerStatusChanged;

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
            Validation.IsNotNullOrEmpty(ipAddress, "ipAddress");

            return Repository.Count == 0 ? null : Repository.Get(ipAddress);
        }

        /// <summary>
        /// Attempts to start the server on the given port and host address
        /// </summary>
        /// <param name="port"></param>
        /// <param name="host"></param>
        public void Startup(int port, IPAddress host)
        {
            Validation.Validate<ArgumentOutOfRangeException>(port > 1 && port < Int32.MaxValue);
            Validation.IsNotNull(host, "host");

            try
            {
                //// configure the listener thread on the pre-defined port
                Log.InfoFormat(Resources.MSG_TCPSERVER_START, port);
                Status = TcpServerStatus.Starting;
                if (OnTcpServerStatusChanged.IsNotNull())
                    OnTcpServerStatusChanged(this, new NetworkEventArgs { ServerStatus = Status });

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
            if (OnTcpServerStatusChanged.IsNotNull())
                OnTcpServerStatusChanged(this, new NetworkEventArgs { ServerStatus = Status });

            Parallel.ForEach(Repository.Values, user =>
                {
                    var tcp = user.CastAs<ITcpClientWrapper>();
                    if (tcp.IsNotNull())
                        tcp.WriteToBuffer(message);
                    user.Disconnect();
                });

            if (_listenerThread.IsNotNull())
                _listenerThread.Abort();
            if (_tcpListener.IsNotNull())
            {
                _tcpListener.Stop();
                _tcpListener = null;
            }

            Log.Info(Resources.MSG_TCPSERVER_STOP);
            Status = TcpServerStatus.Shutdown;

            if (OnTcpServerStatusChanged.IsNotNull())
                OnTcpServerStatusChanged(this, new NetworkEventArgs { ServerStatus = Status });
        }

        /// <summary>
        /// Disconnects a given user by unique id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DisconnectUser(string id)
        {
            Validation.IsNotNullOrEmpty(id, "id");

            var user = GetTcpUser(id);
            if (user.IsNull()) return false;

            user.Disconnect();
            return true;
        }

        private void ListenForClients()
        {
            try
            {
                _tcpListener.Start();
                Log.InfoFormat(Resources.MSG_TCPSERVER_LISTEN, _tcpListener.LocalEndpoint);
                Status = TcpServerStatus.Listening;

                if (OnTcpServerStatusChanged.IsNotNull())
                    OnTcpServerStatusChanged(this, new NetworkEventArgs { ServerStatus = Status });

                while (true)
                {
                    //// blocks until a client has connected to the server
                    if (_listenerThread.ThreadState != ThreadState.Running)
                    {
                        Log.InfoFormat(Resources.MSG_TCPSERVER_STATE, _listenerThread.ThreadState);
                        break;
                    }

                    var user = new TcpUser(Log, _tcpListener.AcceptTcpClient(), new Mxp.MxpFormatter());
                    user.OnConnect();
                    Repository.Add(user.Id, user);

                    if (OnTcpUserStatusChanged.IsNotNull())
                        OnTcpUserStatusChanged(user, new NetworkEventArgs { SocketStatus = TcpSocketStatus.Connected });

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
            Validation.IsNotNull(client, "client");

            try
            {
                var user = client as TcpUser;
                if (user.IsNull())
                    throw new InstanceNotFoundException(Resources.ERR_NO_TCPUSER);

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
                        Log.InfoFormat(Resources.MSG_TCPUSER_DISCONNECT, user.Id, user.IpAddress);
                        break;
                    }

                    //// message has successfully been received
                    var encoder = new ASCIIEncoding();
                    if (OnNetworkMessageReceived.IsNotNull())
                        OnNetworkMessageReceived(user, new NetworkEventArgs { Message = encoder.GetString(message, 0, bytesRead) });
                }

                if (OnTcpUserStatusChanged.IsNotNull())
                    OnTcpUserStatusChanged(user, new NetworkEventArgs { SocketStatus = TcpSocketStatus.Disconnected });

                //// lost the user
                Repository.Delete(user.Id);
                Log.InfoFormat(Resources.MSG_CONNECTION_LOST, user.IpAddress);
            }
            catch (InstanceNotFoundException ex)
            {
                ex.Handle(ExceptionHandlingOptions.RecordAndThrow, Log);
            }
        }
    }
}