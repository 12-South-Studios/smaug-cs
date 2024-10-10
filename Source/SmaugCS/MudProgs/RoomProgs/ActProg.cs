using SmaugCS.Common;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;

namespace SmaugCS.MudProgs.RoomProgs;

public static class ActProg
{
  public static bool Execute(object[] args)
  {
    string buffer = args.GetValue<string>(0);
    RoomTemplate room = args.GetValue<RoomTemplate>(1);
    MobileInstance mob = args.GetValue<MobileInstance>(2);
    ObjectInstance obj = args.GetValue<ObjectInstance>(3);
    // var vo = args[4]

    return false;
  }
}