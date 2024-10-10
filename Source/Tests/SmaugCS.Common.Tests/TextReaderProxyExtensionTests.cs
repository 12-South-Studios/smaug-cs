using FluentAssertions;
using Library.Common;
using System.IO;
using Xunit;

namespace SmaugCS.Common.Tests;

public class TextReaderProxyExtensionTests
{
  [Theory]
  [InlineData("2&4&8~", 2, true)]
  [InlineData("2&4&8~", 4, true)]
  [InlineData("2&4&8~", 8, true)]
  [InlineData("2&4&8~", 16, false)]
  public void ReadBitvectorReturnsValidObject(string stringToRead, int valueToCheck, bool expectedValue)
  {
    TextReaderProxy proxy = new(new StringReader(stringToRead));

    ExtendedBitvector result = proxy.ReadBitvector();

    result.Should().NotBeNull();
    result.IsSet(valueToCheck).Should().Be(expectedValue);
  }

  [Theory]
  [InlineData("2 & 4 & 8~", 2, true)]
  [InlineData("2 & 4 & 8~", 4, true)]
  [InlineData("2 & 4 & 8~", 8, true)]
  [InlineData("2 & 4 & 8~", 16, false)]
  public void ReadBitvectorWithSpacesReturnsValidObject(string stringToRead, int valueToCheck, bool expectedValue)
  {
    TextReaderProxy proxy = new(new StringReader("2 & 4 & 8~"));

    ExtendedBitvector result = proxy.ReadBitvector();

    result.Should().NotBeNull();
    result.IsSet(valueToCheck).Should().Be(expectedValue);
  }
}