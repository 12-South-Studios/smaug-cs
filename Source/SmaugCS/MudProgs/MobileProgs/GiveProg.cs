using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.MobileProgs;

public static class GiveProg
{
  public static bool Execute(object[] args)
  {
    MobileInstance mob = (MobileInstance)args[0];
    CharacterInstance ch = (CharacterInstance)args[1];
    ObjectInstance obj = (ObjectInstance)args[2];

    return false;
  }
}