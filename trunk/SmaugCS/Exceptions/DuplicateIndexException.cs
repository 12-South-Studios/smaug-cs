using System;

namespace SmaugCS.Exceptions
{
    [Serializable]
    public class DuplicateIndexException : Exception
    {
        public DuplicateIndexException() { }

        public DuplicateIndexException(string message)
            : base(message) { }

        public DuplicateIndexException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
