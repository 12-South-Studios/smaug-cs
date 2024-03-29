﻿using Realm.Library.Common.Extensions;
using Realm.Library.Common.Logging;
using System;
using System.Diagnostics;

namespace Realm.Library.Common
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
        public static void Handle<T>(this Exception exception, ExceptionHandlingOptions exceptionBehavior, ILogWrapper log,
            string msg = "", params object[] parameters) where T : Exception
        {
            var caller = GetCaller();
            if (exceptionBehavior == ExceptionHandlingOptions.RecordOnly || exceptionBehavior == ExceptionHandlingOptions.RecordAndThrow)
            {
                if (string.IsNullOrEmpty(msg))
                    log.Error(exception);
                else
                    log.Error(String.Format(msg, parameters), exception);
            }

            if (exceptionBehavior == ExceptionHandlingOptions.RecordAndThrow || exceptionBehavior == ExceptionHandlingOptions.ThrowOnly)
                throw (T)Activator.CreateInstance(typeof(T), string.Format("Exception occurred in {0}", caller), exception);
        }

        /// <summary>
        /// Handles exceptions based upon the indicated behavior and rethrows the Exception
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="exceptionBehavior"></param>
        /// <param name="log"></param>
        /// <param name="msg"></param>
        /// <param name="parameters"></param>
        public static void Handle(this Exception exception, ExceptionHandlingOptions exceptionBehavior, ILogWrapper log,
                                  string msg = "", params object[] parameters)
        {
            var caller = GetCaller();
            if (exceptionBehavior == ExceptionHandlingOptions.RecordOnly || exceptionBehavior == ExceptionHandlingOptions.RecordAndThrow)
            {
                if (string.IsNullOrEmpty(msg))
                    log.Error(exception);
                else
                    log.Error(String.Format(msg, parameters), exception);
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
            if (method == null || method.DeclaringType == null)
                return string.Empty;

            return string.Format("{0}:{1}", method.DeclaringType.FullName, method.Name);
        }
    }
}