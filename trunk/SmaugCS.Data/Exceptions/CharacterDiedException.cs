using System;
using Realm.Library.Common;

namespace SmaugCS.Data.Exceptions
{
    [Serializable]
    public class CharacterDiedException : BaseException
    {
        public CharacterDiedException() { }

        public CharacterDiedException(string format, params object[] args) 
            : base(string.Format(format, args))
        {
        }

        /*public NewException(string, Exception)
protected or private NewException(SerializationInfo, StreamingContext)*/
    }
}
