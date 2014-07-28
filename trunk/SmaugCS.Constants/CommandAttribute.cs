using System;

namespace SmaugCS.Constants
{
    public class CommandAttribute : Attribute
    {
        public bool NoNpc { get; set; }

        public CommandAttribute(bool NoNpc = false)
        {
            this.NoNpc = NoNpc;
        }
    }
}
