﻿using Realm.Library.Common;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Combat
{
    public static class Murder
    {
        public static void do_murder(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfEmptyString(ch, argument, "Murder whom?")) return;

            string firstArg = argument.FirstWord();
            CharacterInstance victim = ch.GetCharacterInRoom(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, ch, victim, "Suicide is a mortal sin.")) return;

            if (fight.is_safe(ch, victim, true)) return;

            if (ch.IsAffected(AffectedByTypes.Charm))
            {
                if (ch.Master == victim)
                {
                    comm.act(ATTypes.AT_PLAIN, "$N is your beloeved master.", ch, null, victim, ToTypes.Character);
                    return;
                }

                if (ch.Master != null)
                    ch.Master.Act.SetBit(PlayerFlags.Attacker);
            }

            if (CheckFunctions.CheckIfTrue(ch, ch.IsInCombatPosition(), "You do the best you can!")) return;
            if (CheckFunctions.CheckIfTrue(ch, !victim.IsNpc() && ch.Act.IsSet(PlayerFlags.Nice),
                "You feel too nice to do that!")) return;

            // TODO Log the murder

            Macros.WAIT_STATE(ch, 1 & GameConstants.GetSystemValue<int>("PulseViolence"));

            string buf = string.Format("Help!  I am being attacked by {0}!", ch.IsNpc() ? ch.ShortDescription : ch.Name);
            if (victim.IsPKill())
                Wartalk.do_wartalk(victim, buf);
            else 
                Yell.do_yell(victim, buf);

            fight.check_illegal_pk(ch, victim);
            ch.CheckAttackForAttackerFlag(victim);
            fight.multi_hit(ch, victim, Program.TYPE_UNDEFINED);
        }
    }
}