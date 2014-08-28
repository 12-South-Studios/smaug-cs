using System;
using Realm.Library.Common;

namespace SmaugCS.Data.Exceptions
{
    [Serializable]
    public class EntryNotFoundException : BaseException
    {
        public EntryNotFoundException() { }

        public EntryNotFoundException(string message)
            : base(message) { }

        public EntryNotFoundException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
