using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Movement
{
    public static class BashDoor
    {
        public static void do_bashdoor(CharacterInstance ch, string argument)
        {
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>("bashdoor");
            if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && ch.Level < skill.SkillLevels.ToList()[(int) ch.CurrentClass],
                "You're not enough of a warrior to bash doors!")) return;

            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Bash what?")) return;
            if (CheckFunctions.CheckIfNotNullObject(ch, ch.CurrentFighting, "You can't break off your fight.")) return;

            ExitData exit = act_move.find_door(ch, firstArg, false);
            if (exit == null)
                Bash(ch, skill, "wall");
            else
                BashSomething(ch, exit, skill, firstArg);
        }

        private static void Bash(CharacterInstance actor, SkillData skill, string arg)
        {
            comm.act(ATTypes.AT_SKILL, "WHAAAAM!!! You bash against the $d, but it doesn't budge.", actor, null, arg,
                ToTypes.Character);
            comm.act(ATTypes.AT_SKILL, "WHAAAAM!!! $n bashes against the $d, but it holds strong.", actor, null, arg,
                ToTypes.Room);

            int damage = (actor.MaximumHealth/20) + 10;
            fight.damage(actor, actor, damage, (int)skill.ID);
            skill.LearnFromFailure(actor);
        }

        private static void BashSomething(CharacterInstance actor, ExitData exit, SkillData skill, string arg)
        {
            if (CheckFunctions.CheckIfSet(actor, exit.Flags, ExitFlags.Closed, "Calm down. It is already open."))
                return;

            Macros.WAIT_STATE(actor, skill.Rounds);

            string keyword = exit.Flags.IsSet(ExitFlags.Secret) ? "wall" : exit.Keywords;

            int chance = !actor.IsNpc()
                ? Macros.LEARNED(actor, (int) skill.ID)/2
                : 90;

            if (exit.Flags.IsSet(ExitFlags.Locked))
                chance /= 3;

            if (exit.Flags.IsSet(ExitFlags.BashProof)
                || actor.CurrentMovement < 15
                || SmaugRandom.Percent() >= (chance + 4*(actor.GetCurrentStrength() - 19)))
            {
                Bash(actor, skill, arg);
                return;
            }

            BashExit(exit);

            comm.act(ATTypes.AT_SKILL, "Crash! You bashed open the $d!", actor, null, keyword, ToTypes.Character);
            comm.act(ATTypes.AT_SKILL, "$n bashes open the $d!", actor, null, keyword, ToTypes.Room);
            skill.LearnFromSuccess(actor);

            ExitData reverseExit = exit.GetReverseExit();
            BashExit(reverseExit);

            RoomTemplate destination = exit.GetDestination(DatabaseManager.Instance);
            foreach(CharacterInstance ch in destination.Persons)
                comm.act(ATTypes.AT_SKILL, "The $d crashes open!", ch, null, reverseExit.Keywords, ToTypes.Character);
            
            fight.damage(actor, actor, (actor.CurrentHealth/20), (int) skill.ID);
        }
       
        private static void BashExit(ExitData exit)
        {
            exit.Flags.RemoveBit(ExitFlags.Closed);
            if (exit.Flags.IsSet(ExitFlags.Locked))
                exit.Flags.RemoveBit(ExitFlags.Locked);
            exit.Flags.SetBit(ExitFlags.Bashed);
        }
    }
}
