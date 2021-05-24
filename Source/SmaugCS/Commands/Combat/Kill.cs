using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Kill
    {
        public static void do_kill(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfEmptyString(ch, argument, "Kill whom?")) return;

            var firstArg = argument.FirstWord();
            var victim = ch.GetCharacterInRoom(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;
            if (CheckFunctions.CheckIfTrue(ch, victim.IsNpc() && victim.CurrentMorph != null,
                "This creature appears strange to you. Look upon it more closely before attempting to kill it."))
                return;
            if (CheckFunctions.CheckIfTrue(ch, !victim.IsNpc() && !victim.Act.IsSet((int)PlayerFlags.Killer)
                && !victim.Act.IsSet((int)PlayerFlags.Thief), "You must MURDER a player.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, ch, victim, "You hit yourself.  Ouch!"))
            {
                fight.multi_hit(ch, ch, Program.TYPE_UNDEFINED);
                return;
            }
            if (fight.is_safe(ch, victim, true)) return;

            if (ch.IsAffected(AffectedByTypes.Charm) && ch.Master == victim)
            {
                comm.act(ATTypes.AT_PLAIN, "$N is your beloved master.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, ch.IsInCombatPosition(), "You do the best you can!")) return;

            Macros.WAIT_STATE(ch, 1 * GameConstants.GetSystemValue<int>("PulseViolence"));
            ch.CheckAttackForAttackerFlag(victim);
            fight.multi_hit(ch, victim, Program.TYPE_UNDEFINED);
        }
    }
}
