
using System.Xml.Serialization;
using SmaugCS.Common;
using SmaugCS.Enums;


namespace SmaugCS.Objects
{
    [XmlRoot("Class")]
    public class ClassData
    {
        [XmlElement("ClassType")]
        public ClassTypes Type { get; set; }

        [XmlElement]
        public string Name { get; set; }

        public ExtendedBitvector AffectedBy { get; set; }

        [XmlElement]
        public int PrimaryAttribute { get; set; }

        [XmlElement]
        public int SecondaryAttribute { get; set; }

        [XmlElement]
        public int DeficientAttribute { get; set; }

        public int Resistance { get; set; }
        public int Susceptibility { get; set; }
        public int Weapon { get; set; }
        public int Guild { get; set; }
        public short SkillAdept { get; set; }
        public short ToHitArmorClass0 { get; set; }
        public short ToHitArmorClass32 { get; set; }
        public short MinimumHealth { get; set; }
        public short MaximumHealth { get; set; }
        public bool UseMana { get; set; }
        public short BaseExperience { get; set; }
    }
}
