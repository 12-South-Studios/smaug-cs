using Library.Common.Exceptions;
using System;
using System.Runtime.Serialization;

namespace SmaugCS.Auction;

[Serializable]
public class InvalidBidException : BaseException
{
  protected InvalidBidException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }

  public InvalidBidException(string msg, params object[] args)
    : base(string.Format(msg, args))
  {
  }
}