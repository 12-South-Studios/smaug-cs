using System;
using System.Runtime.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Exceptions
{
    [Serializable]
    public sealed class ObjectAlreadyEquippedException : BaseException
    {
        public ObjectAlreadyEquippedException() { }

        public ObjectAlreadyEquippedException(string message) : base(message) { }

        public ObjectAlreadyEquippedException(string format, params object[] args) 
            : base(string.Format(format, args)) { }

        public ObjectAlreadyEquippedException(string message, Exception inner) : base(message, inner) { }

        protected ObjectAlreadyEquippedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
