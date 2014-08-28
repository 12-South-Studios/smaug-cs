using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable once CheckNamespace
namespace Realm.Library.Common
{
    [Serializable]
    public abstract class BaseException : Exception
    {
        private string ResourceReferenceProperty { get; set; }
 
        protected BaseException()
        {
        }

        protected BaseException(string message)
            : base(message)
        {
        }
 
        protected BaseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
        }
 
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            info.AddValue("ResourceReferenceProperty", ResourceReferenceProperty);
            base.GetObjectData(info, context);
        }
    }
}
