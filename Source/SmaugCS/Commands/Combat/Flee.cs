using SmaugCS.Commands.Movement;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Player;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Combat
{
    public static class Flee
    {
        public static void do_flee(CharacterInstance ch, string argument)
        {
            if (ch.GetMyTarget() == null)
            {
                if (ch.IsInCombatPosition())
                    ch.CurrentPosition = ch.CurrentMount != null ? PositionTypes.Mounted : PositionTypes.Standing;
                ch.SendTo("You aren't fighting anyone.");
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

            var wasIn = ch.CurrentRoom;

            comm.act(ATTypes.AT_FLEE, "You attempt to flee from combat, but can't escape!", ch, null, null, ToTypes.Character);

            if (AttemptToFlee(ch, wasIn))
                return;

            if (!ch.IsNpc() && ch.Level < LevelConstants.AvatarLevel && SmaugRandom.Bits(3) == 1)
                LoseExperience((PlayerInstance)ch);
        }

        private static void LoseExperience(PlayerInstance ch)
        {
            var lostXp = (int) ((ch.GetExperienceLevel(ch.Level + 1) - ch.GetExperienceLevel(ch.Level))*0.1f);
            comm.act(ATTypes.AT_FLEE, string.Format("Curse the gods, you've lost {0} experience!", lostXp), ch, null,
                null, ToTypes.Character);
            ch.GainXP(0 - lostXp);
        }

        private static bool AttemptToFlee(CharacterInstance ch, RoomTemplate wasIn)
        {
            var success = false;

            for (var i = 0; i < 8; i++)
            {
                success = MakeFleeAttempt(ch, wasIn);
                if (success) break;
            }

            return success;
        }

        private static bool MakeFleeAttempt(CharacterInstance ch, RoomTemplate wasIn)
        {
            var door = db.number_door();
            var exit = wasIn.GetExit(door);
            if (exit == null
                || exit.GetDestination() == null
                || exit.Flags.IsSet(ExitFlags.NoFlee)
                || (exit.Flags.IsSet(ExitFlags.Closed) || !ch.IsAffected(AffectedByTypes.PassDoor))
                || (ch.IsNpc() && exit.GetDestination().Flags.IsSet(RoomFlags.NoMob)))
                return false;

            var sneak = DatabaseManager.Instance.GetEntity<SkillData>("sneak");
            if (sneak == null) return false;

            ch.StripAffects((int)sneak.ID);
            ch.AffectedBy = ch.AffectedBy.RemoveBit(AffectedByTypes.Sneak);

            if (ch.CurrentMount != null && ch.CurrentMount.CurrentFighting != null)
                ch.CurrentMount.StopFighting(true);
            Move.move_char(ch, exit, 0);

            var nowIn = ch.CurrentRoom;
            if (nowIn == wasIn) return false;

            ch.CurrentRoom = wasIn;
            comm.act(ATTypes.AT_FLEE, "$n flees head over heels!", ch, null, null, ToTypes.Room);

            ch.CurrentRoom = nowIn;
            comm.act(ATTypes.AT_FLEE, "$n glances around for signs of pursuit.", ch, null, null, ToTypes.Room);

            if (!ch.IsNpc())
            {
                var wf = ch.GetMyTarget();
                var pch = (PlayerInstance)ch;

                comm.act(ATTypes.AT_FLEE, "You flee head over heels from combat!", pch, null, null, ToTypes.Character);

                if (pch.Level < LevelConstants.AvatarLevel)
                    LoseExperience(pch);

                if (wf != null)
                {
                    if (pch.PlayerData.CurrentDeity != null)
                    {
                        var ratio = 1.GetNumberThatIsBetween(wf.Level / pch.Level, LevelConstants.MaxLevel);
                        if (wf.CurrentRace == pch.PlayerData.CurrentDeity.NPCRace)
                            pch.AdjustFavor(DeityFieldTypes.FleeNPCRace, ratio);
                        else if (wf.CurrentRace == pch.PlayerData.CurrentDeity.NPCFoe)
                            pch.AdjustFavor(DeityFieldTypes.FleeNPCFoe, ratio);
                        else
                            pch.AdjustFavor(DeityFieldTypes.Flee, ratio);
                    }
                }
            }

            ch.StopFighting(true);
            return true;
        }
    }
}
