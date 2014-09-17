using System;
using System.Runtime.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Exceptions
{
    [Serializable]
    public sealed class ObjectAlreadyExtractedException : BaseException
    {
        public ObjectAlreadyExtractedException() { }

        public ObjectAlreadyExtractedException(string message) : base(message) { }

        public ObjectAlreadyExtractedException(string format, params object[] args) 
            : base(string.Format(format, args)) { }

        public ObjectAlreadyExtractedException(string message, Exception inner) : base(message, inner) { }

        protected ObjectAlreadyExtractedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
