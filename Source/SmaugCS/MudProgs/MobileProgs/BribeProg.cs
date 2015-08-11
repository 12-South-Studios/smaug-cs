using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class BribeProg
    {
        public static bool Execute(object[] args)
        {
            var actor = args.GetValue<CharacterInstance>(0);
            var mob = args.GetValue<MobileInstance>(1);
            var amount = args.GetValue<int>(2);

            if (mob.IsNpc() && mob.CanSee(actor) && mob.MobIndex.HasProg(MudProgTypes.Bribe))
            {
                if (actor.IsNpc() && actor.Parent == mob.MobIndex) return false;

               // todo finish this mud_prog.c:2743
            }

            return true;
        }
    }
}
