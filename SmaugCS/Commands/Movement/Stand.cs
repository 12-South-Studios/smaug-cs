using System.Diagnostics.CodeAnalysis;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement
{
    public static class Stand
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "argument")]
        public static void do_stand(CharacterInstance ch, string argument)
        {
            if (ch.CurrentPosition == PositionTypes.Sleeping)
                FromSleeping(ch);
            else if (ch.CurrentPosition == PositionTypes.Resting)
                FromResting(ch);
            else if (ch.CurrentPosition == PositionTypes.Sitting)
                FromSitting(ch);
            else if (ch.CurrentPosition == PositionTypes.Standing)
                ch.SendTo("You are already standing.");
            else if (ch.IsInCombatPosition())
                ch.SendTo("You are already fighting!");
        }

        private static void FromSitting(CharacterInstance ch)
        {
            ch.SendTo("You move quickly to your feet.");
            comm.act(ATTypes.AT_ACTION, "$n rises up.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Standing;
        }

        private static void FromResting(CharacterInstance ch)
        {
            ch.SendTo("You gather yourself and stand up.");
            comm.act(ATTypes.AT_ACTION, "$n rises from $s rest.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Standing;
        }

        private static void FromSleeping(CharacterInstance ch)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.IsAffected(AffectedByTypes.Sleep), "You can't seem to wake up!"))
                return;

            ch.SendTo("You wake and climb quickly to your feet.");
            comm.act(ATTypes.AT_ACTION, "$n arises from $s slumber.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Standing;
        }
    }
}
