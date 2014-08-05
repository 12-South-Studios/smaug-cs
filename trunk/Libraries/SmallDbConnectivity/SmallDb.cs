using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using log4net;

namespace SmallDBConnectivity
{
    public sealed class SmallDb : ISmallDb
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (SmallDb));

        private static readonly List<string> SqlCommands = new List<string>
        {
            "SELECT",
            "INSERT",
            "UPDATE",
            "DELETE",
            "DROP",
            "MERGE",
            "ALTER",
            "CREATE"
        };

        private static readonly List<string> DisallowedCommands = new List<string>
        {
            "DROP",
            "ALTER",
            "CREATE"
        };

        public object ExecuteScalar(IDbConnection dbConnection,
            string storedProcedureName,
            IEnumerable<IDataParameter> parameters = null,
            string errorMessage = "",
            bool throwException = true)
        {
            ValidateArguments(dbConnection, storedProcedureName);

            object result = null;
            try
            {
                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    SetupDbCommand(dbConnection, dbCommand, storedProcedureName, parameters);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                    result = dbCommand.ExecuteScalar();
                }
            }
            catch (DbException ex)
            {
                _log.Error(string.IsNullOrEmpty(errorMessage)
                    ? string.Format("{0} threw an Exception: {1}", storedProcedureName, ex.Message)
                    : errorMessage, ex);

                if (throwException)
                    throw;
            }
            finally
            {
                dbConnection.Close();
            }

            return result;
        }

        public void ExecuteNonQuery(IDbConnection dbConnection,
            string storedProcedureName,
            IEnumerable<IDataParameter> parameters = null,
            string errorMessage = "", bool throwException = true)
        {
            ValidateArguments(dbConnection, storedProcedureName);

            try
            {
                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    SetupDbCommand(dbConnection, dbCommand, storedProcedureName, parameters);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                    dbCommand.ExecuteNonQuery();
                }
            }
            catch (DbException ex)
            {
                _log.Error(string.IsNullOrEmpty(errorMessage)
                    ? string.Format("{0} threw an Exception: {1}", storedProcedureName, ex.Message)
                    : errorMessage, ex);

                if (throwException)
                    throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public DataTable ExecuteQuery(IDbConnection dbConnection,
            string storedProcedureName,
            IEnumerable<IDataParameter> parameters = null,
            string errorMessage = "", bool throwException = true)
        {
            ValidateArguments(dbConnection, storedProcedureName);

            DataTable table = new DataTable();

            try
            {
                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    SetupDbCommand(dbConnection, dbCommand, storedProcedureName, parameters);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                    using (IDataReader dbReader = dbCommand.ExecuteReader())
                    {
                        if (dbReader != null)
                            table.Load(dbReader);
                    }
                }
            }
            catch (DbException ex)
            {
                _log.Error(string.IsNullOrEmpty(errorMessage)
                    ? string.Format("{0} threw an Exception: {1}", storedProcedureName, ex.Message)
                    : errorMessage, ex);

                if (throwException)
                    throw;
            }
            finally
            {
                dbConnection.Close();
            }

            return table;
        }

        public T ExecuteQuery<T>(IDbConnection dbConnection,
            string storedProcedureName,
            Func<IDataReader, T> translateFunction,
            IEnumerable<IDataParameter> parameters = null,
            string errorMessage = "", bool throwException = true) where T : class, new()
        {
            ValidateArguments(dbConnection, storedProcedureName);
            if (translateFunction == null) throw new ArgumentNullException("translateFunction");

            try
            {
                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    SetupDbCommand(dbConnection, dbCommand, storedProcedureName, parameters);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                    using (IDataReader dbReader = dbCommand.ExecuteReader())
                    {
                        return translateFunction.Invoke(dbReader);
                    }
                }
            }
            catch (DbException ex)
            {
                _log.Error(string.IsNullOrEmpty(errorMessage)
                    ? string.Format("{0} threw an Exception: {1}", storedProcedureName, ex.Message)
                    : errorMessage, ex);

                if (throwException)
                    throw;
            }
            finally
            {
                dbConnection.Close();
            }

            return null;
        }

        internal static void SetupDbCommand(IDbConnection dbConnection,
            IDbCommand dbCommand,
            string storedProcedureName,
            IEnumerable<IDataParameter> parameters)
        {
            dbCommand.Connection = dbConnection;
            dbCommand.CommandText = storedProcedureName;
            dbCommand.CommandType = IsInternalSql(storedProcedureName) ? CommandType.Text : CommandType.StoredProcedure;
            if (parameters != null && parameters.Any())
                parameters.ToList().ForEach(parameter => dbCommand.Parameters.Add(parameter));
        }

        internal static void ValidateArguments(IDbConnection dbConnection, string storedProcedureName)
        {
            if (dbConnection == null) throw new ArgumentNullException("dbConnection");
            if (string.IsNullOrWhiteSpace(storedProcedureName)) throw new ArgumentNullException("storedProcedureName");
        }

        internal static bool IsInternalSql(string sqlString)
        {
            string[] words = sqlString.ToUpper().Split(' ');
            return (SqlCommands.Contains(words[0]) && !DisallowedCommands.Contains(words[0]));
        }
    }
}
