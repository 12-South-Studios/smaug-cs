using System;
using System.Data;
using Realm.Library.Common;

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
                          EnumerationExtensions.GetEnumByName<BanTypes>(dataRow["BanType"].ToString()));
            ban.Name = dataRow["Name"].ToString();
            ban.Note = dataRow["Note"].IsNullOrDBNull() ? string.Empty : dataRow["Note"].ToString();
            ban.BannedBy = dataRow["BannedBy"].ToString();
            ban.BannedOn = Convert.ToDateTime(dataRow["BannedOn"]);
            ban.Duration = Convert.ToInt32(dataRow["Duration"]);
            ban.Level = dataRow["Level"].IsNullOrDBNull() ? 0 : Convert.ToInt32(dataRow["Level"]);
            ban.Warn = !dataRow["Warn"].IsNullOrDBNull() && Convert.ToBoolean(dataRow["Warn"]);
            ban.Prefix = !dataRow["Prefix"].IsNullOrDBNull() && Convert.ToBoolean(dataRow["Prefix"]);
            ban.Suffix = !dataRow["Suffix"].IsNullOrDBNull() && Convert.ToBoolean(dataRow["Suffix"]);

            return ban;
        }
    }
}
