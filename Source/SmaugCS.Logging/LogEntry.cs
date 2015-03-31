using System.Collections.Generic;
using System.Data;

namespace SmaugCS.Logging
{
    public class LogEntry
    {
        public LogTypes LogType { get; set; }
        public string Text { get; set; }

        public static DataTable GetLogEntryDataTable(IEnumerable<LogEntry> logs)
        {
            var dt = BuildLogEntryDataTable();

            foreach (LogEntry log in logs)
            {
                DataRow dr = dt.NewRow();
                dr["LogTypeId"] = log.LogType;
                dr["Text"] = log.Text;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable BuildLogEntryDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LogTypeId", typeof(int));
            dt.Columns.Add("Text", typeof(string));
            return dt;
        }
    }
}
