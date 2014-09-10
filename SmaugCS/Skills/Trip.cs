using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Skills
{
    public static class Trip
    {
        public static void CheckTrip(CharacterInstance ch, CharacterInstance victim)
        {
            if (victim.IsAffected(AffectedByTypes.Flying)
                || victim.CurrentMount.IsAffected(AffectedByTypes.Floating))
                return;

            if (victim.CurrentMount != null)
                TripMount(ch, victim);
            else if (victim.wait == 0)
            {
                comm.act(ATTypes.AT_SKILL, "$n trips you and you go down!", ch, null, victim, ToTypes.Victim);
                comm.act(ATTypes.AT_SKILL, "You trip $N and $N goes down!", ch, null, victim, ToTypes.Character);
                comm.act(ATTypes.AT_SKILL, "$n trips $N and $N goes down!", ch, null, victim, ToTypes.Room);

                Macros.WAIT_STATE(ch, 2 * GameConstants.GetSystemValue<int>("PulseViolence"));
                Macros.WAIT_STATE(victim, 2 * GameConstants.GetSystemValue<int>("PulseViolence"));
                victim.CurrentPosition = PositionTypes.Resting;
            }
        }

        private static void TripMount(CharacterInstance ch, CharacterInstance victim)
        {
            if (victim.CurrentMount.IsAffected(AffectedByTypes.Flying)
                || victim.CurrentMount.IsAffected(AffectedByTypes.Floating))
                return;

            comm.act(ATTypes.AT_SKILL, "$n trips your mount and you fall off!", ch, null, victim, ToTypes.Victim);
            comm.act(ATTypes.AT_SKILL, "You trip $N's mount and $N falls off!", ch, null, victim, ToTypes.Character);
            comm.act(ATTypes.AT_SKILL, "$n trips $N's mount and $N falls off!", ch, null, victim, ToTypes.Room);

            victim.CurrentMount.Act.RemoveBit(ActFlags.Mounted);
            victim.CurrentMount = null;

            Macros.WAIT_STATE(ch, 2 * GameConstants.GetSystemValue<int>("PulseViolence"));
            Macros.WAIT_STATE(victim, 2 * GameConstants.GetSystemValue<int>("PulseViolence"));
            victim.CurrentPosition = PositionTypes.Resting;
        }
    }
}
