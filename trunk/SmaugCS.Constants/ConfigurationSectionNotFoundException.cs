using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Realm.Library.Common.Exceptions;

namespace SmaugCS.Constants
{
    public class ConfigurationSectionNotFoundException : BaseException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ConfigurationSectionNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public ConfigurationSectionNotFoundException()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        public ConfigurationSectionNotFoundException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public ConfigurationSectionNotFoundException(string msg, Exception ex)
            : base(msg, ex)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public ConfigurationSectionNotFoundException(string msg, params object[] args)
            : base(string.Format(msg, args))
        {
        }
    }
}
