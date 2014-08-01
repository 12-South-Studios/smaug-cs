using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using Realm.Library.Common.Logging;
using Realm.Library.Network.Mxp;
using Realm.Library.Network.Properties;

namespace Realm.Library.Network
{
    /// <summary>
    /// Tcp User class handles tcp communication
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TcpUser : TcpClientWrapper, ITcpUser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log"></param>
        /// <param name="tcpClient"></param>
        /// <param name="formatter"></param>
        [ExcludeFromCodeCoverage]
        public TcpUser(ILogWrapper log, TcpClient tcpClient, IFormatter formatter)
            : base(log, tcpClient, formatter)
        {
            var guid = Guid.NewGuid();
            Id = guid.ToString();
        }

        /// <summary>
        /// Gets the unique identifier of this user
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Handles the connection
        /// </summary>
        public void OnConnect()
        {
            if (Formatter is MxpFormatter)
                this.EnableMxp(ClientStream);
            Log.InfoFormat(Resources.MSG_TCPUSER_CONNECT, Id, IpAddress);
        }

        /// <summary>
        /// Disconnects the user from the connected socket
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Disconnect()
        {
            TcpClient.Client.Close();
            IpAddress = string.Empty;
            ClientStream = null;
            ConnectedOn = DateTime.MinValue;
        }
    }
}