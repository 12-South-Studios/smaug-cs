using System;
using System.Runtime.Serialization;
using Realm.Library.Common.Exceptions;

namespace SmaugCS.Data.Exceptions
{
    [Serializable]
    public class DuplicateEntryException : BaseException
    {
        public DuplicateEntryException() { }

        public DuplicateEntryException(string message)
            : base(message) { }

        public DuplicateEntryException(string message, params object[] args)
            : base(string.Format(message, args)) { }

        protected DuplicateEntryException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        public DuplicateEntryException(string message, Exception inner) : base(message, inner) { }
    }
}
