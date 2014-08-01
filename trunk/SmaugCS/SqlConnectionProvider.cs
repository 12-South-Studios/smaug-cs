using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SmaugCS.Constants;

namespace SmaugCS
{
    public static class SqlConnectionProvider
    {
        public static IDbConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SmaugDB"]
                .ConnectionString.Replace("|DataPath|", GameConstants.GetDataPath());

            IDbConnection connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
