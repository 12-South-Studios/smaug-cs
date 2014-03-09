using System;

// ReSharper disable CheckNamespace
namespace SmaugCS
// ReSharper restore CheckNamespace
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
