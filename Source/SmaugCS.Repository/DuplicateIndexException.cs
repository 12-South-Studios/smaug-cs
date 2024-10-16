﻿using Library.Common.Exceptions;
using System;
using System.Runtime.Serialization;

namespace SmaugCS.Repository;

[Serializable]
public class DuplicateIndexException : BaseException
{
    public DuplicateIndexException() { }

    public DuplicateIndexException(string message)
        : base(message) { }

    public DuplicateIndexException(string message, params object[] args)
        : base(string.Format(message, args)) { }

    protected DuplicateIndexException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    public DuplicateIndexException(string message, Exception inner) : base(message, inner) { }
}