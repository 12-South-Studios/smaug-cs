using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Templates;

namespace SmaugCS.Data
{
    [XmlRoot("Area")]
    public class AreaData : Entity
    {
        public AreaData(long id, string name)
            : base(id, name)
        {
            Rooms = new List<RoomTemplate>();
        }

        [XmlArray("Rooms")]
        public List<RoomTemplate> Rooms { get; set; }

        [XmlIgnore]
        public string Filename { get; set; }

        [XmlElement]
        public string Author { get; set; }

        [XmlElement]
        public string ResetMessage { get; set; }

        [XmlElement]
        public string Credits { get; set; }

        [XmlElement]
        public int Flags { get; set; }

        public int LowRoomNumber { get; set; }
        public int HighRoomNumber { get; set; }
        public int LowObjectNumber { get; set; }
        public int HighObjectNumber { get; set; }
        public int LowMobNumber { get; set; }
        public int HighMobNumber { get; set; }
        public int LowSoftRange { get; set; }
        public int HighSoftRange { get; set; }
        public int LowHardRange { get; set; }
        public int HighHardRange { get; set; }
        public int SpellLimit { get; set; }
        public int curr_spell_count { get; set; }
        public int PvEKills { get; set; }
        public int PvEDeaths { get; set; }
        public int PvPKills { get; set; }
        public int PvPDeaths { get; set; }
        public int gold_looted { get; set; }
        public int IllegalPvPKill { get; set; }
        public int HighEconomy { get; set; }
        public int LowEconomy { get; set; }
        public int status { get; set; }
        public int Age { get; set; }

        [XmlIgnore]
        public int NumberOfPlayers { get; set; }

        [XmlElement]
        public int ResetFrequency { get; set; }

        [XmlElement]
        public int MaximumPlayers { get; set; }

        [XmlElement]
        public int Version { get; set; }

        [XmlElement]
        public int WeatherX { get; set; }

        [XmlElement]
        public int WeatherY { get; set; }

        public void BoostEconomy(int gold)
        {
            while (gold >= 1000000000)
            {
                ++HighEconomy;
                gold -= 1000000000;
            }
            LowEconomy += gold;
            while (LowEconomy >= 1000000000)
            {
                ++HighEconomy;
                LowEconomy -= 1000000000;
            }
        }

        public void LowerEconomy(int gold)
        {
            while (gold >= 1000000000)
            {
                --HighEconomy;
                gold -= 1000000000;
            }
            LowEconomy -= gold;
            while (LowEconomy < 0)
            {
                --HighEconomy;
                LowEconomy += 1000000000;
            }
        }

        public bool EconomyHas(int gold)
        {
            return (((HighEconomy > 0) ? 1 : 0) * 1000000000 + LowEconomy) >= gold;
        }

        public void AddRoom(RoomTemplate room)
        {
            if (Rooms.All(x => x.ID != room.ID))
                Rooms.Add(room);
        }

        public void SetFlags(string flags)
        {
            string[] words = flags.Split(new[] { ' ' });
            foreach (string word in words)
            {
                Flags += (int)EnumerationExtensions.GetEnumIgnoreCase<AreaFlags>(word);
            }
        }

        /*public void SaveHeader(TextWriterProxy proxy, bool install)
        {
            if (install)
                Flags.RemoveBit((int)AreaFlags.Prototype);

            proxy.Write("#AREADATA\n");
            proxy.Write("Version      {0}\n", Version);
            proxy.Write("Name         {0}~\n", Name);
            proxy.Write("Author       {0}~\n", Author);
            proxy.Write("WeatherX     {0}\n", WeatherX);
            proxy.Write("WeatherY     {0}\n", WeatherY);
            if (!Credits.IsNullOrEmpty())
                proxy.Write("Credits      {0}~\n", Credits);
            proxy.Write("Ranges       {0} {1} {2} {3}\n", LowSoftRange, HighSoftRange, LowHardRange, HighHardRange);
            if (SpellLimit > 0)
                proxy.Write("SpellLimit   {0}\n", SpellLimit);
            if (HighEconomy > 0 || LowEconomy > 0)
                proxy.Write("Economy      {0} {1}\n", HighEconomy, LowEconomy);
            if (!ResetMessage.IsNullOrEmpty())
                proxy.Write("ResetMsg     {0}~\n", ResetMessage);
            if (ResetFrequency > 0)
                proxy.Write("ResetFreq    {0}\n", ResetFrequency);
            if (Flags > 0)
                proxy.Write("Flags        {0}~\n", Flags.GetFlagString(BuilderConstants.area_flags));
            proxy.Write("#ENDAREADATA\n\n");
        }*/



        public void FireStartupResets()
        {
            foreach (RoomTemplate room in Rooms)
                room.ProcessResets();
        }
    }
}
