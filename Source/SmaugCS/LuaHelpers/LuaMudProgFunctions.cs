using System.Linq;
using LuaInterface;
using Realm.Library.Lua;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions.Character;

namespace SmaugCS.LuaHelpers
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

            return ch.IsNpc() && ch.Act.IsSet(ActFlags.Pacifist);
        }

        [LuaFunction("LIsMobInvis", "Gets if the mobile is invisible", "Character")]
        public static bool LuaIsMobInvis(CharacterInstance ch)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");

            return ch.IsNpc() && ch.Act.IsSet(ActFlags.MobInvisibility);
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
                return ch.CanSee((CharacterInstance) target);
            if (target is ObjectInstance)
                return ch.CanSee((ObjectInstance) target);

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

        // CanPKill
        // IsMounted
        // IsMorphed
        // IsNuisance
        // IsGood
        // IsNeutral
        // IsEvil
        // IsFighting
        // IsImmortal
        // IsCharmed
        // IsFlying
        // IsThief
        // IsAttacker
        // IsKiller
        // IsFollowing
        // IsAffectedBy
        // GetNumberFighting
        // GetHitPercent
        // IsInRoom
        // WasInRoom
        // IsNoRecallRoom
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
