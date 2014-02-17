using System.Data;

namespace SmallDBConnectivity.Extensions
{
    public static class ConnectionExtension
    {
        public static void CleanupConnection(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }
    }
}
