using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Wake
    {
        public static void do_wake(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (string.IsNullOrEmpty(firstArg))
            {
                interp.interpret(ch, "stand");
                interp.interpret(ch, "look auto");
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, !ch.IsAwake(), "You are asleep yourself!")) return;

            var victim = ch.GetCharacterInRoom(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;

            if (victim.IsAwake())
            {
                comm.act(ATTypes.AT_PLAIN, "$N is already awake.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (victim.IsAffected(AffectedByTypes.Sleep) || (int)victim.CurrentPosition < (int)PositionTypes.Sleeping)
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
