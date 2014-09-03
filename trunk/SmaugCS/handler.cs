using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Auction;
using SmaugCS.Commands.Admin;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Managers;
using SmaugCS.Objects;
using SmaugCS.Repositories;

namespace SmaugCS
{
    public static class handler
    {
        public static ReturnTypes GlobalReturnCode { get; set; }
        public static ReturnTypes GlobalObjectCode { get; set; }

        public static CharacterInstance SavingCharacter { get; set; }
        public static CharacterInstance LoadingCharacter { get; set; }

        public static CharacterInstance CurrentCharacter { get; set; }
        public static CharacterInstance CurrentDeadCharacter { get; set; }
        public static bool CurrentCharacterDied { get; set; }
        public static RoomTemplate CurrentRoom { get; set; }
        public static ObjectInstance CurrentObject { get; set; }
        public static bool CurrentObjectExtracted { get; set; }

        public static Queue<ObjectInstance> ExtractedObjectQueue { get; set; }
        public static Queue<ExtracedCharacterData> ExtractedCharacterQueue { get; set; }
 
        public static int falling = 0;

        public static void extract_obj(ObjectInstance obj)
        {
            if (obj_extracted(obj))
                throw new ObjectAlreadyExtractedException("Object {0}", obj.ObjectIndex.ID);

            if (obj.ItemType == ItemTypes.Portal)
                update.remove_portal(obj);

            if (AuctionManager.Instance.Auction.ItemForSale == obj)
                Commands.Objects.Auction.StopAuction(AuctionManager.Instance.Auction.Seller,
                    "Sale of {0} has been stopped by a system action.");

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

            DatabaseManager.Instance.OBJECTS.CastAs<Repository<long, ObjectInstance>>().Delete(obj.ID);

            queue_extracted_obj(obj);

            obj.ObjectIndex.count -= obj.Count;
            db.NumberOfObjectsLoaded -= obj.Count;
            --db.PhysicalObjects;

            if (obj == CurrentObject)
            {
                CurrentObjectExtracted = true;
                if (GlobalObjectCode == ReturnTypes.None)
                    GlobalObjectCode = ReturnTypes.ObjectExtracted;
            }
        }

