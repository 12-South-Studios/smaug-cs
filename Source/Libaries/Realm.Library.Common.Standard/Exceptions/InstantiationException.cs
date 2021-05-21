using System;
using System.Runtime.Serialization;

namespace Realm.Library.Common.Exceptions

{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class InstantiationException : BaseException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InstantiationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public InstantiationException()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        public InstantiationException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public InstantiationException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public InstantiationException(string msg, params object[] args)
            : base(string.Format(msg, args))
        {
        }
    }
}