using System;

namespace SmaugCS.Exceptions
{
    public class CharacterDiedException : Exception
    {
        public CharacterDiedException() { }

        public CharacterDiedException(string format, params object[] args) 
            : base(string.Format(format, args))
        {
        }
    }
}
