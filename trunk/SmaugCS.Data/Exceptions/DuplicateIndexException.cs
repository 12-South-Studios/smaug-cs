﻿using System;
using System.Runtime.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Data.Exceptions
{
    [Serializable]
    public class DuplicateIndexException : BaseException
    {
        public DuplicateIndexException() { }

        public DuplicateIndexException(string message)
            : base(message) { }

        public DuplicateIndexException(string message, params object[] args)
            : base(string.Format(message, args)) { }

        public DuplicateIndexException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }

        public DuplicateIndexException(string message, Exception inner) : base(message, inner) { }
    }
}
