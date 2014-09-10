using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;
using SmaugCS.Extensions;
using SmaugCS.Managers;
using SmaugCS.Weather;
using SmaugCS.Constants;

namespace SmaugCS
{
    public static class act_info
    {
        public static void look_sky(CharacterInstance ch)
        {
            WeatherCell cell = WeatherManager.Instance.GetWeather(ch.CurrentRoom.Area);

            color.send_to_pager("You gaze up towards the heavens and see:\r\n", ch);

            if (WeatherCell.GetCloudCover(cell.CloudCover) == CloudCoverTypes.Moderately)
            {
                color.send_to_char("There are too many clouds in the sky so you cannot see anything else.\r\n", ch);
                return;
            }

            int sunpos = (Program.MAP_WIDTH * (24 - GameManager.Instance.GameTime.Hour) / 24);
            int moonpos = (sunpos + GameManager.Instance.GameTime.Day * Program.MAP_WIDTH / Program.NUM_DAYS) % Program.MAP_WIDTH;
            int moonphase = ((((Program.MAP_WIDTH + moonpos - sunpos) % Program.MAP_WIDTH) + (Program.MAP_WIDTH / 16)) * 8) /
                            Program.MAP_WIDTH;
            if (moonphase > 4)
                moonphase -= 8;
            int starpos = (sunpos + Program.MAP_WIDTH * GameManager.Instance.GameTime.Month / Program.NUM_MONTHS) % Program.MAP_WIDTH;

            StringBuilder sb = new StringBuilder();

            for (int line = 0; line < Program.MAP_HEIGHT; line++)
            {
                if ((GameManager.Instance.GameTime.Hour >= 6 && GameManager.Instance.GameTime.Hour <= 18)
                    && (line < 3 || line >= 6))
                    continue;

                sb.Append(" ");

                for (int i = 0; i <= Program.MAP_WIDTH; i++)
                {
                    if ((GameManager.Instance.GameTime.Hour >= 6 && GameManager.Instance.GameTime.Hour <= 18)
                        && (moonpos >= Program.MAP_WIDTH / 4 - 2)
                        && (moonpos <= 3 * Program.MAP_WIDTH / 4 + 2)
                        && (i >= moonpos - 2) && (i <= moonpos + 2)
                        && ((sunpos == moonpos && GameManager.Instance.GameTime.Hour == 12) || moonphase != 0)
                        && (WeatherManager.Instance.Weather.MoonMap.ToList()[line - 3].ToCharArray()[i + 2 - moonpos] == '@'))
                    {
                        if ((moonphase < 0 && i - 2 - moonpos >= moonphase)
                            || (moonphase > 0 && i + 2 - moonpos <= moonphase))
                            sb.Append("&W@");
                        else
                            sb.Append(" ");
                    }
                    else if ((line >= 3) && (line < 6)
                             && (moonpos >= Program.MAP_WIDTH / 4 - 2) && (moonpos <= 3 * Program.MAP_WIDTH / 4 + 2)
                             && (i >= moonpos - 2) && (i <= moonpos + 2)
                             && (WeatherManager.Instance.Weather.MoonMap.ToList()[line - 3].ToCharArray()[i + 2 - moonpos] == '@'))
                    {
                        if ((moonphase < 0 && i - 2 - moonpos >= moonphase)
                            || (moonphase > 0 && i + 2 - moonpos <= moonphase))
                            sb.Append("&W@");
                        else
                            sb.Append(" ");
                    }
                    else
                    {
                        if (GameManager.Instance.GameTime.Hour >= 6 && GameManager.Instance.GameTime.Hour <= 18)
                        {
                            if (i >= sunpos - 2 && i <= sunpos + 2)
                                sb.AppendFormat("&Y{0}",
                                                WeatherManager.Instance.Weather.SunMap.ToList()[line - 3].ToCharArray()[
                                                    i + 2 - sunpos]);
                            else
                                sb.Append(" ");
                        }
                        else
                        {
                            char c =
                                WeatherManager.Instance.Weather.StarMap.ToList()[line].ToCharArray()[
                                    (Program.MAP_WIDTH + 1 - starpos) % Program.MAP_WIDTH];
                            sb.Append(LookupConstants.StarCharacterMap.ContainsKey(c)
                                          ? LookupConstants.StarCharacterMap[c]
                                          : " ");
                        }
                    }
                }

                sb.Append(Environment.NewLine);
                color.send_to_pager(sb.ToString(), ch);
            }
        }

