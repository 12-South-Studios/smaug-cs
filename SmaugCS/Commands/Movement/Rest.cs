using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement
{
    public static class Rest
    {
        public static void do_rest(CharacterInstance ch, string argument)
        {
            if (ch.CurrentPosition == PositionTypes.Sleeping)
                FromSleeping(ch);
            else if (ch.CurrentPosition == PositionTypes.Resting)
                color.send_to_char("You are already resting.", ch);
            else if (ch.CurrentPosition == PositionTypes.Standing)
                FromStanding(ch);
            else if (ch.CurrentPosition == PositionTypes.Sitting)
                FromSitting(ch);
            else if (ch.IsInCombatPosition())
                color.send_to_char("You are busy fighting!", ch);
            else if (ch.CurrentPosition == PositionTypes.Mounted)
                color.send_to_char("You'd better dismount first.", ch);

            mud_prog.rprog_rest_trigger(ch);
        }

        private static void FromSitting(CharacterInstance ch)
        {
            color.send_to_char("You lie back and sprawl out to rest.", ch);
            comm.act(ATTypes.AT_ACTION, "$n lies back and sprawls out to rest.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Resting;
        }

        private static void FromStanding(CharacterInstance ch)
        {
            color.send_to_char("You sprawl out haphazardly.", ch);
            comm.act(ATTypes.AT_ACTION, "$n sprawls out haphazardly.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Resting;
        }

        private static void FromSleeping(CharacterInstance ch)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.IsAffected(AffectedByTypes.Sleep), "You can't seem to wake up!"))
                return;

            color.send_to_char("You rouse from your slumber.", ch);
            comm.act(ATTypes.AT_ACTION, "$n rouses from $s slumber.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Resting;
        }
    }
}
