using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class SpeechProg
    {
        public static void Execute(string txt, CharacterInstance actor)
        {
            foreach (CharacterInstance mob in actor.CurrentRoom.Persons)
            {
                if (mob.IsNpc() && mob.MobIndex.HasProg(MudProgTypes.Speech))
                {
                    if (actor.IsNpc() && actor.MobIndex == mob.MobIndex)
                        continue;

                    // TODO: Trigger on words or phrases?
                }
            }
        }
    }
}
