using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Enums;

namespace SmaugCS.Organizations
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
