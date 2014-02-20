using System;
using System.Collections.Generic;
using System.Data;

namespace SmallDBConnectivity
{
    public interface ISmallDb
    {
        object ExecuteScalar(IDbConnection dbConnection,
                             string storedProcedureName,
                             IEnumerable<IDataParameter> parameters = null,
                             string errorMessage = "",
                             bool throwException = true);

        void ExecuteNonQuery(IDbConnection dbConnection,
                             string storedProcedureName,
                             IEnumerable<IDataParameter> parameters = null,
                             string errorMessage = "", bool throwException = true);

        DataTable ExecuteQuery(IDbConnection dbConnection,
                               string storedProcedureName,
                               IEnumerable<IDataParameter> parameters = null,
                               string errorMessage = "", bool throwException = true);

        T ExecuteQuery<T>(IDbConnection dbConnection,
                          string storedProcedureName,
                          Func<IDataReader, T> translateFunction,
                          IEnumerable<IDataParameter> parameters = null,
                          string errorMessage = "", bool throwException = true) where T : class, new();
    }
}
