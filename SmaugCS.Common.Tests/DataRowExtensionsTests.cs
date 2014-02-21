using System;
using System.Data;
using NUnit.Framework;

namespace SmaugCS.Common.Tests
{
    [TestFixture]
    public class DataRowExtensionsTests
    {
        [Test]
        public void GetDataValue_MissingColumn_Test()
        {
            var table = new DataTable("TestTable");
            var row = table.NewRow();

            var result = row.GetDataValue("Column1", "DefaultValue");

            Assert.That(result, Is.EqualTo("DefaultValue"));
        }

        [Test]
        public void GetDataValue_NullValue_Test()
        {
            var table = new DataTable("TestTable");
            table.Columns.Add("Column1");
            var row = table.NewRow();

            var result = row.GetDataValue("Column1", "DefaultValue");

            Assert.That(result, Is.EqualTo("DefaultValue"));
        }

        [Test]
        public void GetDataValue_GoodStringValue_Test()
        {
            var table = new DataTable("TestTable");
            table.Columns.Add("Column1");
            var row = table.NewRow();
            row["Column1"] = "Testing 1 2 3";

            var result = row.GetDataValue("Column1", "DefaultValue");

            Assert.That(result, Is.EqualTo("Testing 1 2 3"));
        }

        [Test]
        public void GetDataValue_GoodIntegerValue_Test()
        {
            var table = new DataTable("TestTable");
            table.Columns.Add("Column1");
            var row = table.NewRow();
            row["Column1"] = 256;

            var result = row.GetDataValue("Column1", -1);

            Assert.That(result, Is.EqualTo(256));
        }
    }
}
