using SmaugCS.Common;
using SmaugCS.Data.Instances;

namespace SmaugCS.SpecFuns.Breaths;

public static class BreathAcid
{
  public static bool Execute(MobileInstance ch, IManager dbManager)
  {
    return Dragon.Execute(ch, "acid breath", dbManager);
  }
}