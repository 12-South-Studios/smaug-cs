using System;
using Realm.Library.Common.Logging;
using log4net;

namespace SmaugCS.Logging
{
    public interface ILogManager
    {
        ILogWrapper LogWrapper { get; }

        void Initialize(ILogWrapper logWrapper, string path);

        void Boot(string str, params object[] args);
        void Boot(Exception ex);

        void Bug(string str, params object[] args);
        void Bug(Exception ex);

        void Error(Exception ex);
        void Error(string str, params object[] args);

        void Log(string str, params object[] args);
        void Log(LogTypes logType, int level, string fmt, params object[] args);
    }
}
