﻿using System;
using System.Runtime.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Exceptions
{
    [Serializable]
    public sealed class ObjectNotCarriedByCharacterException : BaseException
    {
        public ObjectNotCarriedByCharacterException() { }

        public ObjectNotCarriedByCharacterException(string message) : base(message) { }

        public ObjectNotCarriedByCharacterException(string format, params object[] args) 
            : base(string.Format(format, args)) { }

        public ObjectNotCarriedByCharacterException(string message, Exception inner) : base(message, inner) { }

        protected ObjectNotCarriedByCharacterException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}