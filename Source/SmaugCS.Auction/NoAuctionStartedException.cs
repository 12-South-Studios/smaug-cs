using Realm.Library.Common.Exceptions;
using System;
using System.Runtime.Serialization;

namespace SmaugCS.Auction
{
    [Serializable]
    public class NoAuctionStartedException : BaseException
    {
        protected NoAuctionStartedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public NoAuctionStartedException(string msg) : base(msg)
        {
        }

        public NoAuctionStartedException(string msg, params object[] args)
            : base(string.Format(msg, args))
        {
        }
    }
}
