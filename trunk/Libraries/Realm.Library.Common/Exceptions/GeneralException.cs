using System;
using System.Runtime.Serialization;

// ReSharper disable CheckNamespace
namespace Realm.Library.Common
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class GeneralException : BaseException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected GeneralException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public GeneralException()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        public GeneralException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public GeneralException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public GeneralException(string msg, params object[] args)
            : base(string.Format(msg, args))
        {
        }
    }
}