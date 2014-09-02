using System.Xml.Serialization;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    [XmlRoot("Affect")]
    public class AffectData
    {
        [XmlElement("AffectedBy")]
        public AffectedByTypes Type { get; set; }

        [XmlElement]
        public int Duration { get; set; }

        [XmlElement]
        public ApplyTypes Location { get; set; }

        [XmlElement]
        public int Modifier { get; set; }

        [XmlElement]
        public ExtendedBitvector BitVector { get; set; }

        [XmlElement]
        public int SkillNumber { get; set; }
    }
}
