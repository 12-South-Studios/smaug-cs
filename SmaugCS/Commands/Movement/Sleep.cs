using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands.Movement
{
    public static class Sleep
    {
        public static void do_sleep(CharacterInstance ch, string argument)
        {
            if (ch.CurrentPosition == PositionTypes.Sleeping)
                color.send_to_char("You are already sleeping.", ch);
            else if (ch.CurrentPosition == PositionTypes.Resting)
                FromResting(ch);
            else if (ch.CurrentPosition == PositionTypes.Sitting)
                FromSitting(ch);
            else if (ch.CurrentPosition == PositionTypes.Standing)
                FromStanding(ch);
            else if (ch.IsInCombatPosition())
                color.send_to_char("You are busy fighting!", ch);
            else if (ch.CurrentPosition == PositionTypes.Mounted)
                color.send_to_char("You really should dismount first.", ch);

            mud_prog.rprog_sleep_trigger(ch);
        }

        private static bool CantSleepDueToMentalState(CharacterInstance ch, int modifier)
        {
            if (ch.MentalState > 30 && (SmaugRandom.D100() + modifier) < ch.MentalState)
            {
                color.send_to_char("You just can't seem to calm yourself down enough to sleep.", ch);
                comm.act(ATTypes.AT_PLAIN, "$n closes $s eyes for a few moments, but just can't seem to go to sleep.",
                    ch, null, null, ToTypes.Room);
                return true;
            }
            return false;
        }
        
        private static void FromStanding(CharacterInstance ch)
        {
            if (CantSleepDueToMentalState(ch, 0)) return;

            color.send_to_char("You close your eyes and drift into slumber.", ch);
            comm.act(ATTypes.AT_ACTION, "$n closes $s eyes and drifts into a deep slumber.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Sleeping;
        }

        private static void FromSitting(CharacterInstance ch)
        {
            if (CantSleepDueToMentalState(ch, 5)) return;

            color.send_to_char("You slump over and fall dead asleep.", ch);
            comm.act(ATTypes.AT_ACTION, "$n nods off and slowly slumps over, dead asleep.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Sleeping;
        }

        private static void FromResting(CharacterInstance ch)
        {
            if (CantSleepDueToMentalState(ch, 10)) return;

            color.send_to_char("You collapse into a deep sleep.", ch);
            comm.act(ATTypes.AT_ACTION, "$n collapses into a deep sleep.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Sleeping;
        }
    }
}
