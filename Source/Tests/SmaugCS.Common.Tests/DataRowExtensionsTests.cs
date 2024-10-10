using FluentAssertions;
using System.Data;
using Xunit;

namespace SmaugCS.Common.Tests;

public class DataRowExtensionsTests
{
  [Fact]
  public void GetDataValue_MissingColumn_Test()
  {
    DataTable table = new("TestTable");
    DataRow row = table.NewRow();

    string result = row.GetDataValue("Column1", "DefaultValue");

    result.Should().Be("DefaultValue");
  }

  [Fact]
  public void GetDataValue_NullValue_Test()
  {
    DataTable table = new("TestTable");
    table.Columns.Add("Column1");
    DataRow row = table.NewRow();

    string result = row.GetDataValue("Column1", "DefaultValue");

    result.Should().Be("DefaultValue");
  }

  [Fact]
  public void GetDataValue_GoodStringValue_Test()
  {
    DataTable table = new("TestTable");
    table.Columns.Add("Column1");
    DataRow row = table.NewRow();
    row["Column1"] = "Testing 1 2 3";

    string result = row.GetDataValue("Column1", "DefaultValue");

    result.Should().Be("Testing 1 2 3");
  }

  [Fact]
  public void GetDataValue_GoodIntegerValue_Test()
  {
    DataTable table = new("TestTable");
    table.Columns.Add("Column1");
    DataRow row = table.NewRow();
    row["Column1"] = 256;

    int result = row.GetDataValue("Column1", -1);

    result.Should().Be(256);
  }
}