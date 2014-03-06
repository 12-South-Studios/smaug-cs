using System.Xml.Serialization;
using Realm.Library.Common;

namespace SmaugCS.Data.Organizations
{
    public abstract class OrganizationData : Entity
    {
        protected OrganizationData(long id, string name) : base(id, name)
        {
        }

        [XmlIgnore]
        public string Filename { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlElement]
        public string Leader { get; set; }

        [XmlElement]
        public int Board { get; set; }

        // public abstract void Save();
        // public abstract void Load();
    }
}
