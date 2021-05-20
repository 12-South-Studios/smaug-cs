using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement
{
    public static class Pull
    {
        public static void do_pull(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Pull what?")) return;

            if (handler.FindObject_CheckMentalState(ch)) return;

            var obj = ch.GetObjectOnMeOrInRoom(firstArg);
            if (obj == null)
            {
                comm.act(ATTypes.AT_PLAIN, "I see no $T here.", ch, null, null, ToTypes.Character);
                return;
            }

            PullOrPush.pullorpush(ch, obj, true);
        }
    }
}
