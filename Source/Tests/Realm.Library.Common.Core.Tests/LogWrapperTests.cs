using System;
using Realm.Library.Common.Logging;
using log4net;
using FakeItEasy;
using Xunit;
using FluentAssertions;

namespace Realm.Library.Common.Fact
{
    public class LogWrapperFacts
    {
        private static LogEventArgs _eventArgs;
        private static ILog _mockLog;
        private static LogWrapper _wrapper;

        public LogWrapperFacts()
        {
            _eventArgs = null;

            _mockLog = A.Fake<ILog>();
            _wrapper = new LogWrapper(_mockLog, LogLevel.Info);
            _wrapper.OnLoggingEvent += WrapperOnOnLoggingEvent;
        }

        [Fact]
        public void ThrowLoggingEvent_NotSubscribed_Fact()
        {
            _wrapper.OnLoggingEvent -= WrapperOnOnLoggingEvent;

            _wrapper.Debug("This is a Fact");

            _eventArgs.Should().BeNull();
            //Assert.That(_eventArgs, Is.Null.After(100));
        }

        [Theory]
        [InlineData(LogLevel.Info, true)]
        [InlineData(LogLevel.Debug, true)]
        [InlineData(LogLevel.Warn, true)]
        [InlineData(LogLevel.Error, true)]
        [InlineData(LogLevel.Fatal, true)]
        [InlineData(LogLevel.None, false)]
        public void LogThisFact(LogLevel level, bool expectedValue)
        {
            _wrapper.LogThis(level).Should().Be(expectedValue);
        }

        private static void WrapperOnOnLoggingEvent(object sender, LogEventArgs logEventArgs)
        {
            _eventArgs = logEventArgs;
        }

        [Fact]
        public void Debug_Valid_Fact()
        {
            _wrapper.Debug("This is a Fact");

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("This is a Fact");
            _eventArgs.Level.Should().Be(LogLevel.Debug);
        }

        [Fact]
        public void Debug_Exception_Fact()
        {
            _wrapper.Debug(new Exception("Fact Exception"));

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("Fact Exception");
            _eventArgs.Level.Should().Be(LogLevel.Debug);
        }

        [Fact]
        public void Debug_1Arg_Fact()
        {
            _wrapper.DebugFormat("This is a {0}", "Fact");

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("This is a Fact");
            _eventArgs.Level.Should().Be(LogLevel.Debug);
        }

        [Fact]
        public void Info_Valid_Fact()
        {
            _wrapper.Info("This is a Fact");

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("This is a Fact");
            _eventArgs.Level.Should().Be(LogLevel.Info);
        }

        [Fact]
        public void Info_Exception_Fact()
        {
            _wrapper.Info(new Exception("Fact Exception"));

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("Fact Exception");
            _eventArgs.Level.Should().Be(LogLevel.Info);
        }

        [Fact]
        public void Warn_Valid_Fact()
        {
            _wrapper.Warn("This is a Fact");

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("This is a Fact");
            _eventArgs.Level.Should().Be(LogLevel.Warn);
        }

        [Fact]
        public void Warn_Exception_Fact()
        {
            _wrapper.Warn(new Exception("Fact Exception"));

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("Fact Exception");
            _eventArgs.Level.Should().Be(LogLevel.Warn);
        }

        [Fact]
        public void Error_Valid_Fact()
        {
            _wrapper.Error("This is a Fact");

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("This is a Fact");
            _eventArgs.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public void Error_Exception_Fact()
        {
            _wrapper.Error(new Exception("Fact Exception"));

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("Fact Exception");
            _eventArgs.Level.Should().Be(LogLevel.Error);
        }

        [Fact]
        public void Fatal_Valid_Fact()
        {
            _wrapper.Fatal("This is a Fact");

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("This is a Fact");
            _eventArgs.Level.Should().Be(LogLevel.Fatal);
        }

        [Fact]
        public void Fatal_Exception_Fact()
        {
            _wrapper.Fatal(new Exception("Fact Exception"));

            _eventArgs.Should().NotBeNull();
            _eventArgs.Name.Should().Be("Fact Exception");
            _eventArgs.Level.Should().Be(LogLevel.Fatal);
        }
    }
}
