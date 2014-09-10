using System;
using Realm.Library.Common;

namespace SmaugCS.Data.Exceptions
{
    [Serializable]
    public class ObjectNotFoundException : BaseException
    {
        public ObjectNotFoundException(string message) : base(message) { }
    }
}
