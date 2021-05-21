using System;
using System.Runtime.Serialization;

namespace Realm.Library.Common.Exceptions

{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class InitializationException : BaseException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public InitializationException()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        public InitializationException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public InitializationException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public InitializationException(string msg, params object[] args)
            : base(string.Format(msg, args))
        {
        }
    }
}