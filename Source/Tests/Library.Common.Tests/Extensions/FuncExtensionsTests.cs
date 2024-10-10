using System;
using System.Linq;
using FluentAssertions;
using Library.Common.Extensions;
using Xunit;

namespace Library.Common.Tests.Extensions;

public class FuncExtensionsTests
{
  [Fact]
  public void TryCatch_NoReturnValue_NoException_Test()
  {
    bool callback = false;

    Action<object[]> func = _ => callback = true;

    func.TryCatch();

    callback.Should().BeTrue();
  }

  [Fact]
  public void TryCatch_NoReturnValue_WithException_Test()
  {
    bool callback = false;

    Action<object[]> func = _ => throw new Exception("Test Exception");

    func.TryCatch(ShowError);

    callback.Should().BeTrue();
    return;

    void ShowError(Exception _) => callback = true;
  }

  [Fact]
  public void TryCatch_NoException_NoFinally_Test()
  {
    bool callback = false;

    Func<object[], bool> func = _ =>
    {
      callback = true;
      return true;
    };

    bool result = func.TryCatch(ShowError);

    result.Should().BeTrue();
    callback.Should().BeTrue();
    return;

    bool ShowError(Exception _) => false;
  }

  [Fact]
  public void TryCatch_WithException_Test()
  {
    bool callback = false;

    Func<object[], bool> func = _ => throw new Exception("Test Exception");

    bool result = func.TryCatch(ShowError);

    result.Should().BeFalse();
    callback.Should().BeTrue();
    return;

    bool ShowError(Exception _)
    {
      callback = true;
      return false;
    }
  }

  [Fact]
  public void TryCatch_DifferentType_Test()
  {
    bool callback = false;
    bool finalCallback = false;

    Func<int[], int> func = ints =>
    {
      callback = true;
      return ints.Sum();
    };

    int result = func.TryCatch((Func<Exception, int>)ShowError, Final, 2, 5, 8);

    result.Should().Be(15);
    callback.Should().BeTrue();
    finalCallback.Should().BeTrue();
    return;

    void Final(int[] _)
    {
      finalCallback = true;
    }

    int ShowError(Exception _) => 0;
  }
}