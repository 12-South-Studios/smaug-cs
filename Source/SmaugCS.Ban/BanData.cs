using SmaugCS.Common.Enumerations;
using System;

namespace SmaugCS.Ban
{
    public class BanData
    {
        public int Id { get; set; }
        public BanTypes Type { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string BannedBy { get; set; }
        public DateTime BannedOn { get; set; }
        public int Flag { get; set; }
        public DateTime UnbanDate => Duration > 0 ? BannedOn.AddSeconds(Duration) : DateTime.MaxValue;
        public int Duration { get; set; }
        public int Level { get; set; }
        public bool Warn { get; set; }
        public bool Prefix { get; set; }
        public bool Suffix { get; set; }
        public bool Saved { get; set; }

        public BanData()
        {
            Saved = false;
        }

        public bool IsExpired() => UnbanDate <= DateTime.Now;
    }
}
