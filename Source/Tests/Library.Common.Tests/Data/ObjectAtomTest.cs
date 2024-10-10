using System;
using FakeItEasy;
using FluentAssertions;
using Library.Common.Data;
using Library.Common.Logging;
using Xunit;

namespace Library.Common.Tests.Data;

public class ObjectAtomTest
{
  [Fact]
  public void ObjectAtomDumpNullParameterTest()
  {
    const int value = 5;
    ObjectAtom atom = new(value);

    const string prefix = "Test";

    Action act = () => atom.Dump(null, prefix);
    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void ObjectAtomDumpTest()
  {
    bool callback = false;

    ILogWrapper logger = A.Fake<ILogWrapper>();
    A.CallTo(() => logger.Info(A<string>.Ignored, A<object>.Ignored, A<object>.Ignored))
      .Invokes(() => callback = true);

    const int value = 5;
    ObjectAtom atom = new(value);

    const string prefix = "Test";

    atom.Dump(logger, prefix);

    callback.Should().BeTrue();
  }
}