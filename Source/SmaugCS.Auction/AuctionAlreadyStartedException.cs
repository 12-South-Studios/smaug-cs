using Library.Common.Exceptions;
using System;
using System.Runtime.Serialization;

namespace SmaugCS.Auction;

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