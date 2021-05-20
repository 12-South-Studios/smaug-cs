using System.Xml.Serialization;
using Realm.Library.Common;
using Realm.Library.Common.Objects;

namespace SmaugCS.Data.Organizations
{
    public abstract class OrganizationData : Entity
    {
        protected OrganizationData(long id, string name) : base(id, name)
        {
        }

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
