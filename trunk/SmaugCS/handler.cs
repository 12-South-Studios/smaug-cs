using System;
using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Commands.Admin;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class handler
    {
        public static CharacterInstance SavingCharacter { get; set; }
        public static CharacterInstance LoadingCharacter { get; set; }

        // Private static used by affect_modify
        private static int depth = 0;

        public static void affect_modify(CharacterInstance ch, AffectData affect, bool add)
        {
            int mod = affect.Modifier;
            if (add)
            {
                ch.AffectedBy.SetBits(affect.BitVector);
                if ((int)affect.Location % Program.REVERSE_APPLY == (int)ApplyTypes.RecurringSpell)
                {
                    mod = Math.Abs(mod);
                    SkillData skill = db.SKILLS[mod];

                    if (Macros.IS_VALID_SN(mod) && skill != null && skill.Type == (int)SkillTypes.Spell)
                        ch.AffectedBy.SetBit((int)AffectedByTypes.RecurringSpell);
                    else
                        LogManager.Bug("%s: ApplyTypes.RecurringSpell with bad SN %d", ch.Name, mod);
                }
            }
            else
            {
                ch.AffectedBy.RemoveBits(affect.BitVector);

                if ((int)affect.Location % Program.REVERSE_APPLY == (int)ApplyTypes.RecurringSpell)
                {
                    mod = Math.Abs(mod);
                    SkillData skill = db.SKILLS[mod];

                    if (!Macros.IS_VALID_SN(mod) || skill == null || skill.Type != (int)SkillTypes.Spell)
                        LogManager.Bug("%s: ApplyTypes.RecurringSpell with bad SN %d", ch.Name, mod);
                    ch.AffectedBy.RemoveBit((int)AffectedByTypes.RecurringSpell);
                    return;
                }

                switch ((int)affect.Location % Program.REVERSE_APPLY)
                {
                    case (int)ApplyTypes.Affect:
                        ch.AffectedBy.Bits[0].RemoveBit(mod);
                        return;
                    case (int)ApplyTypes.ExtendedAffect:
                        ch.AffectedBy.RemoveBit(mod);
                        return;
                    case (int)ApplyTypes.Resistance:
                        ch.Resistance.RemoveBit(mod);
                        return;
                    case (int)ApplyTypes.Immunity:
                        ch.Immunity.RemoveBit(mod);
                        return;
                    case (int)ApplyTypes.Susceptibility:
                        ch.Susceptibility.RemoveBit(mod);
                        return;
                    case (int)ApplyTypes.Remove:
                        ch.AffectedBy.Bits[0].RemoveBit(mod);
                        return;
                }
                mod = 0 - mod;
            }

            switch ((int)affect.Location % Program.REVERSE_APPLY)
            {
                case (int)ApplyTypes.Strength:
                    ch.ModStrength += mod;
                    break;
                case (int)ApplyTypes.Dexterity:
                    ch.ModDexterity += mod;
                    break;
                case (int)ApplyTypes.Intelligence:
                    ch.ModIntelligence += mod;
                    break;
                case (int)ApplyTypes.Wisdom:
                    ch.ModWisdom += mod;
                    break;
                case (int)ApplyTypes.Constitution:
                    ch.ModConstitution += mod;
                    break;
                case (int)ApplyTypes.Charisma:
                    ch.ModCharisma += mod;
                    break;
                case (int)ApplyTypes.Luck:
                    ch.ModLuck += mod;
                    break;
                case (int)ApplyTypes.Gender:
                    //ch.Gender = (ch.Gender + mod) % 3;
                    // TODO Fix this
                    //if (ch.Gender < 0)
                    //    ch.Gender += 2;
                    //ch.Gender = Check.Range(0, ch.Gender, 2);
                    break;

                case (int)ApplyTypes.Height:
                    ch.Height += mod;
                    break;
                case (int)ApplyTypes.Weight:
                    ch.Weight += mod;
                    break;
                case (int)ApplyTypes.Mana:
                    ch.MaximumMana += mod;
                    break;
                case (int)ApplyTypes.Hit:
                    ch.MaximumHealth += mod;
                    break;
                case (int)ApplyTypes.Movement:
                    ch.MaximumMovement += mod;
                    break;
                case (int)ApplyTypes.ArmorClass:
                    ch.ArmorClass += mod;
                    break;
                case (int)ApplyTypes.HitRoll:
                    ch.HitRoll.SizeOf += mod;
                    break;
                case (int)ApplyTypes.DamageRoll:
                    ch.DamageRoll.SizeOf += mod;
                    break;

                case (int)ApplyTypes.SaveVsPoison:
                    ch.SavingThrows.SaveVsPoisonDeath += mod;
                    break;
                case (int)ApplyTypes.SaveVsRod:
                    ch.SavingThrows.SaveVsWandRod += mod;
                    break;
                case (int)ApplyTypes.SaveVsParalysis:
                    ch.SavingThrows.SaveVsParalysisPetrify += mod;
                    break;
                case (int)ApplyTypes.SaveVsBreath:
                    ch.SavingThrows.SaveVsBreath += mod;
                    break;
                case (int)ApplyTypes.SaveVsSpell:
                    ch.SavingThrows.SaveVsSpellStaff += mod;
                    break;

                case (int)ApplyTypes.Affect:
                    ch.AffectedBy.Bits[0].SetBit(mod);
                    break;
                case (int)ApplyTypes.ExtendedAffect:
                    ch.AffectedBy.SetBit(mod);
                    break;
                case (int)ApplyTypes.Resistance:
                    ch.Resistance.SetBit(mod);
                    break;
                case (int)ApplyTypes.Immunity:
                    ch.Immunity.SetBit(mod);
                    break;
                case (int)ApplyTypes.Susceptibility:
                    ch.Susceptibility.SetBit(mod);
                    break;
                case (int)ApplyTypes.Remove:
                    ch.AffectedBy.Bits[0].RemoveBit(mod);
                    break;

                case (int)ApplyTypes.Full:
                    if (!ch.IsNpc())
                        ch.PlayerData.ConditionTable[ConditionTypes.Full] = Check.Range(0,
                                                                                               ch.PlayerData
                                                                                                 .ConditionTable[
                                                                                                     ConditionTypes.Full
                                                                                                   ] + mod, 48);
                    break;
                case (int)ApplyTypes.Thirst:
                    if (!ch.IsNpc())
                        ch.PlayerData.ConditionTable[ConditionTypes.Thirsty] = Check.Range(0,
                                                                                                  ch.PlayerData
                                                                                                    .ConditionTable[
                                                                                                        ConditionTypes
                                                                                                            .Thirsty] +
                                                                                                  mod, 48);
                    break;
                case (int)ApplyTypes.Drunk:
                    if (!ch.IsNpc())
                        ch.PlayerData.ConditionTable[ConditionTypes.Drunk] = Check.Range(0,
                                                                                                ch.PlayerData
                                                                                                  .ConditionTable[
                                                                                                      ConditionTypes
                                                                                                          .Drunk] + mod,
                                                                                                48);
                    break;
                case (int)ApplyTypes.Blood:
                    if (!ch.IsNpc())
                        ch.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] = Check.Range(0,
                                                                                                       ch.PlayerData
                                                                                                         .ConditionTable
                                                                                                           [
                                                                                                               ConditionTypes
                                                                                                                   .Bloodthirsty
                                                                                                           ] + mod,
                                                                                                       ch.Level + 10);
                    break;

                case (int)ApplyTypes.MentalState:
                    ch.MentalState = Check.Range(-100, ch.MentalState + mod, 100);
                    break;
                case (int)ApplyTypes.Emotion:
                    ch.EmotionalState = Check.Range(-100, ch.EmotionalState, 100);
                    break;

                case (int)ApplyTypes.StripSN:
                    if (Macros.IS_VALID_SN(mod))
                        ch.StripAffects(mod);
                    else
                        LogManager.Bug("apply_modify: ApplyTypes.StripSN invalid SN %d", mod);
                    break;

                case (int)ApplyTypes.WearSpell:
                case (int)ApplyTypes.RemoveSpell:
                    if ((ch.CurrentRoom.Flags.IsSet((int)RoomFlags.NoMagic)
                         || ch.Immunity.IsSet((int)ResistanceTypes.Magic))
                        || (((int)affect.Location % Program.REVERSE_APPLY) == (int)ApplyTypes.WearSpell && !add)
                        || (((int)affect.Location % Program.REVERSE_APPLY) == (int)ApplyTypes.RemoveSpell && add)
                        || SavingCharacter == ch
                        || LoadingCharacter == ch)
                        return;

                    mod = Math.Abs(mod);
                    SkillData skill = db.SKILLS[mod];

                    if (Macros.IS_VALID_SN(mod) && skill != null && skill.Type == (int)SkillTypes.Spell)
                    {
                        if (skill.Target == (int)TargetTypes.Ignore ||
                            skill.Target == (int)TargetTypes.InvisibleObject)
                        {
                            LogManager.Bug("ApplyTypes.WearSpell trying to apply bad target spell. SN is %d.", mod);
                            return;
                        }
                        int retcode = skill.SpellFunction.Value.Invoke(mod, ch.Level, ch, ch);
                        if (retcode == (int)ReturnTypes.CharacterDied || char_died(ch))
                            return;
                    }
                    break;

                case (int)ApplyTypes.Track:
                    ch.ModifySkill(db.GetSkill("track").Type, mod, add);
                    break;

                // TODO Add the rest

                default:
                    LogManager.Bug("affect_modify: unknown location %d", affect.Location);
                    return;
            }

            ObjectInstance wield = ch.GetEquippedItem(WearLocations.Wield);
            if (!ch.IsNpc() && SavingCharacter != ch
                && wield != null && wield.GetObjectWeight() > GameConstants.str_app[ch.CurrentStrength].Wield)
            {
                if (depth == 0)
                {
                    depth++;
                    comm.act(ATTypes.AT_ACTION, "You are too weak to wield $p any longer.", ch, wield, null,
                             ToTypes.Character);
                    comm.act(ATTypes.AT_ACTION, "$n stops wielding $p.", ch, wield, null, ToTypes.Room);
                    ch.Unequip(wield);
                    depth--;
                }
            }
        }

        public static int falling = 0;

        public static void extract_obj(ObjectInstance obj)
        {
            if (obj_extracted(obj))
            {
                LogManager.Bug("extract_obj: obj %d already extracted", obj.ObjectIndex.Vnum);
                return;
            }

            if (obj.ItemType == ItemTypes.Portal)
                update.remove_portal(obj);

            AuctionData auction = db.AUCTIONS.FirstOrDefault(a => a.ItemForSale == obj);
            if (auction != null)
            {
                string buffer = string.Format("Sale of {0} has been stopped by a system action.", obj.ShortDescription);
                ChatManager.talk_auction(buffer);

                auction.ItemForSale = null;
                if (auction.Buyer != null
                    && auction.Buyer != auction.Seller)
                {
                    auction.Buyer.CurrentCoin += auction.CoinAmount;
                    color.send_to_char("Your money has been returned.\r\n", auction.Buyer);
                }
            }

            if (obj.CarriedBy != null)
                obj.FromCharacter();
            else if (obj.InRoom != null)
                obj.InRoom.FromRoom(obj);
            else if (obj.InObject != null)
                obj.InObject.FromObject(obj);

            ObjectInstance objContent = obj.Contents.Last();
            if (objContent != null)
                extract_obj(objContent);

            obj.Affects.Clear();
            obj.ExtraDescriptions.Clear();

            //trworld_obj_check(obj);

            foreach (RelationData relation in db.RELATIONS
                                                .Where(relation => relation.Types == RelationTypes.OSet_On))
            {
                if (obj == relation.Subject)
                    relation.Actor.CastAs<CharacterInstance>().DestinationBuffer = null;
                else
                    continue;
                db.RELATIONS.Remove(relation);
            }

            db.OBJECTS.Remove(obj);

            queue_extracted_obj(obj);

            obj.ObjectIndex.count -= obj.Count;
            db.NumberOfObjectsLoaded -= obj.Count;
            --db.PhysicalObjects;

            if (obj.serial == db.CurrentObject)
            {
                db.CurrentObjectExtracted = true;
                if (db.GlobalObjectCode == ReturnTypes.None)
                    db.GlobalObjectCode = ReturnTypes.ObjectExtracted;
            }
        }

        public static void extract_char(CharacterInstance ch, bool fPull)
        {
            if (ch == null)
            {
                LogManager.Bug("extract_char: null ch");
                return;
            }

            if (ch.CurrentRoom == null)
            {
                LogManager.Bug("extract_char: %s in null room", !string.IsNullOrEmpty(ch.Name) ? ch.Name : "???");
                return;
            }

            if (ch == db.Supermob)
            {
                LogManager.Bug("extract_char: ch == supermob");
                return;
            }

            if (char_died(ch))
            {
                LogManager.Bug("extract_char: %s already died!", ch.Name);
                return;
            }

            if (ch == db.CurrentChar)
                db.CurrentCharDied = true;

            queue_extracted_char(ch, fPull);

            foreach (RelationData relation in db.RELATIONS
                                                .Where(relation => fPull && relation.Types == RelationTypes.MSet_On))
            {
                if (ch == relation.Subject)
                    relation.Actor.CastAs<CharacterInstance>().DestinationBuffer = null;
                else if (ch != relation.Actor)
                    continue;

                db.RELATIONS.Remove(relation);
            }

            //trworld_char_check(ch);

            if (fPull)
                ch.DieFollower();

            fight.stop_fighting(ch, true);

            if (ch.CurrentMount != null)
            {
                reset.update_room_reset(ch, true);
                ch.CurrentMount.Act.RemoveBit((int)ActFlags.Mounted);
                ch.CurrentMount = null;
                ch.Position = PositionTypes.Standing;
            }

            if (ch.IsNpc())
            {
                ch.CurrentMount.Act.RemoveBit((int)ActFlags.Mounted);
                foreach (CharacterInstance wch in db.CHARACTERS.Where(wch => wch.CurrentMount == ch))
                {
                    wch.CurrentMount = null;
                    wch.Position = PositionTypes.Standing;
                    if (wch.CurrentRoom == ch.CurrentRoom)
                    {
                        comm.act(ATTypes.AT_SOCIAL, "Your faithful mount collapses beneath you...", wch, null, ch,
                                 ToTypes.Character);
                        comm.act(ATTypes.AT_SOCIAL, "Sadly you dismount $M for the last time.", wch, null, ch,
                                 ToTypes.Character);
                        comm.act(ATTypes.AT_PLAIN, "$n sadly dismounts $N for the last time.", wch, null, ch,
                                 ToTypes.Room);
                    }
                    if (wch.PlayerData != null && wch.PlayerData.Pet == ch)
                    {
                        wch.PlayerData.Pet = null;
                        if (wch.CurrentRoom == ch.CurrentRoom)
                            comm.act(ATTypes.AT_SOCIAL, "You mourn for the loss of $N.", wch, null, ch,
                                     ToTypes.Character);
                    }
                }
            }

            ObjectInstance lastObj = ch.Carrying.Last();
            if (lastObj != null)
                extract_obj(lastObj);

            ch.CurrentRoom.FromRoom(ch);

            if (!fPull)
            {
                RoomTemplate location = null;
                if (!ch.IsNpc() && ch.PlayerData.Clan != null)
                    location = db.get_room_index(ch.PlayerData.Clan.RecallRoom);

                if (location == null)
                    location = db.get_room_index(Program.ROOM_VNUM_ALTAR);

                if (location == null)
                    location = db.get_room_index(1);

                location.ToRoom(ch);

                CharacterInstance wch = get_char_room(ch, "healer");
                if (wch != null)
                {
                    comm.act(ATTypes.AT_MAGIC, "$n mutters a few incantations, waves $s hands and points $s finger.",
                             wch, null, null, ToTypes.Room);
                    comm.act(ATTypes.AT_MAGIC, "$n appears from some strange swilring mists!", ch, null, null,
                             ToTypes.Room);
                    Say.do_say(wch,
                               string.Format("Welcome back to the land of the living, {0}", ch.Name.CapitalizeFirst()));
                }
                else
                    comm.act(ATTypes.AT_MAGIC, "$n appears from some strange swirling mists!", ch, null, null,
                             ToTypes.Room);

                ch.Position = PositionTypes.Resting;
            }

            if (ch.IsNpc())
            {
                --ch.MobIndex.Count;
                --db.NumberOfMobsLoaded;
            }

            if (ch.Descriptor != null && ch.Descriptor.Original != null)
                Return.do_return(ch, "");

            if (ch.Switched != null && ch.Switched.Descriptor != null)
                Return.do_return(ch.Switched, "");

            foreach (CharacterInstance wch in db.CHARACTERS)
            {
                if (wch.ReplyTo == ch)
                    wch.ReplyTo = null;
                if (wch.RetellTo == ch)
                    wch.RetellTo = null;
            }

            db.CHARACTERS.Remove(ch);

            if (ch.Descriptor != null)
            {
                if (ch.Descriptor.Character != ch)
                    LogManager.Bug("extract_char: Char's descriptor points to another char");
                else
                {
                    ch.Descriptor.Character = null;
                    // TODO Close the socket
                    ch.Descriptor = null;
                }
            }
        }

        public static CharacterInstance get_char_room(CharacterInstance ch, string argument)
        {
            Tuple<int, string> tuple = argument.NumberArgument();
            int result = tuple.Item1;
            string arg = tuple.Item2;

            if (arg.Equals("self"))
                return ch;

            int vnum;
            if (ch.Trust >= Program.LEVEL_SAVIOR && arg.IsNumeric())
                vnum = Convert.ToInt32(arg);
            else
                vnum = -1;

            int count = 0;

            foreach (CharacterInstance rch in ch.CurrentRoom.Persons
                                            .Where(rch => can_see(ch, rch) && (arg.IsAnyEqual(rch.Name)
                                                                               ||
                                                                               (rch.IsNpc() && vnum == rch.MobIndex.Vnum)))
                )
            {
                if (result == 0 && !rch.IsNpc())
                    return rch;
                if (++count == result)
                    return rch;
            }

            if (vnum != -1)
                return null;

            count = 0;
            foreach (CharacterInstance rch in ch.CurrentRoom.Persons
                                            .Where(rch => can_see(ch, rch) && arg.IsAnyEqualPrefix(rch.Name)))
            {
                if (result == 0 && !rch.IsNpc())
                    return rch;
                if (++count == result)
                    return rch;
            }

            return null;
        }

        /// <summary>
        /// Find a character anywhere in the world
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static CharacterInstance get_char_world(CharacterInstance ch, string argument)
        {
            Tuple<int, string> result = argument.NumberArgument();
            int number = result.Item1;
            string arg = result.Item2;

            if (arg.EqualsIgnoreCase("self"))
                return ch;

            // Allow reference by vnum
            int vnum = (ch.Trust >= Program.LEVEL_SAVIOR && arg.IsNumber()) ? arg.ToInt32() : -1;

            int count = 0;

            // Check the room for an exact match
            foreach (CharacterInstance wch in ch.CurrentRoom.Persons
                                            .Where(wch => can_see(ch, wch) && arg.IsAnyEqual(wch.Name)
                                                          || (wch.IsNpc() && vnum == wch.MobIndex.Vnum)))
            {
                if (number == 0 && !wch.IsNpc())
                    return wch;
                if (++count == number)
                    return wch;
            }

            // Check the world for an exact match
            count = 0;
            foreach (CharacterInstance wch in db.CHARACTERS
                                            .Where(wch => can_see(ch, wch) && arg.IsAnyEqual(wch.Name)
                                                          || (wch.IsNpc() && vnum == wch.MobIndex.Vnum)))
            {
                if (number == 0 && !wch.IsNpc())
                    return wch;
                if (++count == number)
                    return wch;
            }

            // Bail if looking for an exact vnum match
            if (vnum != -1)
                return null;

            // If no exact match was found, check the room for a prefix match 
            // i.e. gu == guard
            count = 0;
            foreach (CharacterInstance wch in ch.CurrentRoom.Persons
                                            .Where(wch => can_see(ch, wch) && arg.IsAnyEqualPrefix(wch.Name)))
            {
                if (number == 0 && !wch.IsNpc())
                    return wch;
                if (++count == number)
                    return wch;
            }

            // If no prefix match was found in room, check the world
            count = 0;
            foreach (CharacterInstance wch in db.CHARACTERS
                                            .Where(wch => can_see(ch, wch) && arg.IsAnyEqualPrefix(wch.Name)))
            {
                if (number == 0 && !wch.IsNpc())
                    return wch;
                if (++count == number)
                    return wch;
            }

            return null;
        }

        /// <summary>
        /// Find an object in the given list
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="argumnet"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ObjectInstance get_obj_list(CharacterInstance ch, string argumnet, List<ObjectInstance> list)
        {
            Tuple<int, string> tuple = argumnet.NumberArgument();
            int number = tuple.Item1;
            string arg = tuple.Item2;

            int count = 0;
            foreach (ObjectInstance obj in list
                .Where(obj => can_see_obj(ch, obj)
                              && arg.IsAnyEqual(obj.Name)))
            {
                count += obj.Count;
                if (count >= number)
                    return obj;
            }

            // No exact match was found, look through the list again looking for prefix matching
            // i.e. swo == sword
            count = 0;
            foreach (ObjectInstance obj in list
                .Where(obj => can_see_obj(ch, obj)
                              && arg.IsAnyEqualPrefix(obj.Name)))
            {
                count += obj.Count;
                if (count >= number)
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// Find an object in the given list, traversing it backwards
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="argument"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ObjectInstance get_obj_list_rev(CharacterInstance ch, string argument, List<ObjectInstance> list)
        {
            List<ObjectInstance> reversedList = new List<ObjectInstance>();
            reversedList.AddRange(list);
            reversedList.Reverse();

            return get_obj_list(ch, argument, reversedList);
        }

        /// <summary>
        /// Find an object in player's inventory or wearing via a vnum
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="vnum"></param>
        /// <returns></returns>
        public static ObjectInstance get_obj_vnum(CharacterInstance ch, int vnum)
        {
            return ch.Carrying.FirstOrDefault(obj => can_see_obj(ch, obj)
                                                     && obj.ObjectIndex.Vnum == vnum);
        }

        public static ObjectInstance get_obj_carry(CharacterInstance ch, string argument)
        {
            Tuple<int, string> tuple = argument.NumberArgument();
            int number = tuple.Item1;
            string arg = tuple.Item2;

            int vnum = (ch.Trust >= Program.LEVEL_SAVIOR && arg.IsNumber()) ? arg.ToInt32() : -1;

            int count = 0;
            foreach (ObjectInstance obj in ch.Carrying
                                         .Where(obj => obj.WearLocation == WearLocations.None
                                                       && can_see_obj(ch, obj) && (arg.IsAnyEqual(obj.Name)
                                                                                   || obj.ObjectIndex.Vnum == vnum)))
            {
                count += obj.Count;
                if (count >= number)
                    return obj;
            }

            if (vnum != -1)
                return null;

            // if we didn't find an exact match, run through the list again for a partial match
            // i.e. swo == sword
            count = 0;
            foreach (ObjectInstance obj in ch.Carrying
                                         .Where(obj => obj.WearLocation == WearLocations.None
                                                       && can_see_obj(ch, obj) && arg.IsAnyEqualPrefix(obj.Name)))
            {
                count += obj.Count;
                if (count >= number)
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// Find an object in player's equipment
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static ObjectInstance get_obj_wear(CharacterInstance ch, string argument)
        {
            Tuple<int, string> tuple = argument.NumberArgument();
            int number = tuple.Item1;
            string arg = tuple.Item2;

            int vnum = (ch.Trust >= Program.LEVEL_SAVIOR && arg.IsNumber()) ? arg.ToInt32() : -1;

            int count = 0;
            foreach (ObjectInstance obj in ch.Carrying
                                         .Where(obj => obj.WearLocation != WearLocations.None
                                                       && can_see_obj(ch, obj) && (arg.IsAnyEqual(obj.Name)
                                                                                   || obj.ObjectIndex.Vnum == vnum)))
            {
                count += obj.Count;
                if (count >= number)
                    return obj;
            }

            if (vnum != -1)
                return null;

            // if we didn't find an exact match, run through the list again for a partial match
            // i.e. swo == sword
            count = 0;
            foreach (ObjectInstance obj in ch.Carrying
                                         .Where(obj => obj.WearLocation != WearLocations.None
                                                       && can_see_obj(ch, obj) && arg.IsAnyEqualPrefix(obj.Name)))
            {
                count += obj.Count;
                if (count >= number)
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// Find an object in the room or in inventory
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static ObjectInstance get_obj_here(CharacterInstance ch, string argument)
        {
            ObjectInstance obj = get_obj_list_rev(ch, argument, ch.CurrentRoom.Contents);
            if (obj != null)
                return obj;

            obj = get_obj_carry(ch, argument);
            if (obj != null)
                return obj;

            obj = get_obj_wear(ch, argument);
            if (obj != null)
                return obj;

            return null;
        }

        /// <summary>
        /// Find an object in the world
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        public static ObjectInstance get_obj_world(CharacterInstance ch, string argument)
        {
            Tuple<int, string> tuple = argument.NumberArgument();
            int number = tuple.Item1;
            string arg = tuple.Item2;

            int vnum = (ch.Trust >= Program.LEVEL_SAVIOR && arg.IsNumber()) ? arg.ToInt32() : -1;

            int count = 0;
            foreach (ObjectInstance obj in db.OBJECTS
                                         .Where(obj => can_see_obj(ch, obj) && (arg.IsAnyEqual(obj.Name)
                                                                                || obj.ObjectIndex.Vnum == vnum)))
            {
                count += obj.Count;
                if (count >= number)
                    return obj;
            }

            if (vnum != -1)
                return null;

            // if we didn't find an exact match, run through the list again for a partial match
            // i.e. swo == sword
            count = 0;
            foreach (ObjectInstance obj in db.OBJECTS
                                         .Where(obj => can_see_obj(ch, obj) && arg.IsAnyEqualPrefix(obj.Name)))
            {
                count += obj.Count;
                if (count >= number)
                    return obj;
            }

            return null;
        }

        private static readonly Dictionary<int, string> ObjectMessageLargeMap = new Dictionary<int, string>()
            {
                {1, "As you reach for it, you forget what it was...\r\n"},
                {2, "As you reach for it, something inside stops you...\r\n"}
            };

        private static readonly Dictionary<int, string> ObjectMessageSmallMap = new Dictionary<int, string>()
            {
                {1, "In just a second...\r\n"},
                {2, "You can't find that...\r\n"},
                {3, "It's just beyond your grasp...\r\n"},
                {4, "... but it's under a pile of other stuff...\r\n"},
                {5, "You go to reach for it, but pick your nose instead.\r\n"},
                {6, "Which one?!?  I see two... no three...\r\n"}
            };

        /// <summary>
        /// Mental state can affect finding an object, used by get/drop/put/quaff/etc.
        /// Gets increasingly "freaky" based on mental state and drunkenness
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool ms_find_obj(CharacterInstance ch)
        {
            int ms = ch.MentalState;

            // We're going to be nice and let nothing weird happen unless you're a tad messed up
            int drunk = Check.Maximum(1, ch.IsNpc() ? 0 : ch.PlayerData.ConditionTable[ConditionTypes.Drunk]);
            if (Math.Abs(ms) + (drunk / 3) < 30)
                return false;
            if ((SmaugRandom.Percent() + (ms < 0 ? 15 : 5)) > Math.Abs(ms) / 2 + drunk / 4)
                return false;

            string output = (ms > 15)
                ? ObjectMessageLargeMap[SmaugRandom.Between(Check.Maximum(1, (ms / 5 - 15)), (ms + 4) / 5)]
                : ObjectMessageSmallMap[SmaugRandom.Between(1, (Check.Range(1, Math.Abs(ms) / 2 + drunk, 60)) / 10)];

            color.send_to_char(output, ch);
            return true;
        }

        public static ObjectInstance find_obj(CharacterInstance ch, string argument, bool carryonly)
        {
            Tuple<string, string> tuple = argument.FirstArgument();
            string arg1 = tuple.Item1;
            tuple = tuple.Item2.FirstArgument();
            string arg2 = tuple.Item1;
            string remainder = tuple.Item2;

            if (arg2.EqualsIgnoreCase("from") && !string.IsNullOrEmpty(remainder))
            {
                tuple = remainder.FirstArgument();
                arg2 = tuple.Item1;
                remainder = tuple.Item2;
            }

            if (string.IsNullOrEmpty(arg2))
            {
                ObjectInstance obj = carryonly ? get_obj_carry(ch, arg1) : get_obj_here(ch, arg1);
                if (obj == null && carryonly)
                {
                    color.send_to_char("You do not have that item.\r\n", ch);
                    return null;
                }
                if (obj == null)
                {
                    comm.act(ATTypes.AT_PLAIN, "I see no $T here.", ch, null, arg1, ToTypes.Character);
                    return null;
                }

                return obj;
            }

            if (carryonly && get_obj_carry(ch, arg2) == null && get_obj_wear(ch, arg2) == null)
            {
                color.send_to_char("You do not have that item.\r\n", ch);
                return null;
            }
            if (!carryonly && get_obj_here(ch, arg2) == null)
            {
                comm.act(ATTypes.AT_PLAIN, "I see no $T here.", ch, null, arg2, ToTypes.Character);
                return null;
            }

            // TODO
            return null;
        }

        public static bool can_see(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static bool can_see_obj(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
            return false;
        }

        public static bool can_drop_obj(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
            return false;
        }

        public static string item_type_name(ObjectInstance obj)
        {
            // TODO
            return string.Empty;
        }

        public static string affect_loc_name(int location)
        {
            // TODO
            return string.Empty;
        }

        public static string affect_bit_name(ExtendedBitvector vector)
        {
            // TODO
            return string.Empty;
        }

        public static string extra_bit_name(ExtendedBitvector extra_flags)
        {
            // TODO
            return string.Empty;
        }

        public static string magic_bit_name(int magic_flags)
        {
            // TODO
            return string.Empty;
        }

        public static string pull_type_name(int pulltype)
        {
            // TODO
            return string.Empty;
        }

        public static int spring_trap(CharacterInstance ch, ObjectInstance obj)
        {
            // TODO
            return 0;
        }

        public static int check_for_trap(CharacterInstance ch, ObjectInstance obj, int flag)
        {
            // TODO
            return 0;
        }

        public static int check_room_for_traps(CharacterInstance ch, int flag)
        {
            // TODO
            return 0;
        }

        public static bool is_trapped(ObjectInstance obj)
        {
            // TODO
            return false;
        }

        public static ObjectInstance get_trap(ObjectInstance obj)
        {
            // TODO
            return null;
        }

        public static ObjectInstance get_objtype(CharacterInstance ch, short type)
        {
            // TODO
            return null;
        }

        public static void extract_exit(RoomTemplate room, ExitData pexit)
        {
            // TODO
        }

        public static void clean_room(RoomTemplate room)
        {
            // TODO
        }

        public static void clean_obj(ObjectTemplate obj)
        {
            // TODO
        }

        public static void clean_mob(MobTemplate mob)
        {
            // TODO
        }

        public static void clean_resets(RoomTemplate room)
        {
            // TODO
        }

        public static void name_stamp_stats(CharacterInstance ch)
        {
            // TODO
        }

        public static void fix_char(CharacterInstance ch)
        {
            // TODO
        }

        public static void showaffect(CharacterInstance ch, AffectData paf)
        {
            // TODO
        }

        public static void set_cur_obj(ObjectInstance obj)
        {
            // TODO
        }

        public static bool obj_extracted(ObjectInstance obj)
        {
            // TODO
            return false;
        }

        public static void queue_extracted_obj(ObjectInstance obj)
        {
            // TODO
        }

        public static void clean_obj_queue()
        {
            // TODO
        }

        public static void set_cur_char(CharacterInstance ch)
        {
            // TODO
        }

        public static bool char_died(CharacterInstance ch)
        {
            // TODO
            return false;
        }

        public static void queue_extracted_char(CharacterInstance ch, bool extract)
        {
            // TODO
        }

        public static void clean_char_queue()
        {
            // TODO
        }

        public static void add_timer(CharacterInstance ch, short type, int count, Action<CharacterInstance, string> fun, int value)
        {
            // TODO
        }

        public static TimerData get_timerptr(CharacterInstance ch, TimerTypes type)
        {
            // TODO
            return null;
        }

        public static short get_timer(CharacterInstance ch, short type)
        {
            // TODO
            return 0;
        }

        public static void extract_timer(CharacterInstance ch, TimerData timer)
        {
            // TODO
        }

        public static void remove_timer(CharacterInstance ch, short type)
        {
            // TODO
        }

        public static bool in_soft_rangE(CharacterInstance ch, AreaData tarea)
        {
            // TODO
            return false;
        }

        public static bool can_astral(CharacterInstance ch, CharacterInstance victim)
        {
            // TODO
            return false;
        }

        public static bool in_hard_range(CharacterInstance ch, AreaData tarea)
        {
            // TODO
            return false;
        }

        public static bool chance(CharacterInstance ch, short percent)
        {
            // TODO
            return false;
        }

        public static bool chance_attrib(CharacterInstance ch, short percent, short attrib)
        {
            // TODO
            return false;
        }

        public static ObjectInstance clone_object(ObjectInstance obj)
        {
            // TODO
            return null;
        }

        public static ObjectInstance group_object(ObjectInstance obj1, ObjectInstance obj2)
        {
            // TODO
            return null;
        }

        public static void split_obj(ObjectInstance obj, int num)
        {
            // TODO
        }

        public static void separate_obj(ObjectInstance obj)
        {
            split_obj(obj, 1);
        }

        public static bool empty_obj(ObjectInstance obj, ObjectInstance destobj, RoomTemplate destroom)
        {
            // TODO
            return false;
        }

        public static void better_mental_state(CharacterInstance ch, int mod)
        {
            // TODO
        }

        public static void worsen_mental_state(CharacterInstance ch, int mod)
        {
            // TODO
        }

        public static void economize_mobgold(CharacterInstance mob)
        {
            // TODO
        }

        public static void add_kill(CharacterInstance ch, CharacterInstance mob)
        {
            // TODO
        }

        public static int times_killed(CharacterInstance ch, CharacterInstance mob)
        {
            // TODO
            return 0;
        }

        public static AreaData get_area(string name)
        {
            // TODO
            return null;
        }

        public static AreaData get_area_obj(ObjectTemplate pObjIndex)
        {
            // TODO
            return null;
        }

        public static void check_switches(bool possess)
        {
            // TODO
        }

        public static void check_switch(CharacterInstance ch, bool possess)
        {
            // TODO
        }
    }
}
