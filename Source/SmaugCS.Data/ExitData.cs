using Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using System.Linq;
using System.Xml.Serialization;

namespace SmaugCS.Data;

[XmlRoot("Exit")]
public class ExitData(long id, string name) : Entity(id, name)
{
  [XmlElement("Reverse")] public long Reverse { get; set; }

  [XmlElement("Destination")] public long Destination { get; set; }

  [XmlElement] public string Keywords { get; set; }

  [XmlElement] public string Description { get; set; }

  [XmlElement("ID")] public long vnum => Id;

  [XmlElement("RoomID")] public long Room_vnum { get; set; }

  public int Flags { get; set; }
  public int Key { get; set; }

  [XmlElement("Direction")] public DirectionTypes Direction { get; set; }

  public int Distance { get; set; }
  public int Pull { get; set; }
  public DirectionPullTypes PullType { get; set; }

  public void SetFlags(string flags)
  {
    string[] words = flags.Split(' ');
    foreach (int flagValue in words
               .Select(Library.Common.Extensions.EnumerationExtensions.GetEnumIgnoreCase<ExitFlags>)
               .Select(flag => (int)flag))
    {
      Flags = Flags.SetBit(flagValue);
    }
  }

  public override bool Equals(object obj)
  {
    if (obj == null) return false;
    if (obj.GetType() != GetType()) return false;

    ExitData objToCheck = (ExitData)obj;
    return objToCheck.Id == Id && objToCheck.Name.Equals(Name);
  }

  public static bool operator ==(ExitData a, ExitData b)
  {
    if (ReferenceEquals(a, b)) return true;
    if (a is null || b is null) return false;
    return a.Id == b.Id && a.Name.Equals(b.Name);
  }

  public static bool operator !=(ExitData a, ExitData b)
  {
    return !(a == b);
  }
}