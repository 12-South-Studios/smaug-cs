using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Exceptions
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
