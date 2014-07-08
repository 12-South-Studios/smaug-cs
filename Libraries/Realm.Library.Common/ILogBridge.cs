using System;

namespace Realm.Library.Common
{
    /// <summary>
    /// Declares a bridge contract for logging
    /// </summary>
    public interface ILogBridge
    {
        /// <summary>
        /// Logs an Exception to the Info path
        /// </summary>
        /// <param name="ex"></param>
        void Info(Exception ex);

        /// <summary>
        /// Logs a string message to the Info path
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// Logs a string message and Exception to the Info path
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Info(string message, Exception ex);


        /// <summary>
        /// Logs an Exception to the Error path
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);

        /// <summary>
        /// Logs a string message to the Error path
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);

        /// <summary>
        /// Logs a string message and Exception to the Error path
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Error(string message, Exception ex);
    }
}