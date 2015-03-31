using System;
using System.Xml.Serialization;

namespace SmaugCS.Data.Organizations
{
    [XmlRoot("Member")]
    public class RosterData
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public DateTime Joined { get; set; }

        [XmlElement]
        public int Class { get; set; }

        [XmlElement]
        public int Level { get; set; }

        [XmlElement]
        public int Kills { get; set; }

        [XmlElement]
        public int Deaths { get; set; }
    }
}
