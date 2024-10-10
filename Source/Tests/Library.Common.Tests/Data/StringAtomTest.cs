using System;
using FakeItEasy;
using FluentAssertions;
using Library.Common.Data;
using Library.Common.Logging;
using Xunit;

namespace Library.Common.Tests.Data;

public class StringAtomTest
{
  [Fact]
  public void StringAtomConstructorTest()
  {
    const string value = "Test";

    StringAtom atom = new(value);

    atom.Should().NotBeNull();
    atom.Type.Should().Be(AtomType.String);
    atom.Value.Should().Be(value);
  }

  [Fact]
  public void StringAtomDumpNullParameterTest()
  {
    const string value = "test";
    StringAtom atom = new(value);

    const string prefix = "Test";

    Action act = () => atom.Dump(null, prefix);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void StringAtomDumpTest()
  {
    bool callback = false;

    ILogWrapper logger = A.Fake<ILogWrapper>();
    A.CallTo(() => logger.Info(A<string>.Ignored, A<object>.Ignored, A<object>.Ignored))
      .Invokes(() => callback = true);

    const string value = "Test";
    StringAtom atom = new(value);

    const string prefix = "Test";
    atom.Dump(logger, prefix);

    callback.Should().BeTrue();
  }

  [Theory]
  [InlineData("Test", "Tester", false)]
  [InlineData("Test", "Test", true)]
  public void StringAtomEqualsTest(string firstValue, string secondValue, bool expected)
  {
    StringAtom atom = new(firstValue);
    StringAtom compareAtom = new(secondValue);

    atom.Equals(compareAtom).Should().Be(expected);
  }

  [Fact]
  public void StringAtomEqualsNullParameterTest()
  {
    const string value = "test";
    StringAtom atom = new(value);

    atom.Equals(null).Should().BeFalse();
  }
}