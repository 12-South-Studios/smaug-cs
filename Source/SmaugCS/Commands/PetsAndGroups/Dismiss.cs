﻿using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Dismiss
    {
        public static void do_dismiss(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Dismiss whom?")) return;

            var victim = ch.GetCharacterInRoom(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;

            if (victim.IsAffected(AffectedByTypes.Charm)
                && victim.IsNpc() && victim.Master == ch)
            {
                victim.StopFollower();
                ((MobileInstance)victim).StopHating();
                ((MobileInstance)victim).StopHunting();
                ((MobileInstance)victim).StopFearing();
                comm.act(ATTypes.AT_ACTION, "$n dismisses $N.", ch, null, victim, ToTypes.NotVictim);
                comm.act(ATTypes.AT_ACTION, "You dismiss $N.", ch, null, victim, ToTypes.Character);
                return;
            }

            ch.SendTo("You cannot dismiss them.");
        }
    }
}
