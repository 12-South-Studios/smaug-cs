using System;
using System.Diagnostics.CodeAnalysis;
using log4net;
using log4net.Core;

namespace Realm.Library.Common.Logging
{
    public class LogWrapper : ILogWrapper
    {
        private readonly int _minLoggingLevel;

        public event EventHandler<LogEventArgs> OnLoggingEvent;

        public LogWrapper()
            : this(LogManager.GetLogger(typeof(LogWrapper)), LogLevel.Error)
        {
        }

        public LogWrapper(ILog log, LogLevel level)
        {
            Log = log;
            _minLoggingLevel = (int)level;

            IsDebugEnabled = true;
            IsErrorEnabled = true;
            IsFatalEnabled = true;
            IsInfoEnabled = true;
            IsWarnEnabled = true;
        }

        public virtual ILog Log { get; }

        [ExcludeFromCodeCoverage]
        public virtual ILogger Logger => Log.Logger;

        public virtual int MinLoggingLevel => _minLoggingLevel;

        public virtual bool LogThis(LogLevel logLevel)
        {
            return (int)logLevel >= _minLoggingLevel;
        }

        private void ThrowLogEvent(LogLevel logLevel, string eventName, string eventLog)
        {
            OnLoggingEvent?.Invoke(this, new LogEventArgs
            {
                Level = logLevel,
                Name = eventName,
                Text = eventLog
            });
        }

        #region Debug

        public virtual void Debug(object message)
        {
            Validation.IsNotNull(message, "message");

            if (LogThis(LogLevel.Debug))
            {
                if (message is Exception)
                {
                    var exception = message as Exception;
                    Debug(exception.Message, exception);
                }
                else
                    ThrowLogEvent(LogLevel.Debug, message.ToString(), string.Empty);
            }
            Log.Debug(message);
        }

        public virtual void Debug(object message, Exception exception)
        {
            Validation.IsNotNull(message, "message");
            Validation.IsNotNull(exception, "exception");

            if (LogThis(LogLevel.Debug))
                ThrowLogEvent(LogLevel.Debug, message.ToString(), exception.StackTrace);
            Log.Debug(message, exception);
        }

        public virtual void DebugFormat(string format, params object[] args)
        {
            if (LogThis(LogLevel.Debug))
                ThrowLogEvent(LogLevel.Debug, string.Format(format, args), string.Empty);
            Log.DebugFormat(format, args);
        }

        public virtual void DebugFormat(string format, object arg0)
        {
            if (LogThis(LogLevel.Debug))
                ThrowLogEvent(LogLevel.Debug, string.Format(format, arg0), string.Empty);
            Log.DebugFormat(format, arg0);
        }

        public virtual void DebugFormat(string format, object arg0, object arg1)
        {
            if (LogThis(LogLevel.Debug))
                ThrowLogEvent(LogLevel.Debug, string.Format(format, arg0, arg1), string.Empty);
            Log.DebugFormat(format, arg0, arg1);
        }

        public virtual void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (LogThis(LogLevel.Debug))
                ThrowLogEvent(LogLevel.Debug, string.Format(format, arg0, arg1, arg2), string.Empty);
            Log.DebugFormat(format, arg0, arg1, arg2);
        }

