using System.Collections.Generic;
using System.Xml.Serialization;

namespace SmaugCS.Objects
{
    [XmlRoot("Area")]
    public class AreaData
    {
        public AreaData()
        {
            Rooms = new List<RoomTemplate>();    
        }

        [XmlArray("Rooms")]
        public List<RoomTemplate> Rooms { get; set; }

        [XmlElement]
        public string Name { get; set; }

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
        public int mkills { get; set; }
        public int mdeaths { get; set; }
        public int pkills { get; set; }
        public int pdeaths { get; set; }
        public int gold_looted { get; set; }
        public int illegal_pk { get; set; }
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
    }
}
