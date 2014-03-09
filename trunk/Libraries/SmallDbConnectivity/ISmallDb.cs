using System;
using System.Collections.Generic;
using System.Data;

namespace SmallDBConnectivity
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISmallDb
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="errorMessage"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        object ExecuteScalar(IDbConnection dbConnection,
                             string storedProcedureName,
                             IEnumerable<IDataParameter> parameters = null,
                             string errorMessage = "",
                             bool throwException = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="errorMessage"></param>
        /// <param name="throwException"></param>
        void ExecuteNonQuery(IDbConnection dbConnection,
                             string storedProcedureName,
                             IEnumerable<IDataParameter> parameters = null,
                             string errorMessage = "", bool throwException = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="errorMessage"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        DataTable ExecuteQuery(IDbConnection dbConnection,
                               string storedProcedureName,
                               IEnumerable<IDataParameter> parameters = null,
                               string errorMessage = "", bool throwException = true);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbConnection"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="translateFunction"></param>
        /// <param name="parameters"></param>
        /// <param name="errorMessage"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        T ExecuteQuery<T>(IDbConnection dbConnection,
                          string storedProcedureName,
                          Func<IDataReader, T> translateFunction,
                          IEnumerable<IDataParameter> parameters = null,
                          string errorMessage = "", bool throwException = true) where T : class, new();
    }
}
