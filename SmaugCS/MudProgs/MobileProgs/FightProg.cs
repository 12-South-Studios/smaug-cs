using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class FightProg
    {
        public static void Execute(MobileInstance mob, CharacterInstance ch)
        {
            if (mob.IsNpc() && mob.MobIndex.HasProg(MudProgTypes.Fight))
                mud_prog.CheckIfExecute(mob, MudProgTypes.Fight);
        }
    }
}
