using Library.Common.Exceptions;
using System;
using System.Runtime.Serialization;

namespace SmaugCS.Data.Exceptions;

[Serializable]
public class ObjectNotFoundException : BaseException
{
    public ObjectNotFoundException() { }

    public ObjectNotFoundException(string message) : base(message) { }

    public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException) { }

    protected ObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}