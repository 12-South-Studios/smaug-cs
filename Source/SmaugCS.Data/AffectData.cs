using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using System.Xml.Serialization;

namespace SmaugCS.Data;

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
    public ExtendedBitvector BitVector { get; set; } = new();

    [XmlElement]
    public int SkillNumber { get; set; }
}