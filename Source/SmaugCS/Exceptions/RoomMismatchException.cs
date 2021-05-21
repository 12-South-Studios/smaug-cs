using Realm.Library.Common.Exceptions;
using System;
using System.Runtime.Serialization;

namespace SmaugCS.Exceptions
{
    [Serializable]
    public class RoomMismatchException : BaseException
    {
        public RoomMismatchException() { }

        public RoomMismatchException(string message) : base(message) { }

        public RoomMismatchException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public RoomMismatchException(string message, Exception inner) : base(message, inner) { }

        protected RoomMismatchException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
