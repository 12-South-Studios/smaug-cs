using System;
using System.Runtime.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Auction
{
    [Serializable]
    public class AuctionAlreadyStartedException : BaseException
    {
        protected AuctionAlreadyStartedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public AuctionAlreadyStartedException(string msg, params object[] args)
            : base(string.Format(msg, args))
        {
        }
    }
}
