using System;

namespace SmaugCS.Communication
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ChannelPrintAttribute : Attribute
    {
        public string On { get; set; }
        public string Off { get; set; }

        public ChannelPrintAttribute(string On = "", string Off = "")
        {
            this.On = On;
            this.Off = Off;
        }
    }
}
