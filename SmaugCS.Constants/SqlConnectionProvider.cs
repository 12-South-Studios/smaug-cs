using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SmaugCS.Constants
{
    public static class SqlConnectionProvider
    {
        public static IDbConnection Connection
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SmaugDB"]
                    .ConnectionString.Replace("|DataPath|", GameConstants.DataPath);

                IDbConnection connection = new SqlConnection(connectionString);
                return connection;
            }
        }
    }
}