        public static string format_obj_to_char(ObjectInstance obj, CharacterInstance ch, bool fShort)
        {
            bool glowsee = false;

            StringBuilder sb = new StringBuilder();

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Invisible)
                && obj.ExtraFlags.IsSet(ItemExtraFlags.Glow)
                && !ch.IsAffected(AffectedByTypes.TrueSight)
                && !ch.IsAffected(AffectedByTypes.DetectInvisibility))
                glowsee = true;

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Invisible))
                sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 0));
            if ((ch.IsAffected(AffectedByTypes.DetectEvil)
                 || ch.CurrentClass == ClassTypes.Paladin)
                && obj.ExtraFlags.IsSet(ItemExtraFlags.Evil))
                sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 1));

            if (ch.CurrentClass == ClassTypes.Paladin)
            {
                if (obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                    && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                    && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                    sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 2));
                if (!obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                    && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                    && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                    sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 3));
                if (!obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                    && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                    && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                    sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 4));

                if (obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                    && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                    && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                    sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 5));
                if (obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                    && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                    && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                    sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 6));
                if (!obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                    && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                    && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                    sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 7));
            }

            if ((ch.IsAffected(AffectedByTypes.DetectMagic)
                 || ch.Act.IsSet((int)PlayerFlags.HolyLight))
                && obj.ExtraFlags.IsSet(ItemExtraFlags.Magical))
                sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 8));
            if (!glowsee && obj.ExtraFlags.IsSet(ItemExtraFlags.Glow))
                sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 9));
            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Hum))
                sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 10));
            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Hidden))
                sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 11));
            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Buried))
                sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 12));
            if (ch.IsImmortal() && obj.ExtraFlags.IsSet(ItemExtraFlags.Prototype))
                sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 13));
            if ((ch.IsAffected(AffectedByTypes.DetectTraps)
                 || ch.Act.IsSet((int)PlayerFlags.HolyLight))
                && obj.IsTrapped())
                sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 14));

            if (fShort)
            {
                if (glowsee && (ch.IsNpc() || !ch.Act.IsSet((int)PlayerFlags.HolyLight)))
                    sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 15));
                else if (!string.IsNullOrWhiteSpace(obj.ShortDescription))
                    sb.Append(obj.ShortDescription);
            }
            else
            {
                if (glowsee && (ch.IsNpc() || !ch.Act.IsSet((int)PlayerFlags.HolyLight)))
                    sb.Append(LookupManager.Instance.GetLookup("ObjectAffectStrings", 16));
                else if (!string.IsNullOrWhiteSpace(obj.Description))
                    sb.Append(obj.Description);
            }

            return sb.ToString();
        }

        public static string hallucinated_object(int ms, bool fShort)
        {
            int sms = ((ms + 10) / 5).GetNumberThatIsBetween(1, 20);

            return fShort
                       ? LookupManager.Instance.GetLookup("HallucinatedShortNames",
                                                          (SmaugRandom.Between(
                                                              6 - (sms/2).GetNumberThatIsBetween(1, 5), sms) - 1))
                       : LookupManager.Instance.GetLookup("HallucinatedLongNames",
                                                          (SmaugRandom.Between(
                                                              6 - (sms/2).GetNumberThatIsBetween(1, 5), sms) - 1));
        }

        public static void show_list_to_char(List<ObjectInstance> list, PlayerInstance ch, bool fShort, bool fShowNothing)
        {
            if (ch.Descriptor == null)
                return;

            if (list == null)
            {
                if (fShowNothing)
                {
                    if (ch.Act.IsSet(PlayerFlags.Combine))
                        color.send_to_char("     ", ch);
                    color.set_char_color(ATTypes.AT_OBJECT, ch);
                    color.send_to_char("Nothing.", ch);
                }
                return;
            }

            int count = list.Count;

            int ms = (ch.MentalState > 0 ? ch.MentalState : 1)*
                     (ch.PlayerData.GetConditionValue(ConditionTypes.Drunk) > 0
                         ? (ch.PlayerData.GetConditionValue(ConditionTypes.Drunk)/12)
                         : 1);

            int offcount;
            if (Math.Abs(ms) > 40)
            {
                offcount = ((count * ms) / 100).GetNumberThatIsBetween(-count, count * 2);
                if (offcount < 0)
                    offcount += SmaugRandom.Between(0, Math.Abs(offcount));
                else if (offcount > 0)
                    offcount -= SmaugRandom.Between(0, offcount);
            }
            else
                offcount = 0;

            if (count + offcount <= 0)
            {
                if (fShowNothing)
                {
                    if (ch.Act.IsSet(PlayerFlags.Combine))
                        color.send_to_char("     ", ch);
                    color.set_char_color(ATTypes.AT_OBJECT, ch);
                    color.send_to_char("Nothing.\r\n", ch);
                }
                return;
            }

            int tmp = offcount > 0 ? offcount : 0;
            int cnt = 0;

            int size = count + (offcount > 0 ? offcount : 0);
            string[] prgpstrShow = new string[size];
            int[] prgnShow = new int[size];
            int[] pitShow = new int[size];
            int nShow = 0;
            string pstrShow;
            bool fCombine = false;

            foreach (ObjectInstance obj in list)
            {
                if (offcount < 0 && ++cnt > (count + offcount))
                    break;

                if (tmp > 0 && SmaugRandom.Bits(1) == 0)
                {
                    prgpstrShow[nShow] = hallucinated_object(ms, fShort);
                    prgnShow[nShow] = 1;
                    pitShow[nShow] = SmaugRandom.Between((int)ItemTypes.Light, (int)ItemTypes.Book);
                    nShow++;
                    --tmp;
                }

                if (obj.WearLocation == WearLocations.None
                    && ch.CanSee(obj)
                    && (obj.ItemType != ItemTypes.Trap
                        || ch.IsAffected(AffectedByTypes.DetectInvisibility)))
                {
                    pstrShow = format_obj_to_char(obj, ch, fShort);
                    fCombine = false;

                    if (ch.Act.IsSet(PlayerFlags.Combine))
                    {
                        for (int i = nShow - 1; i >= 0; i--)
                        {
                            if (prgpstrShow[i] == pstrShow)
                            {
                                prgnShow[i] += obj.Count;
                                fCombine = true;
                                break;
                            }
                        }
                    }

                    pitShow[nShow] = (int)obj.ItemType;

                    if (!fCombine)
                    {
                        prgpstrShow[nShow] = pstrShow;
                        prgnShow[nShow] = obj.Count;
                        nShow++;
                    }
                }
            }

            if (tmp > 0)
            {
                for (int i = 0; i < tmp; i++)
                {
                    prgpstrShow[nShow] = hallucinated_object(ms, fShort);
                    prgnShow[nShow] = 1;
                    pitShow[nShow] = SmaugRandom.Between((int)ItemTypes.Light, (int)ItemTypes.Book);
                    nShow++;
                }
            }

            for (int i = 0; i < nShow; i++)
            {
                SetCharacterColorByItemType(ch, pitShow, i);

                if (fShowNothing)
                    color.send_to_char("     ", ch);
                color.send_to_char(prgpstrShow[i], ch);

                if (prgnShow[i] != 1)
                    color.ch_printf(ch, " (%d)", prgnShow[i]);

                color.send_to_char("\r\n", ch);
            }

            if (fShowNothing && nShow == 0)
            {
                if (ch.Act.IsSet(PlayerFlags.Combine))
                    color.send_to_char("     ", ch);
                color.set_char_color(ATTypes.AT_OBJECT, ch);
                color.send_to_char("Nothing.\r\n", ch);
            }
        }

        private static void SetCharacterColorByItemType(CharacterInstance ch, IList<int> pitShow, int i)
        {
            ItemTypes itemType = Realm.Library.Common.EnumerationExtensions.GetEnum<ItemTypes>(pitShow[i]);
            CharacterColorAttribute attrib = itemType.GetAttribute<CharacterColorAttribute>();
            if (attrib == null)
                color.set_char_color(ATTypes.AT_OBJECT, ch);
            else
                color.set_char_color(attrib.ATType, ch);
        }
        
        public static void show_char_to_char_0(CharacterInstance victim, PlayerInstance ch)
        {
            string buffer = string.Empty;

            color.set_char_color(ATTypes.AT_PERSON, ch);
            if (!victim.IsNpc())
            {
                if (victim.Switched == null)
                    color.send_to_char_color("&P[(Link Dead)]", ch);
                else if (!victim.IsAffected(AffectedByTypes.Possess))
                    buffer += "(Switched) ";
            }

            if (victim.IsNpc()
                && victim.IsAffected(AffectedByTypes.Possess)
                && ch.IsImmortal() && ch.Switched != null)
            {
                buffer += "(" + victim.Switched.Name + ")";
            }

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.AwayFromKeyboard))
                buffer += PlayerFlags.AwayFromKeyboard.GetAttribute<DescriptorAttribute>().Messages.ToList()[0];

            if ((!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.WizardInvisibility))
                || (victim.IsNpc() && victim.Act.IsSet((int)ActFlags.MobInvisibility)))
            {
                if (!victim.IsNpc())
                    buffer += string.Format("(Invis {0}) ", ((PlayerInstance)victim).PlayerData.WizardInvisible);
                else
                    buffer += string.Format("(MobInvis {0}) ", victim.MobInvisible);
            }

            if (!victim.IsNpc())
            {
                PlayerInstance vict = (PlayerInstance) victim;
                if (vict.IsImmortal() && vict.Level > LevelConstants.GetLevel(ImmortalTypes.Avatar))
                    color.send_to_char_color("&P(&WImmortal&P) ", ch);
                if (vict.PlayerData.Clan != null
                    && vict.PlayerData.Flags.IsSet(PCFlags.Deadly)
                    && !string.IsNullOrEmpty(vict.PlayerData.Clan.Badge)
                    && (vict.PlayerData.Clan.ClanType != ClanTypes.Order
                    || vict.PlayerData.Clan.ClanType != ClanTypes.Guild))
                    color.ch_printf_color(ch, "%s ", vict.PlayerData.Clan.Badge);
                else if (vict.CanPKill() && vict.Level < LevelConstants.ImmortalLevel)
                    color.send_to_char_color("&P(&wUnclanned&P) ", ch);
            }

            color.set_char_color(ATTypes.AT_PERSON, ch);

            buffer += GenerateBufferForAffectedBy(victim, ch);

            color.set_char_color(ATTypes.AT_PERSON, ch);
            if ((victim.CurrentPosition == victim.CurrentDefensivePosition && !string.IsNullOrEmpty(victim.LongDescription))
                || (victim.CurrentMorph != null && victim.CurrentMorph.Morph != null
                    && victim.CurrentMorph.Morph.Position == (int)victim.CurrentPosition))
            {
                if (victim.CurrentMorph != null)
                {
                    if (!ch.IsImmortal())
                    {
                        if (victim.CurrentMorph.Morph != null)
                            buffer += victim.CurrentMorph.Morph.LongDescription;
                        else
                            buffer += victim.LongDescription;
                    }
                    else
                    {
                        buffer += Macros.PERS(victim, ch);
                        if (!victim.IsNpc() && !ch.Act.IsSet(PlayerFlags.Brief))
                            buffer += ((PlayerInstance)victim).PlayerData.Title;
                        buffer += ".\r\n";
                    }
                }
                else
                    buffer += victim.LongDescription;
                color.send_to_char(buffer, ch);
                ch.ShowVisibleAffectsOn(victim);
                return;
            }

            if (victim.CurrentMorph != null && victim.CurrentMorph.Morph != null
                && !ch.IsImmortal())
                buffer += Macros.MORPHERS(victim, ch);
            else
                buffer += Macros.PERS(victim, ch);

            if (!victim.IsNpc() && !ch.Act.IsSet(PlayerFlags.Brief))
                buffer += ((PlayerInstance)victim).PlayerData.Title;

            TimerData timer = ch.GetTimer(TimerTypes.DoFunction);
            if (timer != null)
            {
                object[] attributes = timer.Action.Value.Method.GetCustomAttributes(typeof (DescriptorAttribute), false);
                DescriptorAttribute attrib =
                    (DescriptorAttribute) attributes.FirstOrDefault(x => x.GetType() == typeof (DescriptorAttribute));
                buffer += attrib == null ? " is looking rather lost." : attrib.Messages.First();
            }
            else
            {
                buffer += GenerateBufferDescriptorFromVictimPosition(victim, ch);
            }

            buffer += "\r\n";
            buffer = buffer.CapitalizeFirst();
            color.send_to_char(buffer, ch);
            ch.ShowVisibleAffectsOn(victim);
        }

        private static string GenerateBufferForAffectedBy(CharacterInstance victim, PlayerInstance ch)
        {
            if (victim.IsAffected(AffectedByTypes.Invisible))
                return AffectedByTypes.Invisible.GetAttribute<DescriptorAttribute>().Messages.First();
            if (victim.IsAffected(AffectedByTypes.Hide))
                return AffectedByTypes.Hide.GetAttribute<DescriptorAttribute>().Messages.First();
            if (victim.IsAffected(AffectedByTypes.PassDoor))
                return AffectedByTypes.PassDoor.GetAttribute<DescriptorAttribute>().Messages.First();
            if (victim.IsAffected(AffectedByTypes.FaerieFire))
                return AffectedByTypes.FaerieFire.GetAttribute<DescriptorAttribute>().Messages.First();
            if ((ch.IsAffected(AffectedByTypes.DetectEvil) && victim.IsEvil())
                || ch.CurrentClass == ClassTypes.Paladin)
                return ClassTypes.Paladin.GetAttribute<DescriptorAttribute>().Messages.First();
            if (victim.IsNeutral() && ch.CurrentClass == ClassTypes.Paladin)
                return ClassTypes.Paladin.GetAttribute<DescriptorAttribute>().Messages.ToList()[1];
            if (victim.IsGood() && ch.CurrentClass == ClassTypes.Paladin)
                return ClassTypes.Paladin.GetAttribute<DescriptorAttribute>().Messages.ToList()[2];

            if (victim.IsAffected(AffectedByTypes.Berserk))
                return AffectedByTypes.Berserk.GetAttribute<DescriptorAttribute>().Messages.First();
            if (!victim.IsNpc() && victim.Act.IsSet((int) PlayerFlags.Attacker))
                return PlayerFlags.Attacker.GetAttribute<DescriptorAttribute>().Messages.First();
            if (!victim.IsNpc() && victim.Act.IsSet((int) PlayerFlags.Killer))
                return PlayerFlags.Killer.GetAttribute<DescriptorAttribute>().Messages.First();
            if (!victim.IsNpc() && victim.Act.IsSet((int) PlayerFlags.Thief))
                return PlayerFlags.Thief.GetAttribute<DescriptorAttribute>().Messages.First();
            if (!victim.IsNpc() && victim.Act.IsSet((int) PlayerFlags.Litterbug))
                return PlayerFlags.Litterbug.GetAttribute<DescriptorAttribute>().Messages.First();
            if (victim.IsNpc() && ch.IsImmortal() && victim.Act.IsSet((int) ActFlags.Prototype))
                return ActFlags.Prototype.GetAttribute<DescriptorAttribute>().Messages.First();
            if (victim.IsNpc() && ch.CurrentMount != null
                && ch.CurrentMount == victim && ch.CurrentRoom == ch.CurrentMount.CurrentRoom)
                return "(Mount) ";
            if (victim.Switched != null && ((PlayerInstance)victim.Switched).Descriptor.ConnectionStatus == ConnectionTypes.Editing)
                return ConnectionTypes.Editing.GetAttribute<DescriptorAttribute>().Messages.First();
            if (victim.CurrentMorph != null)
                return "(Morphed) ";
            return string.Empty;
        }

        private static string GenerateBufferDescriptorFromVictimPosition(CharacterInstance victim, CharacterInstance ch)
        {
            PositionTypes pos = victim.CurrentPosition;
            DescriptorAttribute attrib = pos.GetAttribute<DescriptorAttribute>();

            switch (pos)
            {
                case PositionTypes.Sleeping:
                    return GetSleepingDescriptor(ch, victim, attrib);
                case PositionTypes.Sitting:
                    return GetSittingDescriptor(ch, victim, attrib);
                case PositionTypes.Standing:
                    return GetStandingDescriptor(ch, victim, attrib);
                case PositionTypes.Mounted:
                    return GetMountedDescriptor(ch, victim, attrib);
                case PositionTypes.Fighting:
                case PositionTypes.Evasive:
                case PositionTypes.Defensive:
                case PositionTypes.Berserk:
                case PositionTypes.Aggressive:
                    return GetFightingDescriptor(ch, victim, attrib);
                default:
                    return attrib.Messages.First();
            }
        }

        private static string GetFightingDescriptor(CharacterInstance ch, CharacterInstance victim,
            DescriptorAttribute attrib)
        {
            if (victim.CurrentFighting != null)
                return attrib.Messages.First();

            if (victim.GetMyTarget() == ch)
                return attrib.Messages.ToList()[2];

            if (victim.CurrentRoom == victim.CurrentFighting.Who.CurrentRoom)
                return string.Format(attrib.Messages.ToList()[2], Macros.PERS(victim.CurrentFighting.Who, ch));

            return attrib.Messages.ToList()[3];
        }

        private static string GetMountedDescriptor(CharacterInstance ch, CharacterInstance victim,
            DescriptorAttribute attrib)
        {
            if (victim.CurrentMount == null)
                return attrib.Messages.First();

            if (victim.CurrentMount == ch)
                return attrib.Messages.ToList()[1];

            if (victim.CurrentRoom == victim.CurrentMount.CurrentRoom)
                return string.Format(attrib.Messages.ToList()[2], Macros.PERS(victim.CurrentMount, ch));

            return attrib.Messages.ToList()[3];
        }

        private static string GetStandingDescriptor(CharacterInstance ch, CharacterInstance victim,
            DescriptorAttribute attrib)
        {
            if (victim.IsImmortal())
                return attrib.Messages.First();

            if (((victim.CurrentRoom.SectorType == SectorTypes.Underwater)
                 || (victim.CurrentRoom.SectorType == SectorTypes.OceanFloor))
                && !victim.IsAffected(AffectedByTypes.AquaBreath)
                && !victim.IsNpc())
                return attrib.Messages.ToList()[1];

            if ((victim.CurrentRoom.SectorType == SectorTypes.Underwater)
                || (victim.CurrentRoom.SectorType == SectorTypes.OceanFloor))
                return attrib.Messages.ToList()[2];

            if (victim.IsAffected(AffectedByTypes.Floating)
                || victim.IsAffected(AffectedByTypes.Flying))
                return attrib.Messages.ToList()[3];

            return attrib.Messages.ToList()[4];
        }

        private static string GetSittingDescriptor(CharacterInstance ch, CharacterInstance victim,
            DescriptorAttribute attrib)
        {
            if (ch.CurrentPosition == PositionTypes.Sitting)
                return attrib.Messages.First();
            if (ch.CurrentPosition == PositionTypes.Resting)
                return attrib.Messages.ToList()[1];
            return attrib.Messages.ToList()[2];
        }

        private static string GetSleepingDescriptor(CharacterInstance ch, CharacterInstance victim,
            DescriptorAttribute attrib)
        {
            if (ch.CurrentPosition == PositionTypes.Sitting || ch.CurrentPosition == PositionTypes.Resting)
                return attrib.Messages.First();
            return attrib.Messages.ToList()[1];
        }

        public static void show_char_to_char_1(CharacterInstance victim, PlayerInstance ch)
        {
            if (victim.CanSee(ch) && !ch.IsNpc()
                && !ch.Act.IsSet(PlayerFlags.WizardInvisibility))
            {
                comm.act(ATTypes.AT_ACTION, "$n looks at you.", ch, null, victim, ToTypes.Victim);
                comm.act(ATTypes.AT_ACTION, victim != ch ? "$n looks at $N." : "$n looks at $mself.", ch, null, victim,
                         ToTypes.NotVictim);
            }

            if (!string.IsNullOrEmpty(victim.Description))
            {
                if (victim.CurrentMorph != null && victim.CurrentMorph.Morph != null)
                    color.send_to_char(victim.CurrentMorph.Morph.Description, ch);
                else
                    color.send_to_char(victim.Description, ch);
            }
            else
            {
                if (victim.CurrentMorph != null && victim.CurrentMorph.Morph != null)
                    color.send_to_char(victim.CurrentMorph.Morph.Description, ch);
                else if (victim.IsNpc())
                    comm.act(ATTypes.AT_PLAIN, "You see nothing special about $M.", ch, null, victim, ToTypes.Character);
                else if (ch != victim)
                    comm.act(ATTypes.AT_PLAIN, "$E isn't much to look at...", ch, null, victim, ToTypes.Character);
                else
                    comm.act(ATTypes.AT_PLAIN, "You're not much to look at...", ch, null, null, ToTypes.Character);
            }

            show_race_line(ch, victim);
            show_condition(ch, victim);

            bool found = false;
            for (int i = 0; i < GameConstants.MaximumWearLocations; i++)
            {
                ObjectInstance obj = victim.GetEquippedItem(i);
                if (obj != null && ch.CanSee(obj))
                {
                    if (!found)
                    {
                        color.send_to_char("\r\n", ch);
                        if (victim != ch)
                            comm.act(ATTypes.AT_PLAIN, "$n is using:", ch, null, victim, ToTypes.Character);
                        else
                            comm.act(ATTypes.AT_PLAIN, "You are using:", ch, null, null, ToTypes.Character);
                        found = true;
                    }

                    if (!victim.IsNpc())
                    {
                        RaceData race = DatabaseManager.Instance.GetRace(victim.CurrentRace);
                        color.send_to_char(race.WhereNames[i], ch);
                    }
                    else
                        color.send_to_char(LookupManager.Instance.GetLookup("WhereNames", i), ch);
                    color.send_to_char(format_obj_to_char(obj, ch, true), ch);
                    color.send_to_char("\r\n", ch);
                }
            }

            if (ch.IsNpc() || victim == ch)
                return;

            if (ch.IsImmortal())
            {
                if (victim.IsNpc())
                    color.ch_printf(ch, "\r\nMobile #%d '%s' ", ((MobileInstance)victim).MobIndex.Vnum, victim.Name);
                else
                    color.ch_printf(ch, "\r\n%s ", victim.Name);

                color.ch_printf(ch, "is a level %d %s %s.\r\n", victim.Level,
                                DatabaseManager.Instance.GetRace(victim.CurrentRace).Name,
                                DatabaseManager.Instance.GetClass(victim.CurrentClass).Name);
            }

            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>("peek");
            if (skill == null)
                throw new ObjectNotFoundException("Skill 'peek' not found");

            if (SmaugRandom.D100() < Macros.LEARNED(ch, (int) skill.ID))
            {
                color.ch_printf(ch, "\r\nYou peek at %s inventory:\r\n", victim.Gender.PossessivePronoun());
                show_list_to_char(victim.Carrying, ch, true, true);
                skill.LearnFromSuccess(ch);
            }
            else if (ch.PlayerData.Learned[0] > (int)skill.ID)
                skill.LearnFromFailure(ch);
        }

        public static void show_char_to_char(IEnumerable<CharacterInstance> list, PlayerInstance ch)
        {
            foreach (CharacterInstance rch in list.Where(x => x != ch))
            {
                if (ch.CanSee(rch))
                    show_char_to_char_0(rch, ch);
                else if (ch.CurrentRoom.IsDark() && ch.IsAffected(AffectedByTypes.Infrared))
                {
                    color.set_char_color(ATTypes.AT_BLOOD, ch);
                    color.send_to_char("The red form of a living creature is here.\r\n", ch);
                }
            }
        }

        private static readonly Dictionary<string, int> StringToDoorDirection = new Dictionary<string, int>()
            {
                {"n;north", 0},
                {"e;east", 1},
                {"s;south", 2},
                {"w;west", 3},
                {"u;up", 4},
                {"d;down", 5},
                {"ne;northeast", 6},
                {"nw;northwest", 7},
                {"se;southeast", 8},
                {"sw;southwest", 9}
            };

        /// <summary>
        /// Returns classical DIKU door direction based on text
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static int get_door(string arg)
        {
            foreach (string args in StringToDoorDirection.Keys)
            {
                string[] words = args.Split(';');
                if (words.Contains(arg.ToLower()))
                    return StringToDoorDirection[args];
            }
            return -1;
        }

        public static void print_compass(CharacterInstance ch)
        {
            List<int> exitInfo = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<string> exitColors = new List<string>() { "&w", "&Y", "&C", "&b", "&w", "&R" };

            foreach (ExitData exit in ch.CurrentRoom.Exits)
            {
                if (exit.Destination == null || exit.Flags.IsSet((int)ExitFlags.Hidden)
                    || (exit.Flags.IsSet((int)ExitFlags.Secret)
                        && exit.Flags.IsSet((int)ExitFlags.Closed)))
                    continue;

                if (exit.Flags.IsSet((int)ExitFlags.Window))
                    exitInfo[(int)exit.Direction] = 2;
                else if (exit.Flags.IsSet((int)ExitFlags.Secret))
                    exitInfo[(int)exit.Direction] = 3;
                else if (exit.Flags.IsSet((int)ExitFlags.Closed))
                    exitInfo[(int)exit.Direction] = 4;
                else if (exit.Flags.IsSet((int)ExitFlags.Locked))
                    exitInfo[(int)exit.Direction] = 5;
                else
                    exitInfo[(int)exit.Direction] = 1;
            }

            color.set_char_color(ATTypes.AT_RMNAME, ch);
            color.ch_printf_color(ch, "\r\n%-50s         %s%s    %s%s    %s%s\r\n",
                                  ch.CurrentRoom.Name, exitColors[exitInfo[(int)DirectionTypes.Northwest]],
                                  exitInfo[(int)DirectionTypes.Northwest] > 0 ? "NW" : "- ",
                                  exitColors[exitInfo[(int)DirectionTypes.North]],
                                  exitInfo[(int)DirectionTypes.North] > 0 ? "N" : "-",
                                  exitColors[exitInfo[(int)DirectionTypes.Northeast]],
                                  exitInfo[(int)DirectionTypes.Northeast] > 0 ? "NE" : " -");

            if (ch.IsImmortal() && ch.Act.IsSet((int)PlayerFlags.RoomVNum))
                color.ch_printf_color(ch, "&w-<---- &YVnum: %6d &w----------------------------->-        ", ch.CurrentRoom.Vnum);
            else
                color.send_to_char_color("&w-<----------------------------------------------->-        ", ch);

            color.ch_printf_color(ch, "%s%s&w<-%s%s&w-&W(*)&w-%s%s&w->%s%s\r\n",
                                  exitColors[exitInfo[(int)DirectionTypes.West]],
                                  exitInfo[(int)DirectionTypes.West] > 0 ? "W" : "-",
                                  exitColors[exitInfo[(int)DirectionTypes.Up]],
                                  exitInfo[(int)DirectionTypes.Up] > 0 ? "U" : "-",
                                  exitColors[exitInfo[(int)DirectionTypes.Down]],
                                  exitInfo[(int)DirectionTypes.Down] > 0 ? "D" : "-",
                                  exitColors[exitInfo[(int)DirectionTypes.East]],
                                  exitInfo[(int)DirectionTypes.East] > 0 ? "E" : "-");
            color.ch_printf_color(ch,
                                  "                                                           %s%s    %s%s    %s%s\r\n\r\n",
                                  exitColors[exitInfo[(int)DirectionTypes.Southwest]],
                                  exitInfo[(int)DirectionTypes.Southwest] > 0 ? "SW" : "- ",
                                  exitColors[exitInfo[(int)DirectionTypes.South]],
                                  exitInfo[(int)DirectionTypes.South] > 0 ? "S" : "-",
                                  exitColors[exitInfo[(int)DirectionTypes.Southeast]],
                                  exitInfo[(int)DirectionTypes.Southeast] > 0 ? "SE" : " -");
        }

        public static string roomdesc(CharacterInstance ch)
        {
            string buffer = string.Empty;

            if (!ch.Act.IsSet((int)PlayerFlags.Brief))
                if (!string.IsNullOrEmpty(ch.CurrentRoom.Description))
                    buffer += ch.CurrentRoom.Description;

            if (string.IsNullOrEmpty(buffer))
                buffer += "(Not Set)";

            return buffer;
        }

        public static void show_race_line(CharacterInstance ch, CharacterInstance victim)
        {
            if (!victim.IsNpc() && victim != ch)
                color.ch_printf(ch, "%s is %d'%d\" and weighs %d pounds.\r\n",
                                Macros.PERS(victim, ch), victim.Height / 12, victim.Height % 12, victim.Weight);
            else if (!victim.IsNpc() && victim == ch)
                color.ch_printf(ch, "You are %d'%d\" and weight %d pounds.\r\n", victim.Height / 12, victim.Height % 12,
                                victim.Weight);
        }

        private static string GetConditionPhrase(int percent, bool isSelf)
        {
            HealthConditionTypes healthCond = HealthConditionTypes.PerfectHealth;
            foreach (HealthConditionTypes cond in EnumerationFunctions.GetAllEnumValues<HealthConditionTypes>())
            {
                if (percent >= cond.GetValue())
                    healthCond = cond;
            }

            DescriptorAttribute attrib = healthCond.GetAttribute<DescriptorAttribute>();
            return isSelf ? attrib.Messages.First() : attrib.Messages.ToList()[1];
        }

        public static void show_condition(CharacterInstance ch, CharacterInstance victim)
        {
            int percent = -1;
            if (victim.MaximumHealth > 0)
                percent = (int)(100.0f * (victim.CurrentHealth / (double)victim.MaximumHealth));

            color.send_to_char(
                string.Format(GetConditionPhrase(percent, victim == ch),
                victim != ch ? Macros.PERS(victim, ch) : "You")
                .CapitalizeFirst() + Environment.NewLine, ch);
        }


        public static HelpData get_help(CharacterInstance ch, string argument)
        {
            // TODO
            return null;
        }

        // TODO Rewrite this whogr stuff as its clunky and uses linked lists which we have no need of
        /*
        public static whogr_s find_whogr(DescriptorData d, List<whogr_s> list)
        {
            // TODO
            return null;
        }

        public static void indent_whogr(CharacterInstance looker, List<whogr_s> list, int ilev)
        {
            foreach (whogr_s whogr in list)
            {
                if (whogr.follower != null)
                {
                    int nlev = ilev;
                    CharacterInstance wch = whogr.d.Original ?? whogr.d.Character;

                    if (handler.can_see(looker, wch) && !Macros.IS_IMMORTAL(wch))
                        nlev += 3;
                    indent_whogr(looker, )
                }
            }
        }

        public static void create_whogr(CharacterInstance looker)
        {
            List<whogr_s> whogrList = new List<whogr_s>();
            whogr_s whogr, whogr_s, whogr_t;

            int dc = 0;
            int wc = 0;

            foreach (DescriptorData d in db.DESCRIPTORS
                .Where(d => d.ConnectionStatus == ConnectionTypes.Playing 
                    || d.ConnectionStatus == ConnectionTypes.Editing))
            {
                ++dc;
                CharacterInstance wch = d.Original ?? d.Character;
                if (wch.Leader == null || wch.Leader == wch || wch.Leader.Descriptor == null
                    || Macros.IS_NPC(wch.Leader) || Macros.IS_IMMORTAL(wch)
                    || Macros.IS_IMMORTAL(wch.Leader))
                {
                    whogr = new whogr_s();
                    whogrList.Add(whogr);
                    whogr.d = d;
                    whogr.indent = 0;
                    ++wc;
                }
            }

            while (wc < dc)
            {
                foreach (DescriptorData d in db.DESCRIPTORS
                    .Where(d => d.ConnectionStatus == ConnectionTypes.Playing
                        || d.ConnectionStatus == ConnectionTypes.Editing))
                {
                    if (find_whogr(d, whogrList) != null)
                        continue;

                    CharacterInstance wch = d.Original ?? d.Character;
                    whogr_t = find_whogr(wch.Leader.Descriptor, whogrList);
                    if (wch.Leader != null && wch.Leader != wch &&
                        wch.Leader.Descriptor != null && !Macros.IS_NPC(wch.Leader)
                        && !Macros.IS_IMMORTAL(wch) && !Macros.IS_IMMORTAL(wch.Leader)
                        && whogr_t != null)
                    {
                        whogr = new whogr_s();
                        whogrList.Add(whogr);
                        whogr.d = d;
                        whogr.indent = 0;
                        ++wc;
                    }
                }
            }

            indent_whogr(looker, whogrList, 0);
        }*/
    }
}
