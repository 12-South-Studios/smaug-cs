using System;

namespace Realm.Library.Common.Exceptions
{
    public class InstanceNotFoundException : Exception
    {
        public InstanceNotFoundException(string message) : base(message) { }
    }
}
