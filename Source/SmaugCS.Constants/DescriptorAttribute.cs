using System;
using System.Collections.Generic;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class DescriptorAttribute : Attribute
    {
        public IEnumerable<string> Messages { get; private set; }

        public DescriptorAttribute(string[] Messages = null)
        {
            this.Messages = Messages;
        }
    }
}
