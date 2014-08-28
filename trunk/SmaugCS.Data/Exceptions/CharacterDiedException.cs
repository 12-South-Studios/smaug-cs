using Realm.Library.Common;

namespace SmaugCS.Data.Exceptions
{
    public class CharacterDiedException : BaseException
    {
        public CharacterDiedException() { }

        public CharacterDiedException(string format, params object[] args) 
            : base(string.Format(format, args))
        {
        }
    }
}
