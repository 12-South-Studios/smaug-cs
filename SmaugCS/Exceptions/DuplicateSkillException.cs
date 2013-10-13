using System;

namespace SmaugCS.Exceptions
{
    [Serializable]
    public class DuplicateSkillException : Exception
    {
        public DuplicateSkillException() { }

        public DuplicateSkillException(string message)
            : base(message) { }

        public DuplicateSkillException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
