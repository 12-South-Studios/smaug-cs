using System;
using System.Collections.Generic;
using System.Data;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;

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
                          Realm.Library.Common.EnumerationExtensions.GetEnumByName<BanTypes>(dataRow["BanTypeName"].ToString()))
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

        public static DataTable GetDataTable(IEnumerable<BanData> list)
        {
            var dt = BuildDataTable();

            foreach (BanData ban in list)
            {
                DataRow dr = dt.NewRow();
                dr["BanId"] = ban.Id;
                dr["BanType"] = (int)ban.Type;
                dr["Name"] = ban.Name;
                dr["Note"] = ban.Note;
                dr["BannedBy"] = ban.BannedBy;
                dr["BannedOn"] = ban.BannedOn;
                dr["Duration"] = ban.Duration;
                dr["Level"] = ban.Level;
                dr["Warn"] = ban.Warn;
                dr["Prefix"] = ban.Prefix;
                dr["Suffix"] = ban.Suffix;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable BuildDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BanId", typeof (int));
            dt.Columns.Add("BanType", typeof (int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Note", typeof(string));
            dt.Columns.Add("BannedBy", typeof (string));
            dt.Columns.Add("BannedOn", typeof (DateTime));
            dt.Columns.Add("Duration", typeof (int));
            dt.Columns.Add("Level", typeof (int));
            dt.Columns.Add("Warn", typeof (bool));
            dt.Columns.Add("Prefix", typeof (bool));
            dt.Columns.Add("Suffix", typeof (bool));
            return dt;
        }
    }
}
