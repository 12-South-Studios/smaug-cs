using SmaugCS.Enums;

namespace SmaugCS.Objects
{
    public class MudProgData
    {
        public MudProgTypes Type { get; set; }
        public bool triggered { get; set; }
        public int resetdelay { get; set; }
        public string arglist { get; set; }
        public string comlist { get; set; }
        public bool IsFileProg { get; set; }
    }
}
