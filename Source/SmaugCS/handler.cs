using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common;
using SmaugCS.Commands.Admin;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Helpers;
using SmaugCS.Managers;
using SmaugCS.Objects;
using SmaugCS.Spells;
using EnumerationExtensions = Realm.Library.Common.EnumerationExtensions;

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
            var ms = ch.MentalState;

            // We're going to be nice and let nothing weird happen unless you're a tad messed up
            var drunk = 1.GetHighestOfTwoNumbers(ch.IsNpc() ? 0 : ((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Drunk]);
            if (Math.Abs(ms) + (drunk / 3) < 30)
                return false;
            if ((SmaugRandom.D100() + (ms < 0 ? 15 : 5)) > Math.Abs(ms) / 2 + drunk / 4)
                return false;

            var output = (ms > 15)
                ? ObjectMessageLargeMap[SmaugRandom.Between(1.GetHighestOfTwoNumbers(ms / 5 - 15), (ms + 4) / 5)]
                : ObjectMessageSmallMap[SmaugRandom.Between(1, ((Math.Abs(ms) / 2 + drunk).GetNumberThatIsBetween(1, 60)) / 10)];

            ch.SendTo(output);
            return true;
        }

        public static ObjectInstance FindObject(CharacterInstance ch, string argument, bool carryonly)
        {
            var tuple = argument.FirstArgument();
            var arg1 = tuple.Item1;
            tuple = tuple.Item2.FirstArgument();
            var arg2 = tuple.Item1;
            var remainder = tuple.Item2;

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
                if (CheckFunctions.CheckIfTrue(ch, obj == null && carryonly, "You do not have that item.")) return null;

                if (obj == null)
                {
                    comm.act(ATTypes.AT_PLAIN, "I see no $T here.", ch, null, arg1, ToTypes.Character);
                    return null;
                }

                return obj;
            }

            ObjectInstance container = null;
            if (CheckFunctions.CheckIfTrue(ch,
                carryonly && (container = ch.GetCarriedObject(arg2)) == null &&
                (container = ch.GetWornObject(arg2)) == null, "You do not have that item.")) return null;

            if (!carryonly && (container = ch.GetObjectOnMeOrInRoom(arg2)) == null)
            {
                comm.act(ATTypes.AT_PLAIN, "I see no $T here.", ch, null, arg2, ToTypes.Character);
                return null;
            }

            if (!container.ExtraFlags.IsSet(ItemExtraFlags.Covering) && container.Value.ToList()[1].IsSet(ContainerFlags.Closed))
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
            var type = EnumerationExtensions.GetEnum<ApplyTypes>(location);
            return type.GetName();
        }

        public static string affect_bit_name(ExtendedBitvector vector)
        {
            var sb = new StringBuilder();
            foreach (var type in Enum.GetValues(typeof (AffectedByTypes))
                        .Cast<AffectedByTypes>()
                        .Where(type => vector.IsSet((int) type)))
            {
                sb.AppendFormat(" {0}", type.GetName());
            }
            return sb.ToString();
        }

        public static string extra_bit_name(ExtendedBitvector extra_flags)
        {
            var sb = new StringBuilder();
            foreach (var type in Enum.GetValues(typeof (ItemExtraFlags))
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
                var b = c%14;
                var a = (c%1) + 1;

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

            foreach(var af in ch.Affects)
                ch.ModifyAffect(af, false);

            if (ch.CurrentRoom != null)
            {
                foreach (var af in ch.CurrentRoom.Affects)
                {
                    if (af.Location != ApplyTypes.WearSpell
                        && af.Location != ApplyTypes.RemoveSpell
                        && af.Location != ApplyTypes.StripSN)
                        ch.ModifyAffect(af, false);
                }

                foreach (var af in ch.CurrentRoom.PermanentAffects)
                {
                    if (af.Location != ApplyTypes.WearSpell
                        && af.Location != ApplyTypes.RemoveSpell
                        && af.Location != ApplyTypes.StripSN)
                        ch.ModifyAffect(af, false);
                }
            }

            ch.AffectedBy = 0;

            var race = DatabaseManager.Instance.GetRace(ch.CurrentRace);
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

            foreach (var af in ch.Affects)
                ch.ModifyAffect(af, true);

            if (ch.CurrentRoom != null)
            {
                foreach (var af in ch.CurrentRoom.Affects)
                {
                    if (af.Location != ApplyTypes.WearSpell
                        && af.Location != ApplyTypes.RemoveSpell
                        && af.Location != ApplyTypes.StripSN)
                        ch.ModifyAffect(af, true);
                }

                foreach (var af in ch.CurrentRoom.PermanentAffects)
                {
                    if (af.Location != ApplyTypes.WearSpell
                        && af.Location != ApplyTypes.RemoveSpell
                        && af.Location != ApplyTypes.StripSN)
                        ch.ModifyAffect(af, true);
                }
            }

            ch.CarryWeight = ch.CarryNumber = 0;

            foreach (var obj in ch.Carrying)
            {
                if (obj.WearLocation == WearLocations.None)
                    ch.CarryNumber += obj.ObjectNumber;
                if (!obj.ExtraFlags.IsSet(ItemExtraFlags.Magical))
                    ch.CarryWeight += obj.GetWeight();
            }

            save.re_equip_char(ch);
        }

        public static void showaffect(CharacterInstance ch, AffectData paf)
        {
            Validation.IsNotNull(paf, "paf");

            var buf = string.Empty;

            if (paf.Location == ApplyTypes.None || paf.Modifier == 0)
                return;

            switch (paf.Location)
            {
                default:
                    buf = string.Format("Affects {0} by {1}.", affect_loc_name((int)paf.Location), paf.Modifier);
                    break;
                case ApplyTypes.Affect:
                    buf = string.Format("Affects {0} by", affect_loc_name((int)paf.Location), paf.Modifier);

                    for (var i = 0; i < 32; i++)
                    {
                        if (paf.Modifier.IsSet(1 << i))
                            buf += " " + BuilderConstants.a_flags[i];
                    }
                    break;
                case ApplyTypes.WeaponSpell:
                case ApplyTypes.WearSpell:
                case ApplyTypes.RemoveSpell:
                    var name = "unknown";
                    if (Macros.IS_VALID_SN(paf.Modifier))
                    {
                        var skill = DatabaseManager.Instance.SKILLS.Get(paf.Modifier);
                        name = skill.Name;
                    }
                    buf = string.Format("Casts spell '{0}'", name);
                    break;
                case ApplyTypes.Resistance:
                case ApplyTypes.Immunity:
                case ApplyTypes.Susceptibility:
                    buf = string.Format("Affects {0} by", affect_loc_name((int)paf.Location), paf.Modifier);

                    for (var i = 0; i < 32; i++)
                    {
                        if (paf.Modifier.IsSet(1 << i))
                            buf += " " + BuilderConstants.ris_flags[i];
                    }
                    
                    break;
            }

            ch.SendTo(buf);            
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

            var ecd = new ExtracedCharacterData
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
                    (ch.IsDevoted() ? ((PlayerInstance)ch).PlayerData.Favor / -500 : 0) <= percent);
        }

        public static void economize_mobgold(CharacterInstance mob)
        {
            mob.CurrentCoin = mob.CurrentCoin.GetLowestOfTwoNumbers(mob.Level*mob.Level*400);
            if (mob.CurrentRoom == null)
                return;

            var gold = (mob.CurrentRoom.Area.HighEconomy > 0 ? 1 : 0)*1000000000*mob.CurrentRoom.Area.LowEconomy;
            mob.CurrentCoin = 0.GetNumberThatIsBetween(mob.CurrentCoin, gold/10);
            if (mob.CurrentCoin > 0)
                mob.CurrentRoom.Area.LowerEconomy(mob.CurrentCoin);
        }

        public static void check_switches(bool possess)
        {
            foreach(var ch in DatabaseManager.Instance.CHARACTERS.Values)
                check_switch(ch, possess);
        }

        public static void check_switch(CharacterInstance ch, bool possess)
        {
            if (ch.Switched == null) return;
            if (!possess)
            {
                foreach (var af in ch.Switched.Affects)
                {
                    if (af.Duration == -1)
                        continue;

                    var skill = DatabaseManager.Instance.SKILLS.Get((int) af.Type);
                    if (af.Type != AffectedByTypes.None && skill != null &&
                        skill.SpellFunction.Value == Possess.spell_possess)
                        return;
                }
            }

            foreach (var cmd in DatabaseManager.Instance.COMMANDS.Values)
            {
                if (cmd.DoFunction.Value != Switch.do_switch)
                    continue;
                if (cmd.Level <= ch.Trust)
                    return;
                if (!ch.IsNpc() && cmd.Name.IsAnyEqual(((PlayerInstance)ch).PlayerData.bestowments)
                    && cmd.Level <= ch.Trust + GameManager.Instance.SystemData.BestowDifference)
                    return;
            }

            if (!possess)
            {
                ch.Switched.SetColor(ATTypes.AT_BLUE);
                ch.Switched.SendTo("You suddenly forfeit the power to switch!");
            }

            Return.do_return(ch.Switched, string.Empty);
        }
    }
}
