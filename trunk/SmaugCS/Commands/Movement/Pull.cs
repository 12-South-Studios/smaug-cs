using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement
{
    public static class Pull
    {
        public static void do_pull(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Pull what?")) return;

            if (handler.ms_find_obj(ch)) return;

            ObjectInstance obj = handler.get_obj_here(ch, firstArg);
            if (obj == null)
            {
                comm.act(ATTypes.AT_PLAIN, "I see no $T here.", ch, null, obj, ToTypes.Character);
                return;
            }

            PullOrPush.pullorpush(ch, obj, true);
        }
    }
}
