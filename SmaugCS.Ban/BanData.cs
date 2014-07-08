using System;
using System.Data;
using Realm.Library.Common;
using SmaugCS.Common;

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
        public DateTime UnbanDate { get { return Duration > 0 ? BannedOn.AddSeconds(Duration) : DateTime.MaxValue; } }
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

        public bool IsExpired()
        {
            return UnbanDate <= DateTime.Now;
        }

        public static BanData Translate(DataRow dataRow)
        {
            BanData ban = new BanData(Convert.ToInt32(dataRow["BanId"]),
                          Realm.Library.Common.EnumerationExtensions.GetEnumByName<BanTypes>(dataRow["BanType"].ToString()))
                {
                    Name = dataRow.GetDataValue("Name", string.Empty),
                    Note = dataRow.GetDataValue("Note", string.Empty),
                    BannedBy = dataRow.GetDataValue("BannedBy", string.Empty),
                    BannedOn = dataRow.GetDataValue("BannedOn", DateTime.MinValue),
                    Duration = dataRow.GetDataValue("Duration", 0),
                    Level = dataRow.GetDataValue("Level", 0),
                    Warn = dataRow.GetDataValue("Warn", false),
                    Prefix = dataRow.GetDataValue("Prefix", false),
                    Suffix = dataRow.GetDataValue("Suffix", false)
                };
            return ban;
        }
    }
}
