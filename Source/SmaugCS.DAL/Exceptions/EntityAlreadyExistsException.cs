using System;

namespace SmaugCS.DAL.Exceptions
{
    [Serializable]
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}
