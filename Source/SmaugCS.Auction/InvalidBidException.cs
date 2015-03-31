using System;
using System.Runtime.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Auction
{
    [Serializable]
    public class InvalidBidException : BaseException
    {
        protected InvalidBidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public InvalidBidException()
        {
        }

        public InvalidBidException(string msg) : base(msg)
        {
        }

        public InvalidBidException(string msg, Exception ex) : base(msg, ex)
        {
        }

        public InvalidBidException(string msg, params object[] args)
            : base(string.Format(msg, args))
        {
        }
    }
}
