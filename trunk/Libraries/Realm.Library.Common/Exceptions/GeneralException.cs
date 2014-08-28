using System;
using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    [Serializable]
    public class GeneralException : BaseException
    {
        protected GeneralException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public GeneralException()
        {
        }

        public GeneralException(string msg) : base(msg)
        {
        }

        public GeneralException(string msg, Exception ex) : base(msg, ex)
        {
        }

        public GeneralException(string msg, params object[] args) : base(string.Format(msg, args))
        {
        }
    }
}