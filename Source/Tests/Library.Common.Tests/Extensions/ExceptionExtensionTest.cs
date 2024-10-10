using System;
using FakeItEasy;
using FluentAssertions;
using Library.Common.Extensions;
using Library.Common.Logging;
using Xunit;

namespace Library.Common.Tests.Extensions;

public class ExceptionExtensionTests
{
  private class TestException(string msg, Exception innerException) : Exception(msg, innerException);

  [Theory]
  [InlineData(ExceptionHandlingOptions.RecordOnly, "Test", false)]
  [InlineData(ExceptionHandlingOptions.RecordOnly, "", false)]
  [InlineData(ExceptionHandlingOptions.Suppress, "Test", true)]
  public void HandleGenericTest(ExceptionHandlingOptions options, string msg, bool throwLoggingException)
  {
    ILogWrapper logger = A.Fake<ILogWrapper>();

    if (throwLoggingException)
      A.CallTo(() => logger.Error(A<Exception>.Ignored))
        .Throws(new InvalidOperationException("Unit test should not get an exception of this type"));
    else
      A.CallTo(() => logger.Error(A<Exception>.Ignored));

    try
    {
      throw new Exception(msg);
    }
    catch (Exception ex)
    {
      ex.Handle<TestException>(options, logger, msg);
    }
  }

  [Fact]
  public void Handle_ReThrowsException_WhenRecordAndThrowIsOption()
  {
    ILogWrapper logger = A.Fake<ILogWrapper>();
    A.CallTo(() => logger.Error(A<Exception>.Ignored));

    try
    {
      throw new Exception("Test");
    }
    catch (Exception ex)
    {
      Action act = () => ex.Handle<TestException>(ExceptionHandlingOptions.RecordAndThrow, logger, "Test");
      act.Should().Throw<TestException>();
    }
  }

  [Fact]
  public void Handle_ReThrowsException_WhenThrowOnlyIsOption()
  {
    ILogWrapper logger = A.Fake<ILogWrapper>();
    A.CallTo(() => logger.Error(A<Exception>.Ignored))
      .Throws(new InvalidOperationException("Unit test should not get an exception of this type"));

    try
    {
      throw new Exception("Test");
    }
    catch (Exception ex)
    {
      Action act = () => ex.Handle<TestException>(ExceptionHandlingOptions.ThrowOnly, logger, "Test");
      act.Should().Throw<TestException>();
    }
  }

  [Theory]
  [InlineData(ExceptionHandlingOptions.RecordOnly, "Test", false)]
  [InlineData(ExceptionHandlingOptions.RecordOnly, "", false)]
  [InlineData(ExceptionHandlingOptions.Suppress, "Test", true)]
  public void HandleTest(ExceptionHandlingOptions options, string msg, bool throwLoggingException)
  {
    ILogWrapper logger = A.Fake<ILogWrapper>();

    if (throwLoggingException)
      A.CallTo(() => logger.Error(A<Exception>.Ignored))
        .Throws(new InvalidOperationException("Unit test should not get an exception of this type"));
    else
      A.CallTo(() => logger.Error(A<Exception>.Ignored));

    try
    {
      throw new Exception(msg);
    }
    catch (Exception ex)
    {
      ex.Handle(options, logger, msg);
    }
  }
}