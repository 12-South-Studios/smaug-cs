using System;

namespace Realm.Library.Common.Test.Fakes
{
    public class FakeException : Exception
    {
        public FakeException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
