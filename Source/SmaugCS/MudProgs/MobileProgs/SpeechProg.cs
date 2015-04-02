using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.MudProgs.MobileProgs
{
    public static class SpeechProg
    {
        public static void Execute(string txt, CharacterInstance actor)
        {
            foreach (var mob in actor.CurrentRoom.Persons.OfType<MobileInstance>().Where(x => x.IsNpc()))
            {
                if (mob.MobIndex.HasProg(MudProgTypes.Speech))
                {
                    if (actor.IsNpc() && ((MobileInstance)actor).MobIndex == mob.MobIndex)
                        continue;

                    // TODO: Trigger on words or phrases?
                }
            }
        }
    }
}
