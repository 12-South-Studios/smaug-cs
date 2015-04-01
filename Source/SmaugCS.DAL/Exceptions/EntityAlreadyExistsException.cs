using System;

namespace SmaugCS.DAL.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}
