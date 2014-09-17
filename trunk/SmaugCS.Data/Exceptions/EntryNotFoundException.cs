﻿using System;
using System.Runtime.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Data.Exceptions
{
    [Serializable]
    public class EntryNotFoundException : BaseException
    {
        public EntryNotFoundException() { }

        public EntryNotFoundException(string message)
            : base(message) { }

        public EntryNotFoundException(string message, params object[] args)
            : base(string.Format(message, args)) { }

        protected EntryNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        public EntryNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
