using System;
using System.Data;
using Realm.Library.Common;

namespace SmaugCS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataRowExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <param name="columnName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetDataValue<T>(this DataRow dataRow, string columnName, T defaultValue)
        {
            if (!dataRow.Table.Columns.Contains(columnName))
                return defaultValue;
            return dataRow[columnName] == DBNull.Value 
                ? defaultValue 
                : dataRow[columnName].CastAs<T>();
        }
    }
}
