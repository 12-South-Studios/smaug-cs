using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Realm.Library.Common.Exceptions

{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class BaseException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public string ResourceReferenceProperty { get; }
 
        /// <summary>
        /// 
        /// </summary>
        protected BaseException()
        {
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        protected BaseException(string message)
            : base(message)
        {
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        protected BaseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("ResourceReferenceProperty", ResourceReferenceProperty);
            base.GetObjectData(info, context);
        }
    }
}
