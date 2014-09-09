using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class DeathProg
    {
        public static void Execute(CharacterInstance killer, MobileInstance mob)
        {
            if (mob.IsNpc() && killer != mob &&
                mob.MobIndex.HasProg(MudProgTypes.Death))
            {
                mob.CurrentPosition = PositionTypes.Standing;
                mud_prog.CheckIfExecute(mob, MudProgTypes.Death);
                mob.CurrentPosition = PositionTypes.Dead;
            }

            fight.death_cry(mob);
        }
    }
}
