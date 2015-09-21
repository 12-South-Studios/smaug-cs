using System;
using System.Collections.Generic;

namespace SmaugCS.Constants
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class DescriptorAttribute : Attribute
    {
        public IEnumerable<string> Messages { get; private set; }

        public DescriptorAttribute(params string[] Messages)
        {
            this.Messages = Messages;
        }
    }
}
