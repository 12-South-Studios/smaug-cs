using Microsoft.Extensions.Logging;
using System;

namespace Realm.Library.Common.Logging
{
    public interface ILogWrapper
    {
        ILogger Logger { get; }

        void Info(string message, params object[] args);

        void Error(Exception ex);

        void Error(string message, params object[] args);

        void Debug(string message, params object[] args);

        void Warn(string message, params object[] args);

        void Fatal(string message, params object[] args);

        void Log(int logType, string message, params object[] args);
    }
}
