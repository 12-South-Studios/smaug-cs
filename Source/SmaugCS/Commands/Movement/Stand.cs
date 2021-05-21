using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace SmaugCS.Commands.Movement
{
    public static class Stand
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "argument")]
        public static void do_stand(CharacterInstance ch, string argument)
        {
            switch (ch.CurrentPosition)
            {
                case PositionTypes.Sleeping:
                    FromSleeping(ch);
                    break;
                case PositionTypes.Resting:
                    FromResting(ch);
                    break;
                case PositionTypes.Sitting:
                    FromSitting(ch);
                    break;
                case PositionTypes.Standing:
                    ch.SendTo("You are already standing.");
                    break;
                default:
                    if (ch.IsInCombatPosition())
                        ch.SendTo("You are already fighting!");
                    break;
            }
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
