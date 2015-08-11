using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class FightProg
    {
        public static bool Execute(object[] args)
        {
            var mob = (MobileInstance)args[0];
            var ch = (CharacterInstance) args[1];

            if (mob.IsNpc() && mob.MobIndex.HasProg(MudProgTypes.Fight))
                CheckFunctions.CheckIfExecute(mob, MudProgTypes.Fight);

            return true;
        }
    }
}