        public virtual void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (LogThis(LogLevel.Debug))
                ThrowLogEvent(LogLevel.Debug, string.Format(provider, format, args), string.Empty);
            Log.DebugFormat(provider, format, args);
        }

        #endregion Debug

        #region Info

        public virtual void Info(object message)
        {
            Validation.IsNotNull(message, "message");

            if (LogThis(LogLevel.Info))
            {
                if (message is Exception)
                {
                    var exception = message as Exception;
                    Info(exception.Message, exception);
                }
                else
                    ThrowLogEvent(LogLevel.Info, message.ToString(), string.Empty);
            }
            Log.Info(message);
        }

        public virtual void Info(object message, Exception exception)
        {
            Validation.IsNotNull(message, "message");
            Validation.IsNotNull(exception, "exception");

            if (LogThis(LogLevel.Info))
                ThrowLogEvent(LogLevel.Info, message.ToString(), exception.StackTrace);
            Log.Info(message, exception);
        }

        public virtual void InfoFormat(string format, params object[] args)
        {
            if (LogThis(LogLevel.Info))
                ThrowLogEvent(LogLevel.Info, string.Format(format, args), string.Empty);
            Log.InfoFormat(format, args);
        }

        public virtual void InfoFormat(string format, object arg0)
        {
            if (LogThis(LogLevel.Info))
                ThrowLogEvent(LogLevel.Info, string.Format(format, arg0), string.Empty);
            Log.InfoFormat(format, arg0);
        }

        public virtual void InfoFormat(string format, object arg0, object arg1)
        {
            if (LogThis(LogLevel.Info))
                ThrowLogEvent(LogLevel.Info, string.Format(format, arg0, arg1), string.Empty);
            Log.InfoFormat(format, arg0, arg1);
        }

        public virtual void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (LogThis(LogLevel.Info))
                ThrowLogEvent(LogLevel.Info, string.Format(format, arg0, arg1, arg2), string.Empty);
            Log.InfoFormat(format, arg0, arg1, arg2);
        }

        public virtual void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (LogThis(LogLevel.Info))
                ThrowLogEvent(LogLevel.Info, string.Format(provider, format, args), string.Empty);
            Log.InfoFormat(provider, format, args);
        }

        #endregion Info

        #region Warn

        public virtual void Warn(object message)
        {
            Validation.IsNotNull(message, "message");

            if (LogThis(LogLevel.Warn))
            {
                if (message is Exception)
                {
                    var exception = message as Exception;
                    Warn(exception.Message, exception);
                }
                else
                    ThrowLogEvent(LogLevel.Warn, message.ToString(), string.Empty);
            }
            Log.Warn(message);
        }

        public virtual void Warn(object message, Exception exception)
        {
            Validation.IsNotNull(message, "message");
            Validation.IsNotNull(exception, "exception");

            if (LogThis(LogLevel.Warn))
                ThrowLogEvent(LogLevel.Warn, message.ToString(), exception.StackTrace);
            Log.Warn(message, exception);
        }

        public virtual void WarnFormat(string format, params object[] args)
        {
            if (LogThis(LogLevel.Warn))
                ThrowLogEvent(LogLevel.Warn, string.Format(format, args), string.Empty);
            Log.WarnFormat(format, args);
        }

        public virtual void WarnFormat(string format, object arg0)
        {
            if (LogThis(LogLevel.Warn))
                ThrowLogEvent(LogLevel.Warn, string.Format(format, arg0), string.Empty);
            Log.WarnFormat(format, arg0);
        }

        public virtual void WarnFormat(string format, object arg0, object arg1)
        {
            if (LogThis(LogLevel.Warn))
                ThrowLogEvent(LogLevel.Warn, string.Format(format, arg0, arg1), string.Empty);
            Log.WarnFormat(format, arg0, arg1);
        }

        public virtual void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (LogThis(LogLevel.Warn))
                ThrowLogEvent(LogLevel.Warn, string.Format(format, arg0, arg1, arg2), string.Empty);
            Log.WarnFormat(format, arg0, arg1, arg2);
        }

        public virtual void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (LogThis(LogLevel.Warn))
                ThrowLogEvent(LogLevel.Warn, string.Format(provider, format, args), string.Empty);
            Log.WarnFormat(provider, format, args);
        }

        #endregion Warn

        #region Error

        public virtual void Error(object message)
        {
            Validation.IsNotNull(message, "message");

            if (LogThis(LogLevel.Error))
            {
                if (message is Exception)
                {
                    var exception = message as Exception;
                    Error(exception.Message, exception);
                }
                else
                    ThrowLogEvent(LogLevel.Error, message.ToString(), string.Empty);
            }
            Log.Error(message);
        }

        public virtual void Error(object message, Exception exception)
        {
            Validation.IsNotNull(message, "message");
            Validation.IsNotNull(exception, "exception");

            if (LogThis(LogLevel.Error))
                ThrowLogEvent(LogLevel.Error, exception.Message, $"{exception}//{exception.StackTrace}");
            Log.Error(message, exception);
        }

        public virtual void ErrorFormat(string format, params object[] args)
        {
            if (LogThis(LogLevel.Error))
                ThrowLogEvent(LogLevel.Error, string.Format(format, args), string.Empty);
            Log.ErrorFormat(format, args);
        }

        public virtual void ErrorFormat(string format, object arg0)
        {
            if (LogThis(LogLevel.Error))
                ThrowLogEvent(LogLevel.Error, string.Format(format, arg0), string.Empty);
            Log.ErrorFormat(format, arg0);
        }

        public virtual void ErrorFormat(string format, object arg0, object arg1)
        {
            if (LogThis(LogLevel.Error))
                ThrowLogEvent(LogLevel.Error, string.Format(format, arg0, arg1), string.Empty);
            Log.ErrorFormat(format, arg0, arg1);
        }

        public virtual void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            if (LogThis(LogLevel.Error))
                ThrowLogEvent(LogLevel.Error, string.Format(format, arg0, arg1, arg2), string.Empty);
            Log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public virtual void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (LogThis(LogLevel.Error))
                ThrowLogEvent(LogLevel.Error, string.Format(provider, format, args), string.Empty);
            Log.ErrorFormat(provider, format, args);
        }

        #endregion Error

        #region Fatal

        public virtual void Fatal(object message)
        {
            Validation.IsNotNull(message, "message");

            if (LogThis(LogLevel.Fatal))
            {
                if (message is Exception)
                {
                    var exception = message as Exception;
                    Fatal(exception.Message, exception);
                }
                else
                    ThrowLogEvent(LogLevel.Fatal, message.ToString(), string.Empty);
            }
            Log.Fatal(message);
        }

        public virtual void Fatal(object message, Exception exception)
        {
            Validation.IsNotNull(message, "message");
            Validation.IsNotNull(exception, "exception");

            if (LogThis(LogLevel.Fatal))
                ThrowLogEvent(LogLevel.Fatal, message.ToString(), exception.StackTrace);
            Log.Fatal(message, exception);
        }

        public virtual void FatalFormat(string format, params object[] args)
        {
            if (LogThis(LogLevel.Fatal))
                ThrowLogEvent(LogLevel.Fatal, string.Format(format, args), string.Empty);
            Log.FatalFormat(format, args);
        }

        public virtual void FatalFormat(string format, object arg0)
        {
            if (LogThis(LogLevel.Fatal))
                ThrowLogEvent(LogLevel.Fatal, string.Format(format, arg0), string.Empty);
            Log.FatalFormat(format, arg0);
        }

        public virtual void FatalFormat(string format, object arg0, object arg1)
        {
            if (LogThis(LogLevel.Fatal))
                ThrowLogEvent(LogLevel.Fatal, string.Format(format, arg0, arg1), string.Empty);
            Log.FatalFormat(format, arg0, arg1);
        }

        public virtual void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            if (LogThis(LogLevel.Fatal))
                ThrowLogEvent(LogLevel.Fatal, string.Format(format, arg0, arg1, arg2), string.Empty);
            Log.FatalFormat(format, arg0, arg1, arg2);
        }

        public virtual void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (LogThis(LogLevel.Fatal))
                ThrowLogEvent(LogLevel.Fatal, string.Format(provider, format, args), string.Empty);
            Log.FatalFormat(provider, format, args);
        }

        #endregion Fatal

        public bool IsDebugEnabled { get; set; }

        public bool IsInfoEnabled { get; set; }

        public bool IsWarnEnabled { get; set; }

        public bool IsErrorEnabled { get; set; }

        public bool IsFatalEnabled { get; set; }
    }
}