using System;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ChannelPrintAttribute : Attribute
    {
        public string On { get; set; }
        public string Off { get; set; }
    }
}
