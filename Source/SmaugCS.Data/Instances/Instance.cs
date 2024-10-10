using Library.Common.Objects;
using SmaugCS.Data.Templates;
using System.Collections.Generic;

namespace SmaugCS.Data.Instances;

public abstract class Instance(long id, string name) : Entity(id, name)
{
  public string ShortDescription { get; set; }
  public string Description { get; set; }
  public Template Parent { get; set; }
  public ICollection<AffectData> Affects { get; private set; } = new List<AffectData>();
  public int Level { get; set; }
  public int Timer { get; set; }

  public override bool Equals(object obj)
  {
    if (obj == null) return false;
    if (obj.GetType() != GetType()) return false;

    Instance objToCheck = (Instance)obj;
    return objToCheck.Id == Id && objToCheck.Name.Equals(Name);
  }

  public static bool operator ==(Instance a, Instance b)
  {
    if (ReferenceEquals(a, b)) return true;
    if (a is null || b is null) return false;
    return a.Id == b.Id && a.Name.Equals(b.Name);
  }

  public static bool operator !=(Instance a, Instance b) => !(a == b);
}