using FluentAssertions;
using System.Data;
using Xunit;

namespace SmaugCS.Common.Tests
{

    public class DataRowExtensionsTests
    {
        [Fact]
        public void GetDataValue_MissingColumn_Test()
        {
            var table = new DataTable("TestTable");
            var row = table.NewRow();

            var result = row.GetDataValue("Column1", "DefaultValue");

            result.Should().Be("DefaultValue");
        }

        [Fact]
        public void GetDataValue_NullValue_Test()
        {
            var table = new DataTable("TestTable");
            table.Columns.Add("Column1");
            var row = table.NewRow();

            var result = row.GetDataValue("Column1", "DefaultValue");

            result.Should().Be("DefaultValue");
        }

        [Fact]
        public void GetDataValue_GoodStringValue_Test()
        {
            var table = new DataTable("TestTable");
            table.Columns.Add("Column1");
            var row = table.NewRow();
            row["Column1"] = "Testing 1 2 3";

            var result = row.GetDataValue("Column1", "DefaultValue");

            result.Should().Be("Testing 1 2 3");
        }

        [Fact]
        public void GetDataValue_GoodIntegerValue_Test()
        {
            var table = new DataTable("TestTable");
            table.Columns.Add("Column1");
            var row = table.NewRow();
            row["Column1"] = 256;

            var result = row.GetDataValue("Column1", -1);

            result.Should().Be(256);
        }
    }
}
