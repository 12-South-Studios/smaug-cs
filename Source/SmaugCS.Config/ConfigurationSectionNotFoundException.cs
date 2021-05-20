using System;
using System.Runtime.Serialization;
using Realm.Library.Common.Exceptions;

namespace SmaugCS.Config
{
    [Serializable]
    public class ConfigurationSectionNotFoundException : BaseException
    {
        protected ConfigurationSectionNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ConfigurationSectionNotFoundException()
        {
        }

        public ConfigurationSectionNotFoundException(string msg)
            : base(msg)
        {
        }

        public ConfigurationSectionNotFoundException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        public ConfigurationSectionNotFoundException(string msg, params object[] args)
            : base(string.Format(msg, args))
        {
        }
    }
}
