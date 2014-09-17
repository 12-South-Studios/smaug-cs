using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement
{
    public static class Sit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "argument")]
        public static void do_sit(CharacterInstance ch, string argument)
        {
            if (ch.CurrentPosition == PositionTypes.Sleeping)
                FromSleeping(ch);
            else if (ch.CurrentPosition == PositionTypes.Resting)
                FromResting(ch);
            else if (ch.CurrentPosition == PositionTypes.Standing)
                FromStanding(ch);
            else if (ch.CurrentPosition == PositionTypes.Sitting)
                color.send_to_char("You are already sitting.", ch);
            else if (ch.IsInCombatPosition())
                color.send_to_char("You are busy fighting!", ch);
            else if (ch.CurrentPosition == PositionTypes.Mounted)
                color.send_to_char("You are already sitting - on your mount.", ch);
        }

        private static void FromStanding(CharacterInstance ch)
        {
            color.send_to_char("You sit down.", ch);
            comm.act(ATTypes.AT_ACTION, "$n sits down.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Sitting;
        }

        private static void FromResting(CharacterInstance ch)
        {
            color.send_to_char("You stop resting and sit up.", ch);
            comm.act(ATTypes.AT_ACTION, "$n stops resting and sits up.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Sitting;
        }

        private static void FromSleeping(CharacterInstance ch)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.IsAffected(AffectedByTypes.Sleep), "You can't seem to wake up!"))
                return;

            color.send_to_char("You wake and sit up.", ch);
            comm.act(ATTypes.AT_ACTION, "$n wakes and sits up.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Sitting;
        }
    }
}
