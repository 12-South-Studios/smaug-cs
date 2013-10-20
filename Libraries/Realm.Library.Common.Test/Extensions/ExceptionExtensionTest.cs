using System;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Logging;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class ExceptionExtensionTests
    {
        private class TestException : Exception
        {
            public TestException(string msg, Exception innerException) : base(msg, innerException) { }
        }

        [TestCase(ExceptionHandlingOptions.RecordAndThrow, "Test", false, ExpectedException = typeof(TestException))]
        [TestCase(ExceptionHandlingOptions.RecordOnly, "Test", false)]
        [TestCase(ExceptionHandlingOptions.RecordOnly, "", false)]
        [TestCase(ExceptionHandlingOptions.ThrowOnly, "Test", true, ExpectedException = typeof(TestException))]
        [TestCase(ExceptionHandlingOptions.Suppress, "Test", true)]
        public void HandleGenericTest(ExceptionHandlingOptions options, string msg, bool throwLoggingException)
        {
            var mockLog = new Mock<ILogWrapper>();
            if (throwLoggingException)
                mockLog.Setup(x => x.Error(It.IsAny<object>(), It.IsAny<Exception>()))
                    .Throws(new InvalidOperationException("Unit test should not get an exception of this type"));
            else
                mockLog.Setup(x => x.Error(It.IsAny<object>(), It.IsAny<Exception>()));

            try
            {
                throw new Exception(msg);
            }
            catch (Exception ex)
            {
                ex.Handle<TestException>(options, mockLog.Object, msg);
            }
        }

        [TestCase(ExceptionHandlingOptions.RecordAndThrow, "Test", false, ExpectedException = typeof(Exception))]
        [TestCase(ExceptionHandlingOptions.RecordOnly, "Test", false)]
        [TestCase(ExceptionHandlingOptions.RecordOnly, "", false)]
        [TestCase(ExceptionHandlingOptions.ThrowOnly, "Test", true, ExpectedException = typeof(Exception))]
        [TestCase(ExceptionHandlingOptions.Suppress, "Test", true)]
        public void HandleTest(ExceptionHandlingOptions options, string msg, bool throwLoggingException)
        {
            var mockLog = new Mock<ILogWrapper>();
            if (throwLoggingException)
                mockLog.Setup(x => x.Error(It.IsAny<object>(), It.IsAny<Exception>()))
                    .Throws(new InvalidOperationException("Unit test should not get an exception of this type"));
            else
                mockLog.Setup(x => x.Error(It.IsAny<object>(), It.IsAny<Exception>()));

            try
            {
                throw new Exception(msg);
            }
            catch (Exception ex)
            {
                ex.Handle(options, mockLog.Object, msg);
            }
        }
    }
}
