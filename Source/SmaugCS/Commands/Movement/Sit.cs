using System.Diagnostics.CodeAnalysis;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Movement
{
    public static class Sit
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "argument")]
        public static void do_sit(CharacterInstance ch, string argument)
        {
            if (ch.CurrentPosition == PositionTypes.Sleeping)
                FromSleeping(ch);
            else if (ch.CurrentPosition == PositionTypes.Resting)
                FromResting(ch);
            else if (ch.CurrentPosition == PositionTypes.Standing)
                FromStanding(ch);
            else if (ch.CurrentPosition == PositionTypes.Sitting)
                ch.SendTo("You are already sitting.");
            else if (ch.IsInCombatPosition())
                ch.SendTo("You are busy fighting!");
            else if (ch.CurrentPosition == PositionTypes.Mounted)
                ch.SendTo("You are already sitting - on your mount.");
        }

        private static void FromStanding(CharacterInstance ch)
        {
            ch.SendTo("You sit down.");
            comm.act(ATTypes.AT_ACTION, "$n sits down.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Sitting;
        }

        private static void FromResting(CharacterInstance ch)
        {
            ch.SendTo("You stop resting and sit up.");
            comm.act(ATTypes.AT_ACTION, "$n stops resting and sits up.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Sitting;
        }

        private static void FromSleeping(CharacterInstance ch)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.IsAffected(AffectedByTypes.Sleep), "You can't seem to wake up!"))
                return;

            ch.SendTo("You wake and sit up.");
            comm.act(ATTypes.AT_ACTION, "$n wakes and sits up.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Sitting;
        }
    }
}
