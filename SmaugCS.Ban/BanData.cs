using System;

namespace SmaugCS.Ban
{
    public class BanData
    {
        public int Id { get; private set; }
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

        public BanData()
        {
            Type = BanTypes.Warn;
        }

        public BanData(int id, BanTypes banType)
        {
            Id = id;
            Type = banType;
        }
    }
}
