using SmaugCS.Constants.Enums;
using System.Collections.Generic;

namespace SmaugCS.Data;

public class ResetData
{
  public ResetTypes Type { get; set; }
  public string Command { get; set; }
  public int Extra { get; set; }
  public IEnumerable<int> Args { get; private set; } = new List<int>();
  public bool sreset { get; set; }
  public ICollection<ResetData> Resets { get; private set; } = new List<ResetData>();

  public void SetArgs(int v1, int v2, int v3)
  {
    Args = new List<int> { v1, v2, v3 };
  }

  public void AddReset(string type, int extra, int arg1, int arg2, int arg3)
  {
    ResetData newReset = new()
    {
      Type = Library.Common.Extensions.EnumerationExtensions.GetEnumIgnoreCase<ResetTypes>(type),
      Extra = extra
    };
    newReset.SetArgs(arg1, arg2, arg3);
    Resets.Add(newReset);
  }
}