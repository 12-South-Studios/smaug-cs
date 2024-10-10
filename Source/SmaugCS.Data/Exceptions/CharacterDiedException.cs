using Library.Common.Exceptions;
using System;
using System.Runtime.Serialization;

namespace SmaugCS.Data.Exceptions;

[Serializable]
public class CharacterDiedException : BaseException
{
  public CharacterDiedException()
  {
  }

  public CharacterDiedException(string message) : base(message)
  {
  }

  public CharacterDiedException(string format, params object[] args)
    : base(string.Format(format, args))
  {
  }

  public CharacterDiedException(string message, Exception inner) : base(message, inner)
  {
  }

  protected CharacterDiedException(SerializationInfo info, StreamingContext context) : base(info, context)
  {
  }
}