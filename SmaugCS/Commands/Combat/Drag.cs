using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Commands.Movement;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Combat
{
    public static class Drag
    {
        public static void do_drag(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNpc(ch, ch, "Only characters can drag.")) return;
            if (CheckFunctions.CheckIfTrue(ch, ch.HasTimer(TimerTypes.PKilled), "You can't drag a player right now."))
                return;

            PlayerInstance pch = (PlayerInstance) ch;

            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(pch, firstArg, "Drag whom?")) return;

            CharacterInstance victim = pch.GetCharacterInRoom(firstArg);
            if (CheckFunctions.CheckIfNullObject(pch, victim, "They aren't here.")) return;
            if (CheckFunctions.CheckIfEquivalent(pch, pch, victim,
                "You take yourself by the scruff of your neck, but go nowhere.")) return;
            if (CheckFunctions.CheckIfNpc(ch, victim, "You can only drag characters.")) return;
            if (CheckFunctions.CheckIfTrue(ch, !victim.Act.IsSet(PlayerFlags.ShoveDrag)
                || (!victim.IsNpc() && !((PlayerInstance) victim).PlayerData.Flags.IsSet(PCFlags.Deadly)),
                "That character doesn't seem to appreciate your attentions.")) return;
            if (CheckFunctions.CheckIfTrue(ch, victim.HasTimer(TimerTypes.PKilled),
                "You can't drag that player right now.")) return;
            if (CheckFunctions.CheckIf(ch, HelperFunctions.IsFighting,
                "You try, but can't get close enough.", new List<object> {ch})) return;
            if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && !((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Deadly)
                                               && ((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Deadly),
                "You can't drag a deadly character.")) return;
            if (CheckFunctions.CheckIfTrue(ch, !ch.IsNpc() && !((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Deadly) 
                && (int)ch.CurrentPosition > 3, "They don't seem to need your assistance.")) return;

            string secondArg = argument.SecondWord();
            if (CheckFunctions.CheckIfEmptyString(ch, secondArg, "Drag them in which direction?")) return;
            if (CheckFunctions.CheckIfTrue(ch, Math.Abs(ch.Level - victim.Level) > 5,
                "There is too great an experience difference for you to even bother.")) return;
            if (CheckFunctions.CheckIfTrue(ch,
                victim.CurrentRoom.Flags.IsSet(RoomFlags.Safe) && victim.GetTimer(TimerTypes.ShoveDrag) == null,
                "That character cannot be dragged right now."))
                return;

            DirectionTypes exitDir = Realm.Library.Common.EnumerationExtensions.GetEnumByName<DirectionTypes>(secondArg);
            ExitData exit = ch.CurrentRoom.GetExit(exitDir);
            if (CheckFunctions.CheckIfNullObject(ch, exit, "There's no exit in that direction.")) return;
            if (CheckFunctions.CheckIfTrue(ch, exit.Flags.IsSet(ExitFlags.Closed)
                                               &&
                                               (!victim.IsAffected(AffectedByTypes.PassDoor) ||
                                                exit.Flags.IsSet(ExitFlags.NoPassDoor)),
                "There's no exit in that direction.")) return;

            RoomTemplate toRoom = exit.GetDestination();
            if (CheckFunctions.CheckIfSet(ch, toRoom.Flags, RoomFlags.Death,
                "You cannot drag someone into a death trap.")) return;

            if (CheckFunctions.CheckIfTrue(ch, ch.CurrentRoom.Area != toRoom.Area && !toRoom.Area.InHardRange(victim),
                "That character cannot enter that area."))
            {
                victim.CurrentPosition = PositionTypes.Standing;
                return;
            }

            int chance = GetChanceByCharacterClass(ch);
            chance += (ch.GetCurrentStrength() - 15)*3;
            chance += (ch.Level - victim.Level);
            chance += GetBonusByCharacterRace(ch);

            if (CheckFunctions.CheckIfTrue(ch, chance < SmaugRandom.D100(), "You failed."))
            {
                victim.CurrentPosition = PositionTypes.Standing;
                return;
            }

            if ((int) victim.CurrentPosition < (int) PositionTypes.Standing)
            {
                PositionTypes temp = victim.CurrentPosition;
                victim.CurrentPosition = PositionTypes.Drag;

                comm.act(ATTypes.AT_ACTION, "You drag $M into the next room.", ch, victim, null, ToTypes.Character);
                comm.act(ATTypes.AT_ACTION, "$n grabs your hair and drags you.", ch, victim, null, ToTypes.Victim);
                Move.move_char(victim, exit, 0);

                if (!victim.CharDied())
                    victim.CurrentPosition = temp;

                Move.move_char(ch, exit, 0);
                Macros.WAIT_STATE(ch, 12);
                return;
            }

            color.send_to_char("You cannot do that to someone who is standing.", ch);
        }

        private static int GetChanceByCharacterClass(CharacterInstance ch)
        {
            DragValueAttribute attrib = ch.CurrentClass.GetAttribute<DragValueAttribute>();
            return attrib == null ? 0 : attrib.ModValue;
        }

        private static int GetBonusByCharacterRace(CharacterInstance ch)
        {
            DragValueAttribute attrib = ch.CurrentRace.GetAttribute<DragValueAttribute>();
            return attrib == null ? 0 : attrib.ModValue;
        }
    }
}
