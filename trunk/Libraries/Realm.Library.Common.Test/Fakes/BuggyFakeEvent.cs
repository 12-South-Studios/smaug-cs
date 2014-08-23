using System;

namespace Realm.Library.Common.Test.Fakes
{
    public class BuggyFakeEvent : EventBase
    {
        public BuggyFakeEvent()
        {
            throw new Exception("Fail!");
        }
    }
}
