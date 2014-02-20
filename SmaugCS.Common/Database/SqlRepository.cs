using System.Collections.Generic;

namespace SmaugCS.Common.Database
{
    public sealed class SqlRepository
    {
        private readonly Dictionary<string, string> SqlTable;

        public SqlRepository()
        {
            SqlTable = new Dictionary<string, string>();
        }

        public bool AddSql(string name, string text)
        {
            if (SqlTable.ContainsKey(name.ToLower()))
                return false;

            SqlTable.Add(name.ToLower(), text);
            return true;
        }

        public string GetSql(string name)
        {
            return SqlTable.ContainsKey(name.ToLower()) ? SqlTable[name.ToLower()] : string.Empty;
        }

        public bool RemoveSql(string name)
        {
            return SqlTable.ContainsKey(name.ToLower()) && SqlTable.Remove(name.ToLower());
        }
    }
}
