using Realm.Library.Lua;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using System.Linq;
using EnumerationExtensions = SmaugCS.Common.EnumerationExtensions;

namespace SmaugCS
{
    public static class LuaMudProgFunctions
    {
        [LuaFunction("LIsCarrying", "Gets true if the character is carrying the object", "Character", "Object", "Id to Check")]
        public static bool LuaIsCarrying(CharacterInstance ch, ObjectInstance obj, int id)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");
            if (id <= 0)
                throw new LuaException("Id cannot be less than or equal to zero");

            if (obj == null)
                return ch.Carrying.Any(x => x.ID == id);

            if (obj.WearLocation == WearLocations.None && (obj.ObjectIndex.ID == id || obj.ID == id))
                return true;

            return obj.Contents.Any(x => LuaIsCarrying(ch, x, id));
        }

        [LuaFunction("LIsPacifist", "Gets if the mobile is a pacifist", "Character")]
        public static bool LuaIsPacifist(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsNpc() && ch.Act.IsSet((int)ActFlags.Pacifist);
        }

        [LuaFunction("LIsMobInvis", "Gets if the mobile is invisible", "Character")]
        public static bool LuaIsMobInvis(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsNpc() && ch.Act.IsSet((int)ActFlags.MobInvisibility);
        }

        [LuaFunction("LIsPC", "Gets if the character is a player", "Character")]
        public static bool LuaIsPC(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return !ch.IsNpc();
        }

        [LuaFunction("LIsNPC", "Gets if the character is a npc", "Character")]
        public static bool LuaIsNPC(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsNpc();
        }

        [LuaFunction("LCanSee", "Gets if the character can see the target", "Character", "Target")]
        public static bool LuaCanSee(CharacterInstance ch, Instance target)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");
            if (target == null)
                throw new LuaException("Target cannot be null");

            if (target is CharacterInstance)
                return ch.CanSee((CharacterInstance)target);
            if (target is ObjectInstance)
                return ch.CanSee((ObjectInstance)target);

