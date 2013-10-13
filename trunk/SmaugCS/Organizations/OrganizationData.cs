using System.Xml.Serialization;
using SmaugCS.Interfaces;

namespace SmaugCS.Organizations
{
    public abstract class OrganizationData : IPersistable
    {
        [XmlIgnore]
        public string Filename { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlElement]
        public string Leader { get; set; }

        [XmlElement]
        public int Board { get; set; }

        public abstract void Save();
        public abstract void Load();
    }
}
