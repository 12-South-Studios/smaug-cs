using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using Realm.Library.Common.Logging;
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
        /// <param name="formatters"></param>
        [ExcludeFromCodeCoverage]
        public TcpUser(ILogWrapper log, TcpClient tcpClient, IEnumerable<IFormatter> formatters)
            : base(log, tcpClient, formatters)
        {
            var guid = Guid.NewGuid();
            Id = guid.ToString();
        }

        /// <summary>
        /// Gets the unique identifier of this user
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Handles the connection
        /// </summary>
        public void OnConnect()
        {
            foreach(var formatter in Formatters)
                formatter.Enable(this, ClientStream);
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