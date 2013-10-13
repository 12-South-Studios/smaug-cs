using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Exceptions
{
    public class DuplicateIndexException : Exception
    {
        public DuplicateIndexException() { }

        public DuplicateIndexException(string message)
            : base(message) { }

        public DuplicateIndexException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
