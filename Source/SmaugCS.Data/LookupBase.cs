using Library.Common.Objects;
using System.Collections.Generic;

namespace SmaugCS.Data;

public abstract class LookupBase<T, TK>(TK defaultFunc)
  where T : Entity
  where TK : class, new()
{
  protected Dictionary<string, TK> LookupTable { get; private set; } = new();

  protected TK GetFunction(string name)
  {
    return LookupTable.ContainsKey(name.ToLower())
      ? LookupTable[name.ToLower()]
      : defaultFunc;
  }

  public abstract void UpdateFunctionReferences(IEnumerable<T> values);

  public virtual TK Get(string name)
  {
    if (LookupTable.TryGetValue(name, out TK value))
      return value;
    throw new KeyNotFoundException($"invalid entity '{name}'");
  }
}