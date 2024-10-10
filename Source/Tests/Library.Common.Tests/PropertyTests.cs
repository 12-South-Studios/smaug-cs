using FluentAssertions;
using Library.Common.Objects;
using Xunit;

namespace Library.Common.Tests;

public class PropertyTests
{
  [Theory]
  [InlineData(true, true)]
  [InlineData(false, false)]
  public void Persistable_Set_Test(bool setTo, bool expectedValue)
  {
    Property prop = new("Test")
    {
      Persistable = setTo
    };

    prop.Persistable.Should().Be(expectedValue);
  }

  [Theory]
  [InlineData(true, true)]
  [InlineData(false, false)]
  public void Visible_Set_Test(bool setTo, bool expectedValue)
  {
    Property prop = new("Test")
    {
      Visible = setTo
    };

    prop.Visible.Should().Be(expectedValue);
  }

  [Theory]
  [InlineData(true, true)]
  [InlineData(false, false)]
  public void Volatile_Set_Test(bool setTo, bool expectedValue)
  {
    Property prop = new("Test")
    {
      Volatile = setTo
    };

    prop.Volatile.Should().Be(expectedValue);
  }
}