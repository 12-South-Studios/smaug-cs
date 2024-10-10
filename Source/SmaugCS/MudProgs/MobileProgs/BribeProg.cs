using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.MudProgs.MobileProgs;

public static class BribeProg
{
  public static bool Execute(object[] args)
  {
    CharacterInstance actor = args.GetValue<CharacterInstance>(0);
    MobileInstance mob = args.GetValue<MobileInstance>(1);
    int amount = args.GetValue<int>(2);

    if (!mob.IsNpc() || !mob.CanSee(actor) || !mob.MobIndex.HasProg(MudProgTypes.Bribe)) return true;
    return !actor.IsNpc() || actor.Parent != mob.MobIndex;

    // todo finish this mud_prog.c:2743
  }
}