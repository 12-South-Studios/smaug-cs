using System;
using System.Runtime.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Auction
{
    [Serializable]
    public class NoAuctionStartedException : BaseException
    {
        protected NoAuctionStartedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public NoAuctionStartedException()
        {
        }

        public NoAuctionStartedException(string msg) : base(msg)
        {
        }

        public NoAuctionStartedException(string msg, Exception ex) : base(msg, ex)
        {
        }

        public NoAuctionStartedException(string msg, params object[] args)
            : base(string.Format(msg, args))
        {
        }
    }
}
