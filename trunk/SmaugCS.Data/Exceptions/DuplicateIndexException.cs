using System;
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
    }
}
