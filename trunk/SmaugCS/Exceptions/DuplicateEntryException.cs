using System;

namespace SmaugCS.Exceptions
{
    [Serializable]
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException() { }

        public DuplicateEntryException(string message)
            : base(message) { }

        public DuplicateEntryException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
