using System;
using Moq;
using NUnit.Framework;
using Realm.Library.Common.Logging;
using log4net;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class LogWrapperTests
    {
        private static LogEventArgs _eventArgs;
        private static Mock<ILog> _mockLog;
        private static LogWrapper _wrapper;

        [SetUp]
        public void OnSetup()
        {
            _eventArgs = null;

            _mockLog = new Mock<ILog>();
            _mockLog.Setup(x => x.Debug(It.IsAny<object>()));
            _mockLog.Setup(x => x.Debug(It.IsAny<object>(), It.IsAny<Exception>()));
            _mockLog.Setup(x => x.DebugFormat(It.IsAny<string>(), It.IsAny<object>()));

            _mockLog.Setup(x => x.Info(It.IsAny<object>()));
            _mockLog.Setup(x => x.Info(It.IsAny<object>(), It.IsAny<Exception>()));

            _mockLog.Setup(x => x.Warn(It.IsAny<object>()));
            _mockLog.Setup(x => x.Warn(It.IsAny<object>(), It.IsAny<Exception>()));

            _mockLog.Setup(x => x.Error(It.IsAny<object>()));
            _mockLog.Setup(x => x.Error(It.IsAny<object>(), It.IsAny<Exception>()));

            _mockLog.Setup(x => x.Fatal(It.IsAny<object>()));
            _mockLog.Setup(x => x.Fatal(It.IsAny<object>(), It.IsAny<Exception>()));


            _wrapper = new LogWrapper(_mockLog.Object, LogLevel.Info);
            _wrapper.OnLoggingEvent += WrapperOnOnLoggingEvent;
        }

        [Test]
        public void ThrowLoggingEvent_NotSubscribed_Test()
        {
            _wrapper.OnLoggingEvent -= WrapperOnOnLoggingEvent;

            _wrapper.Debug("This is a test");

            Assert.That(_eventArgs, Is.Null.After(100));
        }

        [TestCase(LogLevel.Info, true)]
        [TestCase(LogLevel.Debug, true)]
        [TestCase(LogLevel.Warn, true)]
        [TestCase(LogLevel.Error, true)]
        [TestCase(LogLevel.Fatal, true)]
        [TestCase(LogLevel.None, false)]
        public void LogThisTest(LogLevel level, bool expectedValue)
        {
            Assert.That(_wrapper.LogThis(level), Is.EqualTo(expectedValue));
        }

        private static void WrapperOnOnLoggingEvent(object sender, LogEventArgs logEventArgs)
        {
            _eventArgs = logEventArgs;
        }

        [Test]
        public void Debug_Valid_Test()
        {
            _wrapper.Debug("This is a test");

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("This is a test"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Debug));
        }

        [Test]
        public void Debug_Exception_Test()
        {
            _wrapper.Debug(new Exception("Test Exception"));

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("Test Exception"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Debug));
        }

        [Test]
        public void Debug_1Arg_Test()
        {
            _wrapper.DebugFormat("This is a {0}", "test");

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("This is a test"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Debug));
        }

        [Test]
        public void Info_Valid_Test()
        {
            _wrapper.Info("This is a test");

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("This is a test"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Info));
        }

        [Test]
        public void Info_Exception_Test()
        {
            _wrapper.Info(new Exception("Test Exception"));

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("Test Exception"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Info));
        }

        [Test]
        public void Warn_Valid_Test()
        {
            _wrapper.Warn("This is a test");

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("This is a test"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Warn));
        }

        [Test]
        public void Warn_Exception_Test()
        {
            _wrapper.Warn(new Exception("Test Exception"));

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("Test Exception"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Warn));
        }
        [Test]
        public void Error_Valid_Test()
        {
            _wrapper.Error("This is a test");

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("This is a test"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Error));
        }

        [Test]
        public void Error_Exception_Test()
        {
            _wrapper.Error(new Exception("Test Exception"));

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("Test Exception"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Error));
        }

        [Test]
        public void Fatal_Valid_Test()
        {
            _wrapper.Fatal("This is a test");

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("This is a test"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Fatal));
        }

        [Test]
        public void Fatal_Exception_Test()
        {
            _wrapper.Fatal(new Exception("Test Exception"));

            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.Name, Is.EqualTo("Test Exception"));
            Assert.That(_eventArgs.Level, Is.EqualTo(LogLevel.Fatal));
        }
    }
}
