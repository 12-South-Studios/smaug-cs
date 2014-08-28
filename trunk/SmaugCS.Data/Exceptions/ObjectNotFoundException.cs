using Realm.Library.Common;

namespace SmaugCS.Data.Exceptions
{
    public class ObjectNotFoundException : BaseException
    {
        public ObjectNotFoundException(string message) : base(message) { }
    }
}
