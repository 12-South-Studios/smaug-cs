using FluentAssertions;
using Xunit;

namespace SmaugCS.Language.Tests;

public class LanguageConversionDataTests
{
  [Fact]
  public void ConstructorSplitTest()
  {
    LanguageConversionData lcv = new("OldWord NewWord");

    lcv.NewValue.Should().Be("NewWord");
    lcv.OldValue.Should().Be("OldWord");
  }
}