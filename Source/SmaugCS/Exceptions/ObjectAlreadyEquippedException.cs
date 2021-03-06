﻿using Realm.Library.Common.Exceptions;
using System;
using System.Runtime.Serialization;

namespace SmaugCS
{
    [Serializable]
    public sealed class ObjectAlreadyEquippedException : BaseException
    {
        public ObjectAlreadyEquippedException() { }

        public ObjectAlreadyEquippedException(string message) : base(message) { }

        public ObjectAlreadyEquippedException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public ObjectAlreadyEquippedException(string message, Exception inner) : base(message, inner) { }

        private ObjectAlreadyEquippedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
