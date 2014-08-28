using System;
using Realm.Library.Common;

namespace SmaugCS.Data.Exceptions
{
    [Serializable]
    public class DuplicateEntryException : BaseException
    {
        public DuplicateEntryException() { }

        public DuplicateEntryException(string message)
            : base(message) { }

        public DuplicateEntryException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
