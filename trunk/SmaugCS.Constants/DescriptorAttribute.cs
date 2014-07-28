using System;

namespace SmaugCS.Constants
{
    public class DescriptorAttribute : Attribute
    {
        public string[] Messages { get; set; }

        public DescriptorAttribute(string[] Messages = null)
        {
            this.Messages = Messages;
        }
    }
}
