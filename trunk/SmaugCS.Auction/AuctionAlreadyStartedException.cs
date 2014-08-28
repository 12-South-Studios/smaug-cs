using System;
using System.Runtime.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Auction
{
    public class AuctionAlreadyStartedException : BaseException
    {
        protected AuctionAlreadyStartedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public AuctionAlreadyStartedException()
        {
        }

        public AuctionAlreadyStartedException(string msg) : base(msg)
        {
        }

        public AuctionAlreadyStartedException(string msg, Exception ex) : base(msg, ex)
        {
        }

        public AuctionAlreadyStartedException(string msg, params object[] args)
            : base(string.Format(msg, args))
        {
        }
    }
}
