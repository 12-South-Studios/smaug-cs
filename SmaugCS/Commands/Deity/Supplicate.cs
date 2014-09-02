using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Deity
{
    public static class Supplicate
    {
        private static readonly Dictionary<string, Action<CharacterInstance, string>> SupplicateTable = new Dictionary<string, Action<CharacterInstance, string>>
        {
            {"corpse", SupplicateForCorpse},
            {"avatar", SupplicateForAvatar},
            {"object", SupplicateForObject},
            {"recall", SupplicateForRecall}
        }; 

        public static void do_supplicate(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.IsNpc() || ch.PlayerData.CurrentDeity == null,
                "You have no deity to supplicate to.")) return;

            string firstArg = argument.FirstWord();
            if (CheckFunctions.CheckIfEmptyString(ch, firstArg, "Supplicate for what?")) return;

            if (SupplicateTable.ContainsKey(firstArg.ToLower()))
                SupplicateTable[firstArg.ToLower()].Invoke(ch, argument);
            else 
                color.send_to_char("You cannot supplicate for that.", ch);
        }

        private static void SupplicateForCorpse(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SupplicateCorpseCost,
                "You are not favored enough for a corpse retrieval.")) return;
            if (CheckFunctions.CheckIfSet(ch, ch.CurrentRoom.Flags, RoomFlags.ClanStoreroom,
                "You cannot supplicate in a storage room.")) return;

            ObjectInstance corpse =
                ch.CurrentRoom.Contents.FirstOrDefault(
                    x => x.ShortDescription.Equals(string.Format("the corpse of {0}", ch.Name)));
            if (CheckFunctions.CheckIfNullObject(ch, corpse, "No corpse of yours litters the world...")) return;
            if (CheckFunctions.CheckIfSet(ch, corpse.InRoom.Flags, RoomFlags.NoSupplicate,
                "The image of your corpse appears, but suddenly fades away.")) return;

            comm.act(ATTypes.AT_MAGIC, "Your corpse appears suddenly, surrounded by a divine presence...", ch, null, null, ToTypes.Character);
            comm.act(ATTypes.AT_MAGIC, "$n's corpse appears suddenly, surrounded by a divine force...", ch, null, null, ToTypes.Room);
            corpse.InRoom.FromRoom(corpse);
            ch.CurrentRoom.ToRoom(corpse);
            corpse.ExtraFlags.RemoveBit(ItemExtraFlags.Buried);

            ch.PlayerData.Favor -= ch.PlayerData.CurrentDeity.SupplicateCorpseCost;

            // TODO Do suscept, element and affects
        }

        private static void SupplicateForAvatar(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SupplicateAvatarCost,
                "You are not favored enough for that.")) return;

            MobTemplate template = DatabaseManager.Instance.MOBILE_INDEXES.Get(VnumConstants.MOB_VNUM_DEITY);
            CharacterInstance mob = DatabaseManager.Instance.CHARACTERS.Create(template);

            ch.CurrentRoom.ToRoom(mob);

            comm.act(ATTypes.AT_MAGIC, "$n summons a powerful avatar!", ch, null, null, ToTypes.Room);
            comm.act(ATTypes.AT_MAGIC, "You summon a powerful avatar!", ch, null, null, ToTypes.Character);
            mob.AddFollower(ch);
            mob.AffectedBy.SetBit(AffectedByTypes.Charm);
            mob.Level = 10;
            mob.MaximumHealth = ch.MaximumHealth*6 + ch.PlayerData.Favor;
            mob.CurrentAlignment = ch.PlayerData.CurrentDeity.Alignment;
            ch.PlayerData.Favor -= ch.PlayerData.CurrentDeity.SupplicateAvatarCost;

            // TODO Do suscept, element and affects
        }

        private static void SupplicateForObject(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SupplicateDeityObjectCost,
                "You are not favored enough for that.")) return;

            ObjectTemplate template = DatabaseManager.Instance.OBJECT_INDEXES.Get(VnumConstants.OBJ_VNUM_DEITY);
            ObjectInstance obj = DatabaseManager.Instance.OBJECTS.Create(template, ch.Level,
                string.Format("sigil {0}", ch.PlayerData.CurrentDeity.Name));
            obj = obj.WearFlags.IsSet(ItemWearFlags.Take) ? obj.ToCharacter(ch) : ch.CurrentRoom.ToRoom(obj);

            comm.act(ATTypes.AT_MAGIC, "$n weaves $p from divine matter!", ch, obj, null, ToTypes.Room);
            comm.act(ATTypes.AT_MAGIC, "You weave $p from divine matter!", ch, obj, null, ToTypes.Character);
            ch.PlayerData.Favor -= ch.PlayerData.CurrentDeity.SupplicateDeityObjectCost;

            // TODO Do suscept, element and affects

            AffectData af = new AffectData();
            af.Type = AffectedByTypes.None;
            af.Duration = -1;
            af.Location = GetApplyTypeForDeity(ch.PlayerData.CurrentDeity);
            af.Modifier = 1;
            obj.Affects.Add(af);
        }

        private static ApplyTypes GetApplyTypeForDeity(DeityData deityData)
        {
            switch (deityData.ObjStat)
            {
                case 0:
                    return ApplyTypes.Strength;
                case 1:
                    return ApplyTypes.Intelligence;
                case 2:
                    return ApplyTypes.Wisdom;
                case 3:
                    return ApplyTypes.Constitution;
                case 4:
                    return ApplyTypes.Dexterity;
                case 5:
                    return ApplyTypes.Charisma;
                case 6:
                    return ApplyTypes.Luck;
            }
            return ApplyTypes.None;
        }

        private static void SupplicateForRecall(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch, ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SupplicateRecallCost,
                   "Your favor is inadequate for such a supplication.")) return;
            if (CheckFunctions.CheckIfSet(ch, ch.CurrentRoom.Flags, RoomFlags.NoSupplicate, "You have been forsaken!"))
                return;
            if (CheckFunctions.CheckIfTrue(ch, ch.HasTimer(TimerTypes.RecentFight) && !ch.IsImmortal(),
                "You cannot supplicate recall under adrenaline!")) return;

            RoomTemplate location = null;

            if (!ch.IsNpc() && ch.PlayerData.Clan != null)
                location = DatabaseManager.Instance.ROOMS.Get(ch.PlayerData.Clan.RecallRoom);

            if (!ch.IsNpc() && location == null && ch.Level >= 5 && ch.PlayerData.Flags.IsSet(PCFlags.Deadly))
                location = DatabaseManager.Instance.ROOMS.Get(VnumConstants.ROOM_VNUM_DEADLY);

            if (location == null)
            {
                int raceRecallRoom = DatabaseManager.Instance.RACES.Get(ch.CurrentRace.GetValue()).RaceRecallRoom;
                location = DatabaseManager.Instance.ROOMS.Get(raceRecallRoom);
            }

            if (location == null)
                location = DatabaseManager.Instance.ROOMS.Get(VnumConstants.ROOM_VNUM_TEMPLE);

            if (CheckFunctions.CheckIfNullObject(ch, location, "You are completely lost.")) return;

            comm.act(ATTypes.AT_MAGIC, "$n disappears in a column of divine power.", ch, null, null, ToTypes.Room);

            RoomTemplate oldRoom = ch.CurrentRoom;
            oldRoom.FromRoom(ch);
            location.ToRoom(ch);

            if (ch.CurrentMount != null)
            {
                oldRoom.FromRoom(ch.CurrentMount);
                location.ToRoom(ch.CurrentMount);
            }

            comm.act(ATTypes.AT_MAGIC, "$n appears in the room from a column of divine power.", ch, null, null, ToTypes.Room);

            Look.do_look(ch, "auto");
            ch.PlayerData.Favor -= ch.PlayerData.CurrentDeity.SupplicateRecallCost;

            // TODO Do suscept, element and affects
        }
    }
}
