namespace Realm.Library.Network
{
    /// <summary>
    /// Enumeration of Server Status states
    /// </summary>
    public enum TcpServerStatus
    {
        /// <summary>
        /// Tcp Server is Starting Up
        /// </summary>
        Starting,

        /// <summary>
        /// Tcp Server is Listening for incoming Sockets
        /// </summary>
        Listening,

        /// <summary>
        /// Tcp Server is Shutting Down
        /// </summary>
        ShuttingDown,

        /// <summary>
        /// Tcp Server is shutdown and not accepting new sockets
        /// </summary>
        Shutdown
    }
}