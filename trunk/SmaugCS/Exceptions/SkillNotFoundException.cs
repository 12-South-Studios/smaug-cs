using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmaugCS.Exceptions
{
    [Serializable]
    public class SkillNotFoundException : Exception
    {
        public SkillNotFoundException() { }

        public SkillNotFoundException(string message)
            : base(message) { }

        public SkillNotFoundException(string message, params object[] args)
            : base(string.Format(message, args)) { }
    }
}
