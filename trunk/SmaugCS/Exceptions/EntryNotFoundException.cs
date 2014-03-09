using System;

// ReSharper disable CheckNamespace
namespace SmaugCS
// ReSharper restore CheckNamespace
{
    [Serializable]
    public class EntryNotFoundException : Exception
    {
        public EntryNotFoundException() { }

        public EntryNotFoundException(string message)
            : base(message) { }

        public EntryNotFoundException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
