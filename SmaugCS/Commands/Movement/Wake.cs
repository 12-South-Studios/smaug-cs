using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement
{
    public static class Wake
    {
        public static void do_wake(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (string.IsNullOrEmpty(firstArg))
            {
                interp.interpret(ch, "stand");
                interp.interpret(ch, "look auto");
                return;
            }

            if (CheckFunctions.CheckIf(ch, args => !((CharacterInstance) args[0]).IsAwake(), "You are asleep yourself!",
                new List<object> {ch})) return;

            CharacterInstance victim = handler.get_char_room(ch, firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;

            if (victim.IsAwake())
            {
                comm.act(ATTypes.AT_PLAIN, "$N is already awake.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (victim.IsAffected(AffectedByTypes.Sleep) || (int) victim.CurrentPosition < (int) PositionTypes.Sleeping)
            {
                comm.act(ATTypes.AT_PLAIN, "You can't seem to wake $M!", ch, null, victim, ToTypes.Character);
                return;
            }

            comm.act(ATTypes.AT_ACTION, "You wake $M.", ch, null, victim, ToTypes.Character);
            victim.CurrentPosition = PositionTypes.Standing;
            comm.act(ATTypes.AT_ACTION, "$n wakes you.", ch, null, victim, ToTypes.Victim);
            interp.interpret(victim, "look auto");
        }
    }
}
