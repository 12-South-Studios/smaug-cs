using System;
using System.Diagnostics;
using log4net;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Properties;

#pragma warning disable 1591


namespace Realm.Library.Common.Exceptions

{
    /// <summary>
    /// Class that handles extension functions to Exception objects
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Handles exceptions based upon the indicated behavior and throws a new exception of the given
        /// type, assigning the original exception as the InnerException
        /// </summary>
        public static void Handle<T>(this Exception exception, ExceptionHandlingOptions exceptionBehavior, ILogWrapper log = null,
            string msg = "", params object[] parameters) where T : Exception
        {
            var caller = GetCaller();
            if (exceptionBehavior == ExceptionHandlingOptions.RecordOnly || exceptionBehavior == ExceptionHandlingOptions.RecordAndThrow)
            {
                ILogWrapper logger = log ?? new LogWrapper(LogManager.GetLogger(caller), LogLevel.Error);

                if (string.IsNullOrEmpty(msg))
                    logger.Error(exception);
                else
                    logger.Error(string.Format(msg, parameters), exception);
            }

            if (exceptionBehavior == ExceptionHandlingOptions.RecordAndThrow || exceptionBehavior == ExceptionHandlingOptions.ThrowOnly)
                throw (T)Activator.CreateInstance(typeof(T), string.Format(Resources.MSG_EXCEPTION_LOCATION, caller), exception);
        }

        /// <summary>
        /// Handles exceptions based upon the indicated behavior and rethrows the Exception
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="exceptionBehavior"></param>
        /// <param name="log"></param>
        /// <param name="msg"></param>
        /// <param name="parameters"></param>
        public static void Handle(this Exception exception, ExceptionHandlingOptions exceptionBehavior, ILogWrapper log = null,
                                  string msg = "", params object[] parameters)
        {
            var caller = GetCaller();
            if (exceptionBehavior == ExceptionHandlingOptions.RecordOnly || exceptionBehavior == ExceptionHandlingOptions.RecordAndThrow)
            {
                ILogWrapper logger = log ?? new LogWrapper(LogManager.GetLogger(caller), LogLevel.Error);

                if (string.IsNullOrEmpty(msg))
                    logger.Error(exception);
                else
                    logger.Error(string.Format(msg, parameters), exception);
            }

            if (exceptionBehavior == ExceptionHandlingOptions.RecordAndThrow ||
                exceptionBehavior == ExceptionHandlingOptions.ThrowOnly)
                throw exception;
        }

        /// <summary>
        /// Gets the calling class and method for the current stack
        /// </summary>
        /// <returns>Returns the full name of the class (with namespace) and the calling method</returns>
        private static string GetCaller(int level = 2)
        {
            var method = new StackTrace().GetFrame(level).GetMethod();
            return method?.DeclaringType == null ? string.Empty : $"{method.DeclaringType.FullName}:{method.Name}";
        }
    }
}

#pragma warning restore 1591