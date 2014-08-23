﻿using Realm.Library.Common;
using SmaugCS.Commands.Movement;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Combat
{
    public static class Shove
    {
        public static void do_shove(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.IsNpc() || !ch.PlayerData.Flags.IsSet(PCFlags.Deadly),
                "Only deadly characters can shove.")) return;

            TimerData timer = ch.GetTimer(TimerTypes.PKilled);
            if (CheckFunctions.CheckIfNotNullObject(ch, timer, "You can't shove a player right now.")) return;

            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Shove whom?")) return;

            CharacterInstance victim = ch.GetCharacterInRoom(firstArg);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "They aren't here.")) return;
            if (CheckFunctions.CheckIfEquivalent(ch, ch, victim, "You shove yourself around, to no avail.")) return;
            if (CheckFunctions.CheckIfTrue(ch, victim.IsNpc() || !victim.PlayerData.Flags.IsSet(PCFlags.Deadly),
                "You can only shove deadly characters.")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.Level.GetAbsoluteDifference(victim.Level) > 5,
                "There is too great an experience difference for you to even bother.")) return;

            timer = victim.GetTimer(TimerTypes.PKilled);
            if (CheckFunctions.CheckIfNullObject(ch, timer, "You can't shove that player right now.")) return;

            if (victim.CurrentPosition != PositionTypes.Standing)
            {
                comm.act(ATTypes.AT_PLAIN, "$N isn't standing up.", ch, null, victim, ToTypes.Character);
                return;
            }

            string secondArg = argument.SecondWord();
            if (CheckFunctions.CheckIfEmptyString(ch, secondArg, "Shove them in which direction?")) return;

            timer = victim.GetTimer(TimerTypes.ShoveDrag);
            if (CheckFunctions.CheckIfTrue(ch, victim.CurrentRoom.Flags.IsSet(RoomFlags.Safe) && timer == null,
                "That character cannot be shoved right now."))
                return;

            victim.CurrentPosition = PositionTypes.Shove;
            
            DirectionTypes exitDir = Realm.Library.Common.EnumerationExtensions.GetEnumByName<DirectionTypes>(secondArg);
            ExitData exit = ch.CurrentRoom.GetExit(exitDir);
            if (CheckFunctions.CheckIfNullObject(ch, exit, "There's no exit in that direction."))
            {
                victim.CurrentPosition = PositionTypes.Standing;
                return;
            }
            if (CheckFunctions.CheckIfTrue(ch,
                exit.Flags.IsSet(ExitFlags.Closed) &&
                (!victim.IsAffected(AffectedByTypes.PassDoor) || exit.Flags.IsSet(ExitFlags.NoPassDoor)),
                "There's no exit in that direction."))
            {
                victim.CurrentPosition = PositionTypes.Standing;
                return;
            }

            RoomTemplate toRoom = exit.GetDestination();
            if (CheckFunctions.CheckIfSet(ch, toRoom.Flags, RoomFlags.Death,
                "You cannot shove someone into a death trap."))
            {
                victim.CurrentPosition = PositionTypes.Standing;
                return;
            }

            if (CheckFunctions.CheckIfTrue(ch, ch.CurrentRoom.Area != toRoom.Area && !toRoom.Area.InHardRange(victim),
                "That character cannot enter that area."))
            {
                victim.CurrentPosition = PositionTypes.Standing;
                return;
            }

            int chance = GetChanceByCharacterClass(ch);
            chance += ((ch.GetCurrentStrength() - 15)*3);
            chance += ch.Level - victim.Level;
            chance += GetBonusByCharacterRace(ch);

            if (CheckFunctions.CheckIfTrue(ch, chance < SmaugRandom.Percent(), "You failed."))
            {
                victim.CurrentPosition = PositionTypes.Standing;
                return;
            }

            comm.act(ATTypes.AT_ACTION, "You shove $M.", ch, null, victim, ToTypes.Character);
            comm.act(ATTypes.AT_ACTION, "$n shoves you.", ch, null, victim, ToTypes.Victim);
            Move.move_char(victim, exit, 0);

            if (!victim.CharDied())
                victim.CurrentPosition = PositionTypes.Standing;

            Macros.WAIT_STATE(ch, 12);

            timer = ch.GetTimer(TimerTypes.ShoveDrag);
            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe)
                && timer == null)
                ch.AddTimer(TimerTypes.ShoveDrag, 10, null, 0);
        }

        private static int GetChanceByCharacterClass(CharacterInstance ch)
        {
            ShoveValueAttribute attrib = ch.CurrentClass.GetAttribute<ShoveValueAttribute>();
            return attrib == null ? 0 : attrib.ModValue;
        }

        private static int GetBonusByCharacterRace(CharacterInstance ch)
        {
            ShoveValueAttribute attrib = ch.CurrentRace.GetAttribute<ShoveValueAttribute>();
            return attrib == null ? 0 : attrib.ModValue;
        }
    }
}
