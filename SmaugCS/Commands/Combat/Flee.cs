using SmaugCS.Commands.Movement;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Combat
{
    public static class Flee
    {
        public static void do_flee(CharacterInstance ch, string argument)
        {
            if (fight.who_fighting(ch) == null)
            {
                if (ch.IsInCombatPosition())
                    ch.CurrentPosition = ch.CurrentMount != null ? PositionTypes.Mounted : PositionTypes.Standing;
                color.send_to_char("You aren't fighting anyone.", ch);
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, ch.IsAffected(AffectedByTypes.Berserk),
                "Flee while berserking? You aren't thinking very clearly...")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.CurrentMovement <= 0, "You're too exhausted to flee from combat!"))
                return;
            if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && (int) ch.CurrentPosition < (int) PositionTypes.Fighting,
                "You can't flee in an aggressive stance...")) return;
            if (ch.IsNpc() && (int) ch.CurrentPosition <= (int) PositionTypes.Sleeping)
                return;

            RoomTemplate wasIn = ch.CurrentRoom;

            comm.act(ATTypes.AT_FLEE, "You attempt to flee from combat, but can't escape!", ch, null, null, ToTypes.Character);

            if (AttemptToFlee(ch, wasIn))
                return;

            if (ch.Level < LevelConstants.AvatarLevel && SmaugRandom.Bits(3) == 1)
                LoseExperience(ch);
        }

        private static void LoseExperience(CharacterInstance ch)
        {
            int lostXp = (int) ((ch.GetExperienceLevel(ch.Level + 1) - ch.GetExperienceLevel(ch.Level))*0.1f);
            comm.act(ATTypes.AT_FLEE, string.Format("Curse the gods, you've lost {0} experience!", lostXp), ch, null,
                null, ToTypes.Character);
            ch.GainXP(0 - lostXp);
        }

        private static bool AttemptToFlee(CharacterInstance ch, RoomTemplate wasIn)
        {
            bool success = false;

            for (int i = 0; i < 8; i++)
            {
                success = MakeFleeAttempt(ch, wasIn);
                if (success) break;
            }

            return success;
        }

        private static bool MakeFleeAttempt(CharacterInstance ch, RoomTemplate wasIn)
        {
            int door = db.number_door();
            ExitData exit = wasIn.GetExit(door);
            if (exit == null
                || exit.GetDestination() == null
                || exit.Flags.IsSet(ExitFlags.NoFlee)
                || (exit.Flags.IsSet(ExitFlags.Closed) || !ch.IsAffected(AffectedByTypes.PassDoor))
                || (ch.IsNpc() && exit.GetDestination().Flags.IsSet(RoomFlags.NoMob)))
                return false;

            SkillData sneak = DatabaseManager.Instance.GetEntity<SkillData>("sneak");
            if (sneak == null) return false;

            ch.StripAffects((int)sneak.ID);
            ch.AffectedBy.RemoveBit(AffectedByTypes.Sneak);

            if (ch.CurrentMount != null && ch.CurrentMount.CurrentFighting != null)
                fight.stop_fighting(ch.CurrentMount, true);
            Move.move_char(ch, exit, 0);

            RoomTemplate nowIn = ch.CurrentRoom;
            if (nowIn == wasIn) return false;

            ch.CurrentRoom = wasIn;
            comm.act(ATTypes.AT_FLEE, "$n flees head over heels!", ch, null, null, ToTypes.Room);

            ch.CurrentRoom = nowIn;
            comm.act(ATTypes.AT_FLEE, "$n glances around for signs of pursuit.", ch, null, null, ToTypes.Room);

            if (!ch.IsNpc())
            {
                CharacterInstance wf = fight.who_fighting(ch);
                comm.act(ATTypes.AT_FLEE, "You flee head over heels from combat!", ch, null, null, ToTypes.Character);

                if (ch.Level < LevelConstants.AvatarLevel)
                    LoseExperience(ch);

                if (wf != null && ch.PlayerData.CurrentDeity != null)
                {
                    int ratio = 1.GetNumberThatIsBetween(wf.Level/ch.Level, LevelConstants.MaxLevel);
                    if (wf.CurrentRace == ch.PlayerData.CurrentDeity.NPCRace)
                        ch.AdjustFavor(DeityFieldTypes.FleeNPCRace, ratio);
                    else if (wf.CurrentRace == ch.PlayerData.CurrentDeity.NPCFoe)
                        ch.AdjustFavor(DeityFieldTypes.FleeNPCFoe, ratio);
                    else 
                        ch.AdjustFavor(DeityFieldTypes.Flee, ratio);
                }
            }

            fight.stop_fighting(ch, true);
            return true;
        }
    }
}
