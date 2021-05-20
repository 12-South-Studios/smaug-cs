namespace Realm.Library.Network
{
    /// <summary>
    /// Defines the contract for Tcp Users
    /// </summary>
    public interface ITcpUser : ITcpClientWrapper
    {
        /// <summary>
        /// Handles the connection
        /// </summary>
        void OnConnect();

        /// <summary>
        /// Disconnects the user from the connected socket
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Gets the unique identifier of this user
        /// </summary>
        string Id { get; }
    }
}