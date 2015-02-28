using System;
using System.Data;
using Realm.Library.Common;

namespace SmaugCS.Common
{
    public static class DataRowExtensions
    {
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
