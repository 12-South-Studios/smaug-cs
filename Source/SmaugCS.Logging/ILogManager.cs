using System;
using Realm.Library.Common.Logging;

namespace SmaugCS.Logging
{
    public interface ILogManager
    {
        ILogWrapper LogWrapper { get; }

        void DatabaseFailureLog(string str, params object[] args);

        void Boot(string str, params object[] args);
        void Boot(Exception ex);

        void Bug(string str, params object[] args);
        void Bug(Exception ex);

        void Error(Exception ex);
        void Error(string str, params object[] args);

        void Info(string str, params object[] args);
    }
}
