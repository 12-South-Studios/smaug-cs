using System;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///
    /// </summary>
    public static class FuncExtensions
    {
        /// <summary>
        /// Takes a function and retries its execution up to 3 times
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="action">Func to execute</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
                 Justification = "Retries execution three times and rethrows the Exception.")]
        public static T WithRetry<T>(this Func<T> action)
        {
            var result = default(T);
            int retryCount = 0;

            Exception caughtException = null;
            bool successful = false;
            do
            {
                try
                {
                    result = action();
                    successful = true;
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                    retryCount++;
                }
            } while (retryCount < 3 && !successful);

            if (!successful)
                throw caughtException;

            return result;
        }

        /// <summary>
        /// Returns a function that wraps the execution of that func that is passed
        /// as a parameter (and passing in the parameter it expects)
        /// </summary>
        /// <example>
        /// var client = new WebClient();
        /// Func&lt;string, string&gt; download = url => client.DownloadString(url);
        /// var data = download.Partial(url).WithRetry();
        /// </example>
        /// <typeparam name="TParam1">Type of parameter</typeparam>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <param name="func">Expects to be passed the parameter and returns a result</param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Func<TResult> Partial<TParam1, TResult>(
            this Func<TParam1, TResult> func, TParam1 parameter)
        {
            return () => func(parameter);
        }

        /// <summary>
        /// Transforms a function that takes N parameters into a function that
        /// you invoke to apply a parameter and get back as a result a function
        /// that takes N-1 parameters
        /// </summary>
        /// <example>
        /// var client = new WebClient();
        /// Func&lt;string, string&gt; download = url => client.DownloadString(url);
        /// Func&lt;string, Func&lt;string&gt;&gt; downloadCurry = download.Curry();
        /// var data = downloadCurry(url).WithRetry();
        /// </example>
        /// <typeparam name="TParam1">Type of parameter</typeparam>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <param name="func">Func that expects to be passed the paramter and returns a result</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Func<TParam1, Func<TResult>> Curry<TParam1, TResult>
            (this Func<TParam1, TResult> func)
        {
            return parameter => () => func(parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TParam1"></typeparam>
        /// <param name="executeFunc"></param>
        /// <param name="errorAction"></param>
        /// <param name="args"></param>
        public static void TryCatch<TParam1>(this Action<TParam1[]> executeFunc,
                                                 Action<Exception> errorAction = null,
                                                 params TParam1[] args)
        {
            if (executeFunc == null)
                throw new ArgumentNullException("executeFunc");

            try
            {
                executeFunc.Invoke(args);
            }
            catch (Exception ex)
            {
                if (errorAction != null)
                    errorAction.Invoke(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TParam1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="executeFunc"></param>
        /// <param name="errorAction"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static TResult TryCatch<TParam1, TResult>(this Func<TParam1[], TResult> executeFunc,
                                                         Func<Exception, TResult> errorAction = null,
                                                         params TParam1[] args)
        {
            if (executeFunc == null)
                throw new ArgumentNullException("executeFunc");

            try
            {
                return executeFunc.Invoke(args);
            }
            catch (Exception ex)
            {
                if (errorAction != null)
                    return errorAction.Invoke(ex);
            }
            throw new ArgumentException("No return value");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TParam1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="executeFunc"></param>
        /// <param name="errorAction"></param>
        /// <param name="finalAction"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static TResult TryCatch<TParam1, TResult, TException>(this Func<TParam1[], TResult> executeFunc,
                                                                     Func<TException, TResult> errorAction = null,
                                                                     Action<TParam1[]> finalAction = null,
                                                                     params TParam1[] args) where TException : Exception
        {
            if (executeFunc == null)
                throw new ArgumentNullException("executeFunc");

            try
            {
                return executeFunc.Invoke(args);
            }
            catch (TException ex)
            {
                if (errorAction != null)
                    return errorAction.Invoke(ex);
            }
            finally
            {
                if (finalAction != null)
                    finalAction.Invoke(args);
            }
            throw new ArgumentException("No return value specified");
        }
    }
}