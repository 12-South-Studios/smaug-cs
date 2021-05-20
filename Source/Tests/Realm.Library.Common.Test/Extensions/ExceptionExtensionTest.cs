using System;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Exceptions;
using Xunit;
using FakeItEasy;
using FluentAssertions;

namespace Realm.Library.Common.Test.Extensions
{
    public class ExceptionExtensionTests
    {
        private class TestException : Exception
        {
            public TestException(string msg, Exception innerException) : base(msg, innerException) { }
        }

        [Theory]
        [InlineData(ExceptionHandlingOptions.RecordOnly, "Test", false)]
        [InlineData(ExceptionHandlingOptions.RecordOnly, "", false)]
        [InlineData(ExceptionHandlingOptions.Suppress, "Test", true)]
        public void HandleGenericTest(ExceptionHandlingOptions options, string msg, bool throwLoggingException)
        {
            var logger = A.Fake<ILogWrapper>();

            if (throwLoggingException)
                A.CallTo(() => logger.Error(A<object>.Ignored, A<Exception>.Ignored))
                    .Throws(new InvalidOperationException("Unit test should not get an exception of this type"));
            else
                A.CallTo(() => logger.Error(A<object>.Ignored, A<Exception>.Ignored));

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
            var logger = A.Fake<ILogWrapper>();
            A.CallTo(() => logger.Error(A<object>.Ignored, A<Exception>.Ignored));

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
            var logger = A.Fake<ILogWrapper>();
            A.CallTo(() => logger.Error(A<object>.Ignored, A<Exception>.Ignored))
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
            var logger = A.Fake<ILogWrapper>();

            if (throwLoggingException)
                A.CallTo(() => logger.Error(A<object>.Ignored, A<Exception>.Ignored))
                    .Throws(new InvalidOperationException("Unit test should not get an exception of this type"));
            else
                A.CallTo(() => logger.Error(A<object>.Ignored, A<Exception>.Ignored));

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
}
