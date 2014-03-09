using System.Data;

namespace SmallDBConnectivity
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConnectionExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public static void CleanupConnection(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
            connection.Dispose();
        }
    }
}
