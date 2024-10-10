using System;
using FakeItEasy;
using FluentAssertions;
using Library.Common.Data;
using Library.Common.Logging;
using Xunit;

namespace Library.Common.Tests.Data;

public class RealAtomTest
{
  [Theory]
  [InlineData(5.5D)]
  [InlineData(5.5f)]
  public void RealAtomConstructorTest(double value)
  {
    RealAtom atom = new(value);

    atom.Should().NotBeNull();
    atom.Type.Should().Be(AtomType.Real);
    atom.Value.Should().Be(value);
  }

  [Fact]
  public void RealAtomDumpNullParameterTest()
  {
    const double value = 5.5D;
    RealAtom atom = new(value);

    const string prefix = "Test";

    Action act = () => atom.Dump(null, prefix);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void RealAtomDumpTest()
  {
    bool callback = false;

    ILogWrapper logger = A.Fake<ILogWrapper>();
    A.CallTo(() => logger.Info(A<string>.Ignored, A<object>.Ignored, A<object>.Ignored))
      .Invokes(() => callback = true);

    const double value = 5.5D;
    RealAtom atom = new(value);

    const string prefix = "Test";
    atom.Dump(logger, prefix);

    callback.Should().BeTrue();
  }
}