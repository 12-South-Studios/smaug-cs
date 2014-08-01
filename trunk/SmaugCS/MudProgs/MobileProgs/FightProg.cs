using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class FightProg
    {
        public static void Execute(CharacterInstance mob, CharacterInstance ch)
        {
            if (mob.IsNpc() && mob.MobIndex.HasProg(MudProgTypes.Fight))
                mud_prog.CheckIfExecute(mob, MudProgTypes.Fight);
        }
    }
}
