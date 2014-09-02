using Realm.Library.Common;

namespace SmaugCS
{
    public class ObjectAlreadyExtractedException : BaseException
    {
        public ObjectAlreadyExtractedException(string format, params object[] args) 
            : base(string.Format(format, args))
        {
            
        }
    }
}
