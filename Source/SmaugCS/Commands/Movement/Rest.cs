using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.MudProgs;
using CheckFunctions = SmaugCS.Helpers.CheckFunctions;

namespace SmaugCS.Commands.Movement
{
    public static class Rest
    {
        public static void do_rest(CharacterInstance ch, string argument)
        {
            switch (ch.CurrentPosition)
            {
                case PositionTypes.Sleeping:
                    FromSleeping(ch);
                    break;
                case PositionTypes.Resting:
                    ch.SendTo("You are already resting.");
                    break;
                case PositionTypes.Standing:
                    FromStanding(ch);
                    break;
                case PositionTypes.Sitting:
                    FromSitting(ch);
                    break;
                default:
                    if (ch.IsInCombatPosition())
                        ch.SendTo("You are busy fighting!");
                    else if (ch.CurrentPosition == PositionTypes.Mounted)
                        ch.SendTo("You'd better dismount first.");
                    break;
            }

            MudProgHandler.ExecuteRoomProg(MudProgTypes.Rest, ch);
        }

        private static void FromSitting(CharacterInstance ch)
        {
            ch.SendTo("You lie back and sprawl out to rest.");
            comm.act(ATTypes.AT_ACTION, "$n lies back and sprawls out to rest.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Resting;
        }

        private static void FromStanding(CharacterInstance ch)
        {
            ch.SendTo("You sprawl out haphazardly.");
            comm.act(ATTypes.AT_ACTION, "$n sprawls out haphazardly.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Resting;
        }

        private static void FromSleeping(CharacterInstance ch)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.IsAffected(AffectedByTypes.Sleep), "You can't seem to wake up!"))
                return;

            ch.SendTo("You rouse from your slumber.");
            comm.act(ATTypes.AT_ACTION, "$n rouses from $s slumber.", ch, null, null, ToTypes.Room);
            ch.CurrentPosition = PositionTypes.Resting;
        }
    }
}
