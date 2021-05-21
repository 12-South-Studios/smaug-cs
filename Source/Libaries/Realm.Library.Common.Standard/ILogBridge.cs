using System;

namespace Realm.Library.Common
{
    /// <summary>
    /// Declares a bridge contract for logging
    /// </summary>
    public interface ILogBridge
    {
        void Info(Exception ex);
        void Info(string message);
        void Info(string message, Exception ex);

        void Error(Exception ex);
        void Error(string message);
        void Error(string message, Exception ex);
    }
}