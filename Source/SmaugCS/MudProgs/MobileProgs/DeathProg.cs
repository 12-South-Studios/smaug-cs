using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class DeathProg
    {
        public static bool Execute(object[] args)
        {
            var killer = (CharacterInstance)args[0];
            var mob = (MobileInstance)args[1];

            if (mob.IsNpc() && killer != mob &&
                mob.MobIndex.HasProg(MudProgTypes.Death))
            {
                mob.CurrentPosition = PositionTypes.Standing;
                CheckFunctions.CheckIfExecute(mob, MudProgTypes.Death);
                mob.CurrentPosition = PositionTypes.Dead;
            }

            fight.death_cry(mob);
            return true;
        }
    }
}
