using System;
using Realm.Library.Common.Logging;
using log4net;

namespace SmaugCS.Logging
{
    public interface ILogManager
    {
        ILogWrapper LogWrapper { get; }

        void Initialize(ILogWrapper logWrapper, string path);

        void BootLog(string str, params object[] args);
        void BootLog(Exception ex);

        void Bug(string str, params object[] args);

        void Error(Exception ex);
        void Error(string str, params object[] args);

        void Log(string str, params object[] args);
        void Log(LogTypes logType, int level, string fmt, params object[] args);
        void Log(string str, LogTypes logType, int level);
    }
}
