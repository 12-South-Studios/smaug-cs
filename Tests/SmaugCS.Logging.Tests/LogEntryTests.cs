using System.Collections.Generic;
using NUnit.Framework;

namespace SmaugCS.Logging.Tests
{
    [TestFixture]
    public class LogEntryTests
    {
        [Test]
        public void BuildLogEntryDataTable()
        {
            var result = LogEntry.BuildLogEntryDataTable();

            Assert.That(result.Columns.Count, Is.EqualTo(2));
            Assert.That(result.Columns[0].ColumnName, Is.EqualTo("LogTypeId"));
            Assert.That(result.Columns[1].ColumnName, Is.EqualTo("Text"));
        }

        [Test]
        public void GetLogEntryDataTable()
        {
            var entryList = new List<LogEntry>
            {
                new LogEntry {LogType = LogTypes.Bug, Text = "This is a test bug"},
                new LogEntry {LogType = LogTypes.Error, Text = "This is a test error"}
            };

            var result = LogEntry.GetLogEntryDataTable(entryList);

            Assert.That(result.Rows.Count, Is.EqualTo(2));
            Assert.That(result.Rows[0]["Text"].ToString(), Is.EqualTo("This is a test bug"));
        }
    }
}
