using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands.Movement
{
    public static class Stand
    {
        public static void do_stand(CharacterInstance ch, string argument)
        {
            if (ch.CurrentPosition == PositionTypes.Sleeping)
                FromSleeping(ch);
            else if (ch.CurrentPosition == PositionTypes.Resting)
                FromResting(ch);
            else if (ch.CurrentPosition == PositionTypes.Sitting)
                FromSitting(ch);
            else if (ch.CurrentPosition == PositionTypes.Standing)
                color.send_to_char("You are already standing.", ch);
            else if (ch.IsInCombatPosition())
                color.send_to_char("You are already fighting!", ch);
        }

        private static void FromSitting(CharacterInstance ch)
        {
            color.send_to_char("You move quickly to your feet.", ch);
            comm.act(ATTypes.AT_ACTION, "$n rises up.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Standing;
        }

        private static void FromResting(CharacterInstance ch)
        {
            color.send_to_char("You gather yourself and stand up.", ch);
            comm.act(ATTypes.AT_ACTION, "$n rises from $s rest.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Standing;
        }

        private static void FromSleeping(CharacterInstance ch)
        {
            if (ch.IsAffected(AffectedByTypes.Sleep))
            {
                color.send_to_char("You can't seem to wake up!", ch);
                return;
            }

            color.send_to_char("You wake and climb quickly to your feet.", ch);
            comm.act(ATTypes.AT_ACTION, "$n arises from $s slumber.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Standing;
        }
    }
}
