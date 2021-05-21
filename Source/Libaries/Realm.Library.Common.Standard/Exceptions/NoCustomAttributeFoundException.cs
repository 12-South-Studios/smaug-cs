using System;

namespace Realm.Library.Common.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class NoCustomAttributeFoundException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public NoCustomAttributeFoundException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }
}