        public static void extract_char(CharacterInstance ch, bool fPull)
        {
            if (ch == null) return;
            if (ch.CurrentRoom == null) return;
            if (ch == db.Supermob) return;
            if (ch.CharDied()) return;

            if (ch == CurrentCharacter)
                CurrentCharacterDied = true;

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

            ch.StopFighting(true);

            if (ch.CurrentMount != null)
            {
                reset.update_room_reset(ch, true);
                ch.CurrentMount.Act.RemoveBit(ActFlags.Mounted);
                ch.CurrentMount = null;
                ch.CurrentPosition = PositionTypes.Standing;
            }

            if (ch.IsNpc())
            {
                ch.CurrentMount.Act.RemoveBit(ActFlags.Mounted);
                foreach (
                    CharacterInstance wch in
                        DatabaseManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>()
                            .Values.Where(wch => wch.CurrentMount == ch))
                {
                    wch.CurrentMount = null;
                    wch.CurrentPosition = PositionTypes.Standing;
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
                    location = DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(ch.PlayerData.Clan.RecallRoom);

                if (location == null)
                    location = DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(VnumConstants.ROOM_VNUM_ALTAR);

                if (location == null)
                    location = DatabaseManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(1);

                location.ToRoom(ch);

                CharacterInstance wch = ch.GetCharacterInRoom("healer");
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

                ch.CurrentPosition = PositionTypes.Resting;
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

            foreach (CharacterInstance wch in DatabaseManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values)
            {
                if (wch.ReplyTo == ch)
                    wch.ReplyTo = null;
                if (wch.RetellTo == ch)
                    wch.RetellTo = null;
            }

            DatabaseManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Delete(ch.ID);

            if (ch.Descriptor != null)
            {
                if (ch.Descriptor.Character == ch)
                {
                    ch.Descriptor.Character = null;
                    // TODO Close the socket
                    ch.Descriptor = null;
                }
            }
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
        public static bool FindObject_CheckMentalState(CharacterInstance ch)
        {
            int ms = ch.MentalState;

            // We're going to be nice and let nothing weird happen unless you're a tad messed up
            int drunk = 1.GetHighestOfTwoNumbers(ch.IsNpc() ? 0 : ch.PlayerData.ConditionTable[ConditionTypes.Drunk]);
            if (Math.Abs(ms) + (drunk / 3) < 30)
                return false;
            if ((SmaugRandom.D100() + (ms < 0 ? 15 : 5)) > Math.Abs(ms) / 2 + drunk / 4)
                return false;

            string output = (ms > 15)
                ? ObjectMessageLargeMap[SmaugRandom.Between(1.GetHighestOfTwoNumbers(ms / 5 - 15), (ms + 4) / 5)]
                : ObjectMessageSmallMap[SmaugRandom.Between(1, ((Math.Abs(ms) / 2 + drunk).GetNumberThatIsBetween(1, 60)) / 10)];

            color.send_to_char(output, ch);
            return true;
        }

        public static ObjectInstance FindObject(CharacterInstance ch, string argument, bool carryonly)
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

            ObjectInstance obj;
            if (string.IsNullOrEmpty(arg2))
            {
                obj = carryonly ? ch.GetCarriedObject(arg1) : ch.GetObjectOnMeOrInRoom(arg1);
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

            ObjectInstance container = null;
            if (carryonly &&  (container = ch.GetCarriedObject(arg2)) == null && (container = ch.GetWornObject(arg2)) == null)
            {
                color.send_to_char("You do not have that item.\r\n", ch);
                return null;
            }
            if (!carryonly && (container = ch.GetObjectOnMeOrInRoom(arg2)) == null)
            {
                comm.act(ATTypes.AT_PLAIN, "I see no $T here.", ch, null, arg2, ToTypes.Character);
                return null;
            }

            if (!container.ExtraFlags.IsSet(ItemExtraFlags.Covering) && container.Value[1].IsSet(ContainerFlags.Closed))
            {
                comm.act(ATTypes.AT_PLAIN, "The $d is closed.", ch, null, container.Name, ToTypes.Character);
                return null;
            }

            obj = ch.GetObjectInList(container.Contents, arg1);
            if (obj == null)
                comm.act(ATTypes.AT_PLAIN, container.ExtraFlags.IsSet(ItemExtraFlags.Covering)
                    ? "I see nothing like that beneath $p."
                    : "I see nothing like that in $p.", ch, container, null, ToTypes.Character);
            
            return obj;
        }

        public static string affect_loc_name(int location)
        {
            ApplyTypes type = Realm.Library.Common.EnumerationExtensions.GetEnum<ApplyTypes>(location);
            return type.GetName();
        }

        public static string affect_bit_name(ExtendedBitvector vector)
        {
            StringBuilder sb = new StringBuilder();
            foreach (AffectedByTypes type in Enum.GetValues(typeof (AffectedByTypes))
                        .Cast<AffectedByTypes>()
                        .Where(type => vector.IsSet((int) type)))
            {
                sb.AppendFormat(" {0}", type.GetName());
            }
            return sb.ToString();
        }

        public static string extra_bit_name(ExtendedBitvector extra_flags)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ItemExtraFlags type in Enum.GetValues(typeof (ItemExtraFlags))
                        .Cast<ItemExtraFlags>()
                        .Where(type => extra_flags.IsSet((int) type)))
            {
                sb.AppendFormat(" {0}", type.GetName());
            }
            return sb.ToString();
        }

        public static string magic_bit_name(int magic_flags)
        {
            return (magic_flags & (int) ItemMagicFlags.Returning) > 0 ? " returning" : "none";
        }

        public static string pull_type_name(int pulltype)
        {
            foreach (DirectionPullTypes type in Enum.GetValues(typeof (DirectionPullTypes)))
            {
                if ((int) type == pulltype || pulltype == type.GetValue())
                    return type.GetName();
            }
            return "ERROR";
        }

        public static ObjectInstance get_trap(ObjectInstance obj)
        {
            if (!obj.IsTrapped())
                return null;
            return obj.Contents.FirstOrDefault(check => check.ItemType == ItemTypes.Trap);
        }

        public static ObjectInstance get_objtype(CharacterInstance ch, int type)
        {
            return ch.Carrying.FirstOrDefault(obj => (int) obj.ItemType == type);
        }

        public static void extract_exit(RoomTemplate room, ExitData pexit)
        {
            room.Exits.Remove(pexit);
            ExitData rexit = pexit.GetReverseExit();
            rexit.Reverse = 0;
        }

        public static void name_stamp_stats(CharacterInstance ch)
        {
            ch.PermanentStrength = 18.GetLowestOfTwoNumbers(ch.PermanentStrength);
            ch.PermanentWisdom = 18.GetLowestOfTwoNumbers(ch.PermanentWisdom);
            ch.PermanentDexterity = 18.GetLowestOfTwoNumbers(ch.PermanentDexterity);
            ch.PermanentIntelligence = 18.GetLowestOfTwoNumbers(ch.PermanentIntelligence);
            ch.PermanentConstitution = 18.GetLowestOfTwoNumbers(ch.PermanentConstitution);
            ch.PermanentCharisma = 18.GetLowestOfTwoNumbers(ch.PermanentCharisma);
            ch.PermanentLuck = 18.GetLowestOfTwoNumbers(ch.PermanentLuck);
            ch.PermanentStrength = 9.GetHighestOfTwoNumbers(ch.PermanentStrength);
            ch.PermanentWisdom = 9.GetHighestOfTwoNumbers(ch.PermanentWisdom);
            ch.PermanentDexterity = 9.GetHighestOfTwoNumbers(ch.PermanentDexterity);
            ch.PermanentIntelligence = 9.GetHighestOfTwoNumbers(ch.PermanentIntelligence);
            ch.PermanentConstitution = 9.GetHighestOfTwoNumbers(ch.PermanentConstitution);
            ch.PermanentCharisma = 9.GetHighestOfTwoNumbers(ch.PermanentCharisma);
            ch.PermanentLuck = 9.GetHighestOfTwoNumbers(ch.PermanentLuck);

            foreach (var c in ch.Name)
            {
                int b = c%14;
                int a = (c%1) + 1;

                switch (b)
                {
                    case 0:
                        ch.PermanentStrength = 18.GetLowestOfTwoNumbers(ch.PermanentStrength + a);
                        break;
                    case 1:
                        ch.PermanentDexterity = 18.GetLowestOfTwoNumbers(ch.PermanentDexterity + a);
                        break;
                    case 2:
                        ch.PermanentWisdom = 18.GetLowestOfTwoNumbers(ch.PermanentWisdom + a);
                        break;
                    case 3:
                        ch.PermanentIntelligence = 18.GetLowestOfTwoNumbers(ch.PermanentIntelligence + a);
                        break;
                    case 4:
                        ch.PermanentConstitution = 18.GetLowestOfTwoNumbers(ch.PermanentConstitution + a);
                        break;
                    case 5:
                        ch.PermanentCharisma = 18.GetLowestOfTwoNumbers(ch.PermanentCharisma + a);
                        break;
                    case 6:
                        ch.PermanentLuck = 9.GetLowestOfTwoNumbers(ch.PermanentLuck + a);
                        break;
                    case 7:
                        ch.PermanentStrength = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
                        break;
                    case 8:
                        ch.PermanentDexterity = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
                        break;
                    case 9:
                        ch.PermanentWisdom = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
                        break;
                    case 10:
                        ch.PermanentIntelligence = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
                        break;
                    case 11:
                        ch.PermanentConstitution = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
                        break;
                    case 12:
                        ch.PermanentCharisma = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
                        break;
                    case 13:
                        ch.PermanentLuck = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
                        break;
                }
            }
        }

        public static void fix_char(CharacterInstance ch)
        {
            save.de_equip_char(ch);

            foreach(AffectData af in ch.Affects)
                ch.ModifyAffect(af, false);

            if (ch.CurrentRoom != null)
            {
                foreach (AffectData af in ch.CurrentRoom.Affects)
                {
                    if (af.Location != ApplyTypes.WearSpell
                        && af.Location != ApplyTypes.RemoveSpell
                        && af.Location != ApplyTypes.StripSN)
                        ch.ModifyAffect(af, false);
                }

                foreach (AffectData af in ch.CurrentRoom.PermanentAffects)
                {
                    if (af.Location != ApplyTypes.WearSpell
                        && af.Location != ApplyTypes.RemoveSpell
                        && af.Location != ApplyTypes.StripSN)
                        ch.ModifyAffect(af, false);
                }
            }

            ch.AffectedBy = 0;

            RaceData race = DatabaseManager.Instance.GetRace(ch.CurrentRace);
            ch.AffectedBy.SetBit(race.AffectedBy);
            ch.MentalState = -10;
            ch.CurrentHealth = 1.GetHighestOfTwoNumbers(ch.CurrentHealth);
            ch.CurrentMana = 1.GetHighestOfTwoNumbers(ch.CurrentMana);
            ch.CurrentMovement = 1.GetHighestOfTwoNumbers(ch.CurrentMovement);
            ch.ArmorClass = 100;
            ch.ModStrength =
                ch.ModDexterity =
                    ch.ModWisdom = ch.ModIntelligence = ch.ModConstitution = ch.ModCharisma = ch.ModLuck = 0;
            ch.DamageRoll = new DiceData();
            ch.HitRoll = new DiceData();
            ch.CurrentAlignment = -1000.GetNumberThatIsBetween(ch.CurrentAlignment, 1000);
            ch.SavingThrows = new SavingThrowData();

            foreach (AffectData af in ch.Affects)
                ch.ModifyAffect(af, true);

            if (ch.CurrentRoom != null)
            {
                foreach (AffectData af in ch.CurrentRoom.Affects)
                {
                    if (af.Location != ApplyTypes.WearSpell
                        && af.Location != ApplyTypes.RemoveSpell
                        && af.Location != ApplyTypes.StripSN)
                        ch.ModifyAffect(af, true);
                }

                foreach (AffectData af in ch.CurrentRoom.PermanentAffects)
                {
                    if (af.Location != ApplyTypes.WearSpell
                        && af.Location != ApplyTypes.RemoveSpell
                        && af.Location != ApplyTypes.StripSN)
                        ch.ModifyAffect(af, true);
                }
            }

            ch.CarryWeight = ch.CarryNumber = 0;

            foreach (ObjectInstance obj in ch.Carrying)
            {
                if (obj.WearLocation == WearLocations.None)
                    ch.CarryNumber += obj.GetObjectNumber();
                if (!obj.ExtraFlags.IsSet(ItemExtraFlags.Magical))
                    ch.CarryWeight += obj.GetObjectWeight();
            }

            save.re_equip_char(ch);
        }

        public static void showaffect(CharacterInstance ch, AffectData paf)
        {
            Validation.IsNotNull(paf, "paf");

            string buf = string.Empty;

            if (paf.Location == ApplyTypes.None || paf.Modifier == 0)
                return;

            switch (paf.Location)
            {
                default:
                    buf = string.Format("Affects {0} by {1}.", affect_loc_name((int)paf.Location), paf.Modifier);
                    break;
                case ApplyTypes.Affect:
                    buf = string.Format("Affects {0} by", affect_loc_name((int)paf.Location), paf.Modifier);

                    for (int i = 0; i < 32; i++)
                    {
                        if (paf.Modifier.IsSet(1 << i))
                            buf += " " + BuilderConstants.a_flags[i];
                    }
                    break;
                case ApplyTypes.WeaponSpell:
                case ApplyTypes.WearSpell:
                case ApplyTypes.RemoveSpell:
                    string name = "unknown";
                    if (Macros.IS_VALID_SN(paf.Modifier))
                    {
                        SkillData skill = DatabaseManager.Instance.SKILLS.Get(paf.Modifier);
                        name = skill.Name;
                    }
                    buf = string.Format("Casts spell '{0}'", name);
                    break;
                case ApplyTypes.Resistance:
                case ApplyTypes.Immunity:
                case ApplyTypes.Susceptibility:
                    buf = string.Format("Affects {0} by", affect_loc_name((int)paf.Location), paf.Modifier);

                    for (int i = 0; i < 32; i++)
                    {
                        if (paf.Modifier.IsSet(1 << i))
                            buf += " " + BuilderConstants.ris_flags[i];
                    }
                    
                    break;
            }

            color.send_to_char(buf, ch);            
        }

        public static void set_cur_obj(ObjectInstance obj)
        {
            CurrentObject = obj;
            CurrentObjectExtracted = false;
            GlobalObjectCode = ReturnTypes.None;
        }

        public static bool obj_extracted(ObjectInstance obj)
        {
            if (obj == CurrentObject && CurrentObjectExtracted)
                return true;

            return ExtractedObjectQueue.Any(extractObj => obj == extractObj);
        }

        public static void queue_extracted_obj(ObjectInstance obj)
        {
            ExtractedObjectQueue.Enqueue(obj);
        }

        public static void clean_obj_queue()
        {
            ExtractedObjectQueue.Clear();
        }

        public static void set_cur_char(CharacterInstance ch)
        {
            CurrentCharacter = ch;
            CurrentCharacterDied = false;
            CurrentRoom = ch.CurrentRoom;
            GlobalReturnCode = ReturnTypes.None;
        }

        public static void queue_extracted_char(CharacterInstance ch, bool extract)
        {
            if (ch == null)
                throw new ArgumentNullException("ch");

            ExtracedCharacterData ecd = new ExtracedCharacterData
            {
                Character = ch,
                Room = ch.CurrentRoom,
                Extract = extract,
                ReturnCode = ch == CurrentCharacter ? GlobalReturnCode : ReturnTypes.CharacterDied
            };

            ExtractedCharacterQueue.Enqueue(ecd);
        }

        public static void clean_char_queue()
        {
            ExtractedCharacterQueue.Clear();
        }

        public static bool chance_attrib(CharacterInstance ch, short percent, short attrib)
        {
            return (SmaugRandom.D100() - ch.GetCurrentLuck() + 13 - attrib + 13 +
                    (ch.IsDevoted() ? ch.PlayerData.Favor/-500 : 0) <= percent);
        }

        public static ObjectInstance clone_object(ObjectInstance obj)
        {
            ObjInstanceRepository repo = (ObjInstanceRepository)Program.Kernel.Get<IInstanceRepository<ObjectInstance>>();
            return repo.Clone(obj);
        }

        public static ObjectInstance group_object(ObjectInstance obj1, ObjectInstance obj2)
        {
            if (obj1 == null || obj2 == null)
                return null;
            if (obj1 == obj2)
                return obj1;

            if (obj1.ObjectIndex == obj2.ObjectIndex
                && obj1.Name.EqualsIgnoreCase(obj2.Name)
                && obj1.ShortDescription.EqualsIgnoreCase(obj2.ShortDescription)
                && obj1.Description.EqualsIgnoreCase(obj2.Description)
                && obj1.Owner.EqualsIgnoreCase(obj2.Owner)
                && obj1.ItemType == obj2.ItemType
                //&& obj1.ExtraFlags.SameBits(obj2.ExtraFlags)
                && obj1.MagicFlags == obj2.MagicFlags
                && obj1.WearFlags == obj2.WearFlags
                && obj1.WearLocation == obj2.WearLocation
                && obj1.Weight == obj2.Weight
                && obj1.Cost == obj2.Cost
                && obj1.Level == obj2.Level
                && obj1.Timer == obj2.Timer
                && obj1.Value[0] == obj2.Value[0]
                && obj1.Value[1] == obj2.Value[1]
                && obj1.Value[2] == obj2.Value[2]
                && obj1.Value[3] == obj2.Value[3]
                && obj1.Value[4] == obj2.Value[4]
                && obj1.Value[5] == obj2.Value[5]
                && obj1.ExtraDescriptions.SequenceEqual(obj2.ExtraDescriptions)
                && obj1.Affects.SequenceEqual(obj2.Affects)
                && obj1.Contents.SequenceEqual(obj2.Contents)
                && obj1.Count + obj2.Count > 0)
            {
                obj1.Count += obj2.Count;
                obj1.ObjectIndex.count += obj2.Count;
                extract_obj(obj2);
                return obj1;
            }
            return obj2;
        }

        public static void split_obj(ObjectInstance obj, int num)
        {
            int count = obj.Count;
            if (count <= num || num == 0)
                return;

            ObjectInstance rest = clone_object(obj);
            --obj.ObjectIndex.count;
            rest.Count = obj.Count - num;
            obj.Count = num;

            if (obj.CarriedBy != null)
            {
                obj.CarriedBy.Carrying.Add(rest);
                rest.CarriedBy = obj.CarriedBy;
                rest.InRoom = null;
                rest.InObject = null;
            }
            else if (obj.InRoom != null)
            {
                obj.InRoom.Contents.Add(rest);
                rest.CarriedBy = null;
                rest.InRoom = obj.InRoom;
                rest.InObject = null;
            }
            else if (obj.InObject != null)
            {
                obj.InObject.Contents.Add(rest);
                rest.InObject = obj.InObject;
                rest.InRoom = null;
                rest.CarriedBy = null;
            }
        }

        public static void separate_obj(ObjectInstance obj)
        {
            split_obj(obj, 1);
        }

        public static bool empty_obj(ObjectInstance obj, ObjectInstance destobj, RoomTemplate destroom)
        {
            Validation.IsNotNull(obj, "obj");

            CharacterInstance ch = obj.CarriedBy;

            if (destobj != null)
                return EmptyIntoObject(ch, obj, destobj);

            if (destroom != null)
                return EmptyIntoRoom(ch, obj, destroom);

            if (obj.InObject != null)
                return EmptyIntoObject(ch, obj, obj.InObject);

            if (ch != null)
            {
                bool retVal = false;
                foreach (ObjectInstance cobj in obj.Contents)
                {
                    cobj.FromObject(cobj);
                    cobj.ToCharacter(ch);
                    retVal = true;
                }
                return retVal;
            }

            throw new InvalidOperationException(
                string.Format("Nothing specified to empty the contents of object {0} into", obj.ID));
        }

        private static bool EmptyIntoObject(CharacterInstance ch, ObjectInstance obj, ObjectInstance destobj)
        {
            bool retVal = false;
            foreach (ObjectInstance cobj in obj.Contents)
            {
                if (destobj.ItemType == ItemTypes.KeyRing && cobj.ItemType != ItemTypes.Key)
                    continue;
                if (destobj.ItemType == ItemTypes.Quiver && cobj.ItemType != ItemTypes.Projectile)
                    continue;
                if ((destobj.ItemType == ItemTypes.Container
                     || destobj.ItemType == ItemTypes.KeyRing
                     || destobj.ItemType == ItemTypes.Quiver)
                    && (cobj.GetRealObjectWeight() + destobj.GetRealObjectWeight() > destobj.Value[0]))
                    continue;

                cobj.FromObject(cobj);
                destobj.ToObject(cobj);
                retVal = true;
            }
            return retVal;
        }

        private static bool EmptyIntoRoom(CharacterInstance ch, ObjectInstance obj, RoomTemplate destroom)
        {
            bool retVal = false;
            foreach (ObjectInstance cobj in obj.Contents)
            {
                if (ch != null && cobj.ObjectIndex.HasProg(MudProgTypes.Drop) && cobj.Count > 1)
                {
                    separate_obj(cobj);
                    cobj.FromObject(cobj);
                }
                else 
                    cobj.FromObject(cobj);

                ObjectInstance tObj = destroom.ToRoom(cobj);

                if (ch != null)
                {
                    mud_prog.oprog_drop_trigger(ch, tObj);
                    if (ch.CharDied())
                        ch = null;
                }
                retVal = true;
            }
            return retVal;
        }

        public static void economize_mobgold(CharacterInstance mob)
        {
            mob.CurrentCoin = mob.CurrentCoin.GetLowestOfTwoNumbers(mob.Level*mob.Level*400);
            if (mob.CurrentRoom == null)
                return;

            int gold = (mob.CurrentRoom.Area.HighEconomy > 0 ? 1 : 0)*1000000000*mob.CurrentRoom.Area.LowEconomy;
            mob.CurrentCoin = 0.GetNumberThatIsBetween(mob.CurrentCoin, gold/10);
            if (mob.CurrentCoin > 0)
                mob.CurrentRoom.Area.LowerEconomy(mob.CurrentCoin);
        }

        public static void check_switches(bool possess)
        {
            foreach(CharacterInstance ch in DatabaseManager.Instance.CHARACTERS.Values)
                check_switch(ch, possess);
        }

        public static void check_switch(CharacterInstance ch, bool possess)
        {
            if (ch.Switched == null) return;
            if (!possess)
            {
                foreach (AffectData af in ch.Switched.Affects)
                {
                    if (af.Duration == -1)
                        continue;

                    SkillData skill = DatabaseManager.Instance.SKILLS.Get((int) af.Type);
                    if (af.Type != AffectedByTypes.None && skill != null &&
                        skill.SpellFunction.Value == Spells.Possess.spell_possess)
                        return;
                }
            }

            foreach (CommandData cmd in DatabaseManager.Instance.COMMANDS.Values)
            {
                if (cmd.DoFunction.Value != Switch.do_switch)
                    continue;
                if (cmd.Level <= ch.Trust)
                    return;
                if (!ch.IsNpc() && cmd.Name.IsAnyEqual(ch.PlayerData.bestowments)
                    && cmd.Level <= ch.Trust + GameManager.Instance.SystemData.BestowDifference)
                    return;
            }

            if (!possess)
            {
                color.set_char_color(ATTypes.AT_BLUE, ch.Switched);
                color.send_to_char("You suddenly forfeit the power to switch!", ch.Switched);
            }

            Return.do_return(ch.Switched, string.Empty);
        }
    }
}
