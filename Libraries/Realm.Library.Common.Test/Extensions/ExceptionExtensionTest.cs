using System;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Test.Fakes;

namespace Realm.Library.Common.Test.Extensions
{
    [TestFixture]
    public class ExceptionExtensionTests
    {
        [TestCase(ExceptionHandlingOptions.RecordAndThrow, "Test", false, ExpectedException = typeof(FakeException))]
        [TestCase(ExceptionHandlingOptions.RecordOnly, "Test", false)]
        [TestCase(ExceptionHandlingOptions.RecordOnly, "", false)]
        [TestCase(ExceptionHandlingOptions.ThrowOnly, "Test", true, ExpectedException = typeof(FakeException))]
        [TestCase(ExceptionHandlingOptions.Suppress, "Test", true)]
        [Category("Extension Tests")]
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
                ex.Handle<FakeException>(options, mockLog.Object, msg);
            }
        }

        [TestCase(ExceptionHandlingOptions.RecordAndThrow, "Test", false, ExpectedException = typeof(Exception))]
        [TestCase(ExceptionHandlingOptions.RecordOnly, "Test", false)]
        [TestCase(ExceptionHandlingOptions.RecordOnly, "", false)]
        [TestCase(ExceptionHandlingOptions.ThrowOnly, "Test", true, ExpectedException = typeof(Exception))]
        [TestCase(ExceptionHandlingOptions.Suppress, "Test", true)]
        [Category("Extension Tests")]
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
