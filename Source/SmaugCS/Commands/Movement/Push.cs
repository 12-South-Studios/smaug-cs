﻿using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Push
    {
        public static void do_push(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Push what?")) return;

            if (handler.FindObject_CheckMentalState(ch)) return;

            var obj = ch.GetObjectOnMeOrInRoom(firstArg);
            if (obj == null)
            {
                comm.act(ATTypes.AT_PLAIN, "I see no $T here.", ch, null, null, ToTypes.Character);
                return;
            }

            PullOrPush.pullorpush(ch, obj, false);
        }
    }
}
