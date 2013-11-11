using System;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Objects
{
    public class BanData
    {
        public BanTypes Type { get; private set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string BannedBy { get; set; }
        public DateTime BannedOn { get; set; }
        public int Flag { get; set; }
        public DateTime UnbanDate { get; set; }
        public int Duration { get; set; }
        public int Level { get; set; }
        public bool Warn { get; set; }
        public bool Prefix { get; set; }
        public bool Suffix { get; set; }

        public BanData(BanTypes banType)
        {
            Type = banType;
        }
    }
}
