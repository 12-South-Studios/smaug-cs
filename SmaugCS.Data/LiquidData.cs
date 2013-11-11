using System.Collections.Generic;
using System.Xml.Serialization;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    [XmlRoot("Liquid")]
    public class LiquidData
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string ShortDescription { get; set; }

        [XmlElement]
        public string Color { get; set; }

        [XmlElement("ID")]
        public int Vnum { get; set; }

        [XmlElement("LiquidType")]
        public LiquidTypes Type { get; set; }

        public Dictionary<ConditionTypes, int> Mods { get; set; }

        public LiquidData()
        {
            Mods = new Dictionary<ConditionTypes, int>();
        }
    }
}
