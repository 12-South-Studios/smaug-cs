using Library.Common.Objects;
using System.Xml.Serialization;

namespace SmaugCS.Data.Organizations;

public abstract class OrganizationData(long id, string name) : Entity(id, name)
{
    [XmlElement]
    public string Description { get; set; }

    [XmlElement]
    public string Leader { get; set; }

    [XmlElement]
    public int Board { get; set; }
}