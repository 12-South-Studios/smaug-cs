using System;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Declares a bridge contract for logging
    /// </summary>
    public interface ILogBridge
    {
        /// <summary>
        /// Logs the exception as informational
        /// </summary>
        /// <param name="ex"></param>
        void Info(Exception ex);

        /// <summary>
        /// Logs the message as informational
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// Logs the message and exception as informational
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Info(string message, Exception ex);

        /// <summary>
        /// Logs the exception as an error
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);

        /// <summary>
        /// Logs the message as an error
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);

        /// <summary>
        /// Logs the message and exception as an error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Error(string message, Exception ex);
    }
}