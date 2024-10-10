using Library.Common.Objects;
using SmaugCS.Constants.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Data.Templates;

public abstract class Template(long id, string name) : Entity(id, name), IHasMudProgs
{
  public string Description { get; set; }

  private readonly List<MudProgData> _mudProgs = [];
  public IEnumerable<MudProgData> MudProgs => _mudProgs;

  #region Implementation of IHasMudProgs

  public bool HasProg(int prog) => MudProgs.Any(x => (int)x.Type == prog);

  public bool HasProg(MudProgTypes type) => HasProg((int)type);

  public void AddMudProg(MudProgData mprog)
  {
    if (!MudProgs.Contains(mprog))
      _mudProgs.Add(mprog);
  }

  #endregion

  public override bool Equals(object obj)
  {
    if (obj == null) return false;
    if (obj.GetType() != GetType()) return false;

    Template objToCheck = (Template)obj;
    return objToCheck.Id == Id && objToCheck.Name.Equals(Name);
  }

  public override int GetHashCode()
  {
    int hash = 13;
    hash = hash * 7 + Id.GetHashCode();
    hash = hash * 7 + Name.GetHashCode();
    return hash;
  }

  public static bool operator ==(Template a, Template b)
  {
    if (ReferenceEquals(a, b)) return true;
    if ((object)a == null || (object)b == null) return false;
    return a.Id == b.Id && a.Name.Equals(b.Name);
  }

  public static bool operator !=(Template a, Template b) => !(a == b);
}