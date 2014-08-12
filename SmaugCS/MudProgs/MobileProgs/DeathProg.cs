using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class DeathProg
    {
        public static void Execute(CharacterInstance killer, CharacterInstance mob)
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
