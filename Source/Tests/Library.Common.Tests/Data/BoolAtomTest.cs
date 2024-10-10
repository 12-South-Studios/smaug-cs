using System;
using FakeItEasy;
using FluentAssertions;
using Library.Common.Data;
using Library.Common.Logging;
using Xunit;

namespace Library.Common.Tests.Data;

public class BoolAtomTest
{
  [Fact]
  public void ConstructorTest()
  {
    const bool value = true;

    BoolAtom atom = new(value);

    atom.Should().NotBeNull();
    atom.Type.Should().Be(AtomType.Boolean);
    atom.Value.Should().Be(value);
  }

  [Fact]
  public void BoolAtomDumpNullParameterTest()
  {
    const bool value = true;
    BoolAtom atom = new(value);

    const string prefix = "Test";

    Action act = () => atom.Dump(null, prefix);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void BoolAtomDumpTest()
  {
    bool callback = false;

    ILogWrapper logger = A.Fake<ILogWrapper>();
    A.CallTo(() => logger.Info(A<string>.Ignored, A<object>.Ignored, A<object>.Ignored))
      .Invokes(() => callback = true);

    const bool value = true;
    BoolAtom atom = new(value);

    const string prefix = "Test";
    atom.Dump(logger, prefix);

    callback.Should().BeTrue();
  }
}