using System;
using FakeItEasy;
using FluentAssertions;
using Library.Common.Data;
using Library.Common.Logging;
using Xunit;

namespace Library.Common.Tests.Data;

public class IntAtomTest
{
  [Fact]
  public void IntAtomIntegerTest()
  {
    const int value = 5;

    IntAtom atom = new(value);

    atom.Should().NotBeNull();
    atom.Type.Should().Be(AtomType.Integer);
    atom.Value.Should().Be(value);
  }

  [Fact]
  public void IntAtomLongTest()
  {
    const long value = 50001;

    IntAtom atom = new(value);

    atom.Should().NotBeNull();
    atom.Type.Should().Be(AtomType.Integer);
    atom.Value.Should().Be((int)value);
  }

  [Fact]
  public void IntAtomDumpNullParameterTest()
  {
    const int value = 5;
    IntAtom atom = new(value);

    const string prefix = "Test";

    Action act = () => atom.Dump(null, prefix);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void IntAtomDumpTest()
  {
    bool callback = false;

    ILogWrapper logger = A.Fake<ILogWrapper>();
    A.CallTo(() => logger.Info(A<string>.Ignored, A<object>.Ignored, A<object>.Ignored))
      .Invokes(() => callback = true);

    const int value = 5;
    IntAtom atom = new(value);

    const string prefix = "Test";

    atom.Dump(logger, prefix);

    callback.Should().BeTrue();
  }

  [Theory]
  [InlineData(5, 10, false)]
  [InlineData(5, 5, true)]
  public void IntAtomEqualsTest(int firstValue, int secondValue, bool expected)
  {
    IntAtom atom = new(firstValue);
    IntAtom compareAtom = new(secondValue);

    atom.Equals(compareAtom).Should().Be(expected);
  }

  [Fact]
  public void IntAtomGetHashCodeTest()
  {
    const int value = 5;
    IntAtom atom = new(value);

    atom.GetHashCode().Should().Be(value);
  }
}