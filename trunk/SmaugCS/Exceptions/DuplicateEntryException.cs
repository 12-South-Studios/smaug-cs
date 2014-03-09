using System;

// ReSharper disable CheckNamespace
namespace SmaugCS
// ReSharper restore CheckNamespace
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
