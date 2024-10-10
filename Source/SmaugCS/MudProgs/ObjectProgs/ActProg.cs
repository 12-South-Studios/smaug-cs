using SmaugCS.Common;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.ObjectProgs;

public static class ActProg
{
  public static bool Execute(object[] args)
  {
    string buffer = args.GetValue<string>(0);
    ObjectInstance actObj = args.GetValue<ObjectInstance>(1);
    MobileInstance mob = args.GetValue<MobileInstance>(2);
    ObjectInstance obj = args.GetValue<ObjectInstance>(3);
    // var vo = args[4]

    return false;
  }
}