            return false;
        }

        [LuaFunction("LIsRiding", "Gets if the character is riding the target", "Character", "Target")]
        public static bool LuaIsRiding(CharacterInstance ch, CharacterInstance target)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");
            if (target == null)
                throw new LuaException("Target cannot be null");

            return ch.CurrentMount != null && ch.CurrentMount == target;
        }

        [LuaFunction("LIsPassage", "Gets if the argument is an exit", "Room", "Direction")]
        public static bool LuaIsPassage(RoomTemplate room, int direction)
        {
            if (room == null)
                throw new LuaException("Room cannot be null");

            return room.GetExit(direction) != null;
        }

        [LuaFunction("LIsExitOpen", "Gets if the exit is open", "Room", "Direction")]
        public static bool LuaIsExitOpen(RoomTemplate room, int direction)
        {
            if (room == null)
                throw new LuaException("Room cannot be null");

            var exit = room.GetExit(direction);
            if (exit == null) return false;

            return !exit.Flags.IsSet(ExitFlags.Closed);
        }

        [LuaFunction("LIsObjectOpen", "Gets if the object is open", "Object")]
        public static bool LuaIsObjectOpen(ObjectInstance obj)
        {
            if (obj == null)
                throw new LuaException("Object cannot be null");

            if (obj.ItemType != ItemTypes.Container) return false;

            return !obj.Values.Flags.IsSet(ContainerFlags.Closed);
        }

        [LuaFunction("LIsExitLocked", "Gets if the exit is locked", "Room", "Direction")]
        public static bool LuaIsExitLocked(RoomTemplate room, int direction)
        {
            if (room == null)
                throw new LuaException("Room cannot be null");

            var exit = room.GetExit(direction);
            if (exit == null) return false;

            return !exit.Flags.IsSet(ExitFlags.Locked);
        }

        [LuaFunction("LIsObjectLocked", "Gets if the object is locked", "Object")]
        public static bool LuaIsObjectLocked(ObjectInstance obj)
        {
            if (obj == null)
                throw new LuaException("Object cannot be null");

            if (obj.ItemType != ItemTypes.Container) return false;

            return !obj.Values.Flags.IsSet(ContainerFlags.Locked);
        }

        [LuaFunction("LIsPKil", "Gets if the player is PKill", "Character")]
        public static bool LuaIsPKill(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsPKill();
        }

        [LuaFunction("LIsDevoted", "Gets if the player is devoted", "Character")]
        public static bool LuaIsDevoted(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsDevoted();
        }

        [LuaFunction("LCanPKill", "Gets if the player can pkill", "Character")]
        public static bool LuaCanPKill(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.CanPKill();
        }

        [LuaFunction("LIsMounted", "Gets if the player is mounted", "Character")]
        public static bool LuaIsMounted(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.CurrentPosition == PositionTypes.Mounted && ch.CurrentMount != null;
        }

        [LuaFunction("LIsMorphed", "Gets if the player is polymorphed", "Character")]
        public static bool LuaIsMorphed(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.CurrentMorph != null;
        }

        [LuaFunction("LIsNuisance", "Gets if the player is a nuisance", "Character")]
        public static bool LuaIsNuisance(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            if (ch.IsNpc()) return false;

            var player = (PlayerInstance)ch;
            return player.PlayerData.Nuisance != null;
        }

        [LuaFunction("LIsGood", "Gets if the player is good", "Character")]
        public static bool LuaIsGood(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsGood();
        }

        [LuaFunction("LIsEvil", "Gets if the player is evil", "Character")]
        public static bool LuaIsEvil(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsEvil();
        }

        [LuaFunction("LIsNeutral", "Gets if the player is neutral", "Character")]
        public static bool LuaIsNeutral(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsNeutral();
        }

        [LuaFunction("LIsFighting", "Gets if the player is fighting", "Character")]
        public static bool LuaIsFighting(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.CurrentFighting != null;
        }

        [LuaFunction("LIsImmortal", "Gets if the player is immortal", "Character")]
        public static bool LuaIsImmortal(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.Trust >= LevelConstants.ImmortalLevel;
        }

        [LuaFunction("LIsCharmed", "Gets if the player is charmed", "Character")]
        public static bool LuaIsCharmed(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsAffected(AffectedByTypes.Charm);
        }

        [LuaFunction("LIsFlying", "Gets if the player is flying", "Character")]
        public static bool LuaIsFlying(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsAffected(AffectedByTypes.Flying);
        }

        [LuaFunction("LIsThief", "Gets if the player is a thief", "Character")]
        public static bool LuaIsThief(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return !ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.Thief);
        }

        [LuaFunction("LIsAttacker", "Gets if the player is an attacker", "Character")]
        public static bool LuaIsAttacker(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return !ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.Attacker);
        }

        [LuaFunction("LIsKiller", "Gets if the player is a killer", "Character")]
        public static bool LuaIsKiller(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return !ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.Killer);
        }

        [LuaFunction("LIsFollowing", "Gets if the player is a follower", "Character")]
        public static bool LuaIsFollowing(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            if (ch.Master == null) return false;
            return ch.Master.CurrentRoom == ch.CurrentRoom;
        }

        [LuaFunction("LIsAffectedByNumeric", "Gets if the player is affected by X", "Character", "Affected By Value")]
        public static bool LuaIsAffected(CharacterInstance ch, long affectedBy)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            AffectedByTypes affected = EnumerationExtensions.GetEnum<AffectedByTypes>(affectedBy);
            return ch.IsAffected(affected);
        }

        [LuaFunction("LIsAffectedByString", "Gets if the player is affected by X", "Character", "Affected By name")]
        public static bool LuaIsAffected(CharacterInstance ch, string affectedBy)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            AffectedByTypes affected = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumByName<AffectedByTypes>(affectedBy);
            return ch.IsAffected(affected);
        }

        [LuaFunction("LGetNumberFighting", "Gets the number of combatants the player is fighting", "Character")]
        public static int LuaGetNumberFighting(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.NumberFighting;
        }

        [LuaFunction("LGetHitPercent", "Gets the to-hit percentage of the player", "Character")]
        public static int LuaGetHitPercent(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.GetHitroll();
        }

        [LuaFunction("LIsInRoom", "Gets if the player is in the room", "Character", "Room")]
        public static bool LuaIsInRoom(CharacterInstance ch, long roomId)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.CurrentRoom.ID == roomId;
        }

        [LuaFunction("LWasInRoom", "Gets if the player was in the room", "Character", "Room")]
        public static bool LuaWasInRoom(CharacterInstance ch, long roomId)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.PreviousRoom != null && ch.PreviousRoom.ID == roomId;
        }

        [LuaFunction("LIsNoRecall", "Gets if the player's current room is no-recall", "Character", "Room")]
        public static bool LuaIsNoRecall(CharacterInstance ch, long roomId)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.CurrentRoom.Flags.IsSet(RoomFlags.NoRecall);
        }


        // IsDoingQuest
        // IsHelled
        // IsLeader
        // IsWearing
        // IsWearingById
        // IsCarryingById
        // IsClanLeader
        // IsClan
        // IsFlagged
        // IsTagged
        // MortInWorld?
        // MortInRoom?
        // MortInArea?
        // MortCount
        // MobCount
        // CharCount

        // Obsolete Ones
        // Sex
        // Position
        // Level
        // GoldAmt
        // Class
        // Weight
        // HostDesc
        // Multi?
        // Race
        // Morph
        // Nuisance
        // Clan
        // Council
        // Deity
        // Guild
        // ClanType
        // WaitState
        // ASupressed
        // Favor
        // Statistics
        // ObjType
        // LeverPos
        // Object Values
        // Number
        // Time
        // Name
        // Rank


    }
}
