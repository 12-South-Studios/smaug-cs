using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common.Extensions;
using SmaugCS.Commands;
using SmaugCS.Commands.Skills;
using SmaugCS.Commands.Skills.Thief;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;
using SmaugCS.Extensions;
using SmaugCS.Weather;

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

            int sunpos = (Program.MAP_WIDTH * (24 - db.GameTime.Hour) / 24);
            int moonpos = (sunpos + db.GameTime.Day * Program.MAP_WIDTH / Program.NUM_DAYS) % Program.MAP_WIDTH;
            int moonphase = ((((Program.MAP_WIDTH + moonpos - sunpos) % Program.MAP_WIDTH) + (Program.MAP_WIDTH / 16)) * 8) /
                            Program.MAP_WIDTH;
            if (moonphase > 4)
                moonphase -= 8;
            int starpos = (sunpos + Program.MAP_WIDTH * db.GameTime.Month / Program.NUM_MONTHS) % Program.MAP_WIDTH;

            StringBuilder sb = new StringBuilder();

            for (int line = 0; line < Program.MAP_HEIGHT; line++)
            {
                if ((db.GameTime.Hour >= 6 && db.GameTime.Hour <= 18)
                    && (line < 3 || line >= 6))
                    continue;

                sb.Append(" ");

                for (int i = 0; i <= Program.MAP_WIDTH; i++)
                {
                    if ((db.GameTime.Hour >= 6 && db.GameTime.Hour <= 18)
                        && (moonpos >= Program.MAP_WIDTH / 4 - 2)
                        && (moonpos <= 3 * Program.MAP_WIDTH / 4 + 2)
                        && (i >= moonpos - 2) && (i <= moonpos + 2)
                        && ((sunpos == moonpos && db.GameTime.Hour == 12) || moonphase != 0)
                        && (WeatherManager.Instance.Weather.MoonMap[line - 3].ToCharArray()[i + 2 - moonpos] == '@'))
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
                             && (WeatherManager.Instance.Weather.MoonMap[line - 3].ToCharArray()[i + 2 - moonpos] == '@'))
                    {
                        if ((moonphase < 0 && i - 2 - moonpos >= moonphase)
                            || (moonphase > 0 && i + 2 - moonpos <= moonphase))
                            sb.Append("&W@");
                        else
                            sb.Append(" ");
                    }
                    else
                    {
                        if (db.GameTime.Hour >= 6 && db.GameTime.Hour <= 18)
                        {
                            if (i >= sunpos - 2 && i <= sunpos + 2)
                                sb.AppendFormat("&Y{0}",
                                                WeatherManager.Instance.Weather.SunMap[line - 3].ToCharArray()[
                                                    i + 2 - sunpos]);
                            else
                                sb.Append(" ");
                        }
                        else
                        {
                            char c =
                                WeatherManager.Instance.Weather.StarMap[line].ToCharArray()[
                                    (Program.MAP_WIDTH + 1 - starpos) % Program.MAP_WIDTH];
                            sb.Append(GameConstants.StarCharacterMap.ContainsKey(c)
                                          ? GameConstants.StarCharacterMap[c]
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

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Invisible)
                && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Glow)
                && !ch.IsAffected(AffectedByTypes.TrueSight)
                && !ch.IsAffected(AffectedByTypes.DetectInvisibility))
                glowsee = true;

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Invisible))
                sb.Append(GameConstants.ObjectAffectStrings[0]);
            if ((ch.IsAffected(AffectedByTypes.DetectEvil)
                 || ch.CurrentClass == ClassTypes.Paladin)
                && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Evil))
                sb.Append(GameConstants.ObjectAffectStrings[1]);

            if (ch.CurrentClass == ClassTypes.Paladin)
            {
                if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiEvil)
                    && !Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiNeutral)
                    && !Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiGood))
                    sb.Append(GameConstants.ObjectAffectStrings[2]);
                if (!Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiEvil)
                    && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiNeutral)
                    && !Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiGood))
                    sb.Append(GameConstants.ObjectAffectStrings[3]);
                if (!Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiEvil)
                    && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiNeutral)
                    && !Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiGood))
                    sb.Append(GameConstants.ObjectAffectStrings[4]);

                if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiEvil)
                    && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiNeutral)
                    && !Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiGood))
                    sb.Append(GameConstants.ObjectAffectStrings[5]);
                if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiEvil)
                    && !Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiNeutral)
                    && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiGood))
                    sb.Append(GameConstants.ObjectAffectStrings[6]);
                if (!Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiEvil)
                    && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiNeutral)
                    && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.AntiGood))
                    sb.Append(GameConstants.ObjectAffectStrings[7]);
            }

            if ((ch.IsAffected(AffectedByTypes.DetectMagic)
                 || ch.Act.IsSet((int)PlayerFlags.HolyLight))
                && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Magical))
                sb.Append(GameConstants.ObjectAffectStrings[8]);
            if (!glowsee && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Glow))
                sb.Append(GameConstants.ObjectAffectStrings[9]);
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Hum))
                sb.Append(GameConstants.ObjectAffectStrings[10]);
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Hidden))
                sb.Append(GameConstants.ObjectAffectStrings[11]);
            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Buried))
                sb.Append(GameConstants.ObjectAffectStrings[12]);
            if (ch.IsImmortal() && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Prototype))
                sb.Append(GameConstants.ObjectAffectStrings[13]);
            if ((ch.IsAffected(AffectedByTypes.DetectTraps)
                 || ch.Act.IsSet((int)PlayerFlags.HolyLight))
                && handler.is_trapped(obj))
                sb.Append(GameConstants.ObjectAffectStrings[14]);

            if (fShort)
            {
                if (glowsee && (ch.IsNpc() || !ch.Act.IsSet((int)PlayerFlags.HolyLight)))
                    sb.Append(GameConstants.ObjectAffectStrings[15]);
                else if (!string.IsNullOrWhiteSpace(obj.ShortDescription))
                    sb.Append(obj.ShortDescription);
            }
            else
            {
                if (glowsee && (ch.IsNpc() || !ch.Act.IsSet((int)PlayerFlags.HolyLight)))
                    sb.Append(GameConstants.ObjectAffectStrings[16]);
                else if (!string.IsNullOrWhiteSpace(obj.Description))
                    sb.Append(obj.Description);
            }

            return sb.ToString();
        }

        public static string hallucinated_object(int ms, bool fShort)
        {
            int sms = Check.Range(1, (ms + 10) / 5, 20);

            return fShort
                ? GameConstants.HallucinatedObjectShort[SmaugRandom.Between(6 - Check.Range(1, sms / 2, 5), sms) - 1]
                : GameConstants.HallucinatedObjectLong[SmaugRandom.Between(6 - Check.Range(1, sms / 2, 5), sms) - 1];
        }

        public static string num_punct(int foo)
        {
            string buffer = foo.ToString();
            int rest = buffer.Length % 3;
            char[] newBuffer = new char[buffer.Length];

            for (int nindex = 0, index_new = 0; nindex < buffer.Length; nindex++, index_new++)
            {
                int x = nindex - rest;
                if (nindex != 0 && (x % 3) == 0)
                {
                    newBuffer[index_new] = ',';
                    index_new++;
                    newBuffer[index_new] = buffer[nindex];
                }
                else
                    newBuffer[index_new] = buffer[nindex];
            }

            return new string(newBuffer);
        }

        public static void show_list_to_char(List<ObjectInstance> list, CharacterInstance ch, bool fShort, bool fShowNothing)
        {
            if (ch.Descriptor == null)
                return;

            if (list == null)
            {
                if (fShowNothing)
                {
                    if (ch.IsNpc() || ch.Act.IsSet((int)PlayerFlags.Combine))
                        color.send_to_char("     ", ch);
                    color.set_char_color(ATTypes.AT_OBJECT, ch);
                    color.send_to_char("Nothing.\r\n", ch);
                }
                return;
            }

            int count = list.Count;

            int ms = (ch.MentalState > 0 ? ch.MentalState : 1) *
                     (ch.IsNpc()
                          ? 1
                          : (ch.PlayerData.ConditionTable[ConditionTypes.Drunk] > 0
                                 ? (ch.PlayerData.ConditionTable[ConditionTypes.Drunk] / 12)
                                 : 1));

            int offcount;
            if (Math.Abs(ms) > 40)
            {
                offcount = Check.Range(-count, (count * ms) / 100, count * 2);
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
                    if (ch.IsNpc() || ch.Act.IsSet((int)PlayerFlags.Combine))
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
                    && handler.can_see_obj(ch, obj)
                    && (obj.ItemType != ItemTypes.Trap
                        || ch.IsAffected(AffectedByTypes.DetectInvisibility)))
                {
                    pstrShow = format_obj_to_char(obj, ch, fShort);
                    fCombine = false;

                    if (ch.IsNpc() || ch.Act.IsSet((int)PlayerFlags.Combine))
                    {
                        for (int i = nShow - 1; i >= 0; i--)
                        {
                            if (prgpstrShow[i].Equals(pstrShow))
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
                if (ch.IsNpc() || ch.Act.IsSet((int)PlayerFlags.Combine))
                    color.send_to_char("     ", ch);
                color.set_char_color(ATTypes.AT_OBJECT, ch);
                color.send_to_char("Nothing.\r\n", ch);
            }
        }

        private static void SetCharacterColorByItemType(CharacterInstance ch, IList<int> pitShow, int i)
        {
            switch (pitShow[i])
            {
                default:
                    color.set_char_color(ATTypes.AT_OBJECT, ch);
                    break;
                case (int)ItemTypes.Blood:
                    color.set_char_color(ATTypes.AT_BLOOD, ch);
                    break;
                case (int)ItemTypes.Money:
                case (int)ItemTypes.Treasure:
                    color.set_char_color(ATTypes.AT_YELLOW, ch);
                    break;
                case (int)ItemTypes.Cook:
                case (int)ItemTypes.Food:
                    color.set_char_color(ATTypes.AT_HUNGRY, ch);
                    break;
                case (int)ItemTypes.DrinkContainer:
                case (int)ItemTypes.Fountain:
                    color.set_char_color(ATTypes.AT_THIRSTY, ch);
                    break;
                case (int)ItemTypes.Fire:
                    color.set_char_color(ATTypes.AT_FIRE, ch);
                    break;
                case (int)ItemTypes.Scroll:
                case (int)ItemTypes.Wand:
                case (int)ItemTypes.Staff:
                    color.set_char_color(ATTypes.AT_MAGIC, ch);
                    break;
            }
        }

        private static readonly Dictionary<AffectedByTypes, KeyValuePair<string, ATTypes>> VisibleAffectMap = new Dictionary<AffectedByTypes, KeyValuePair<string, ATTypes>>
            {
                { AffectedByTypes.FireShield, new KeyValuePair<string, ATTypes>("{0} is engulfed within a blaze of mystical flame.\r\n", ATTypes.AT_FIRE) },
                { AffectedByTypes.ShockShield, new KeyValuePair<string, ATTypes>("%s is surrounded by cascading torrents of energy.", ATTypes.AT_BLUE) },
                { AffectedByTypes.AcidMist, new KeyValuePair<string, ATTypes>("%s is visible through a cloud of churning mist.\r\n", ATTypes.AT_GREEN)},
                { AffectedByTypes.IceShield, new KeyValuePair<string, ATTypes>("%s is ensphered by shards of glistening ice.\r\n", ATTypes.AT_LBLUE)},
                { AffectedByTypes.VenomShield, new KeyValuePair<string, ATTypes>("%s is enshrouded in a choking cloud of gas.\r\n", ATTypes.AT_GREEN)},
                { AffectedByTypes.Charm, new KeyValuePair<string, ATTypes>("%s wanders in a dazed, zombie-like state.\r\n", ATTypes.AT_MAGIC)},
                { AffectedByTypes.Possess, new KeyValuePair<string, ATTypes>("%s appears to be in a deep trance...\r\n", ATTypes.AT_MAGIC)}
            };
        private static readonly KeyValuePair<string, ATTypes> DefaultVisibleAffectMap = new KeyValuePair<string, ATTypes>("", ATTypes.AT_PLAIN);
        private static KeyValuePair<string, ATTypes> GetVisibleAffects(AffectedByTypes type, string replaceString)
        {
            KeyValuePair<string, ATTypes> kvp = VisibleAffectMap.ContainsKey(type)
                          ? VisibleAffectMap[type]
                          : DefaultVisibleAffectMap;
            return new KeyValuePair<string, ATTypes>(string.Format(kvp.Key, replaceString), kvp.Value);
        }

        public static void show_visible_affects_to_char(CharacterInstance victim, CharacterInstance ch)
        {
            string name = (victim.IsNpc() ? victim.ShortDescription : victim.Name).CapitalizeFirst();

            KeyValuePair<string, ATTypes> kvp = DefaultVisibleAffectMap;
            if (victim.IsAffected(AffectedByTypes.Sanctuary))
            {
                color.set_char_color(ATTypes.AT_WHITE, ch);
                if (victim.IsGood())
                    kvp = new KeyValuePair<string, ATTypes>(string.Format("{0} glows with an aura of divine radiance.\r\n", name), ATTypes.AT_WHITE);
                else if (victim.IsEvil())
                    kvp = new KeyValuePair<string, ATTypes>(string.Format("{0} shimmers beneath an aura of dark energy.\r\n", name), ATTypes.AT_WHITE);
                else
                    kvp = new KeyValuePair<string, ATTypes>(string.Format("{0} is shrouded in flowing shadow and light.\r\n", name), ATTypes.AT_WHITE);
            }
            if (victim.IsAffected(AffectedByTypes.FireShield))
                kvp = GetVisibleAffects(AffectedByTypes.FireShield, name);
            if (victim.IsAffected(AffectedByTypes.ShockShield))
                kvp = GetVisibleAffects(AffectedByTypes.ShockShield, name);
            if (victim.IsAffected(AffectedByTypes.AcidMist))
                kvp = GetVisibleAffects(AffectedByTypes.AcidMist, name);
            if (victim.IsAffected(AffectedByTypes.IceShield))
                kvp = GetVisibleAffects(AffectedByTypes.IceShield, name);
            if (victim.IsAffected(AffectedByTypes.VenomShield))
                kvp = GetVisibleAffects(AffectedByTypes.VenomShield, name);
            if (victim.IsAffected(AffectedByTypes.Charm))
                kvp = GetVisibleAffects(AffectedByTypes.Charm, name);
            if (!victim.IsNpc() && victim.Descriptor == null
                && victim.Switched.IsAffected(AffectedByTypes.Possess))
                kvp = GetVisibleAffects(AffectedByTypes.Possess, Macros.PERS(victim, ch));

            color.set_char_color(kvp.Value, ch);
            color.ch_printf(ch, kvp.Key);
        }


        public static void show_char_to_char_0(CharacterInstance victim, CharacterInstance ch)
        {
            string buffer = string.Empty;

            color.set_char_color(ATTypes.AT_PERSON, ch);
            if (!victim.IsNpc() && victim.Descriptor == null)
            {
                if (victim.Switched == null)
                    color.send_to_char_color("&P[(Link Dead)]", ch);
                else if (!victim.IsAffected(AffectedByTypes.Possess))
                    buffer += "(Switched) ";
            }

            if (victim.IsNpc()
                && victim.IsAffected(AffectedByTypes.Possess)
                && ch.IsImmortal()
                && victim.Descriptor != null)
            {
                buffer += "(" + victim.Descriptor.Original.Name + ")";
            }

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.AwayFromKeyboard))
                buffer += "[AFK]";

            if ((!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.WizardInvisibility))
                || (victim.IsNpc() && victim.Act.IsSet((int)ActFlags.MobInvisibility)))
            {
                if (!victim.IsNpc())
                    buffer += string.Format("(Invis {0}) ", victim.PlayerData.WizardInvisible);
                else
                    buffer += string.Format("(MobInvis {0}) ", victim.MobInvisible);
            }

            if (!victim.IsNpc())
            {
                if (victim.IsImmortal() && victim.Level > Program.LEVEL_AVATAR)
                    color.send_to_char_color("&P(&WImmortal&P) ", ch);
                if (victim.PlayerData.Clan != null
                    && victim.PlayerData.Flags.IsSet((int)PCFlags.Deadly)
                    && !string.IsNullOrEmpty(victim.PlayerData.Clan.Badge)
                    && (victim.PlayerData.Clan.ClanType != ClanTypes.Order
                    || victim.PlayerData.Clan.ClanType != ClanTypes.Guild))
                    color.ch_printf_color(ch, "%s ", victim.PlayerData.Clan.Badge);
                else if (victim.CanPKill() && victim.Level < Program.LEVEL_IMMORTAL)
                    color.send_to_char_color("&P(&wUnclanned&P) ", ch);
            }

            color.set_char_color(ATTypes.AT_PERSON, ch);

            if (victim.IsAffected(AffectedByTypes.Invisible))
                buffer += "(Invis) ";
            if (victim.IsAffected(AffectedByTypes.Hide))
                buffer += "(Hide) ";
            if (victim.IsAffected(AffectedByTypes.PassDoor))
                buffer += "(Translucent) ";
            if (victim.IsAffected(AffectedByTypes.FaerieFire))
                buffer += "(Pink Aura) ";
            if ((ch.IsAffected(AffectedByTypes.DetectEvil) && victim.IsEvil())
                || ch.CurrentClass == ClassTypes.Paladin)
                buffer += "(Red Aura) ";
            if (victim.IsNeutral() && ch.CurrentClass == ClassTypes.Paladin)
                buffer += "(Grey Aura) ";
            if (victim.IsGood() && ch.CurrentClass == ClassTypes.Paladin)
                buffer += "(White Aura) ";

            if (victim.IsAffected(AffectedByTypes.Berserk))
                buffer += "(Wild-eyed) ";
            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Attacker))
                buffer += "(ATTACKER) ";
            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Killer))
                buffer += "(KILLER) ";
            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Thief))
                buffer += "(THIEF) ";
            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Litterbug))
                buffer += "(LITTERBUG) ";
            if (victim.IsNpc() && ch.IsImmortal() && victim.Act.IsSet((int)ActFlags.Prototype))
                buffer += "(PROTO) ";
            if (victim.IsNpc() && ch.CurrentMount != null
                && ch.CurrentMount == victim && ch.CurrentRoom == ch.CurrentMount.CurrentRoom)
                buffer += "(Mount) ";
            if (victim.Descriptor != null && victim.Descriptor.ConnectionStatus == ConnectionTypes.Editing)
                buffer += "(Writing) ";
            if (victim.CurrentMorph != null)
                buffer += "(Morphed) ";

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
                        if (!victim.IsNpc() && !ch.Act.IsSet((int)PlayerFlags.Brief))
                            buffer += victim.PlayerData.Title;
                        buffer += ".\r\n";
                    }
                }
                else
                    buffer += victim.LongDescription;
                color.send_to_char(buffer, ch);
                show_visible_affects_to_char(victim, ch);
                return;
            }

            if (victim.CurrentMorph != null && victim.CurrentMorph.Morph != null
                && !ch.IsImmortal())
                buffer += Macros.MORPHERS(victim, ch);
            else
                buffer += Macros.PERS(victim, ch);

            if (!victim.IsNpc() && !ch.Act.IsSet((int)PlayerFlags.Brief))
                buffer += victim.PlayerData.Title;

            TimerData timer = handler.get_timerptr(ch, TimerTypes.DoFunction);
            if (timer != null)
            {
                if (timer.Action.Value == Cast.do_cast)
                    buffer += " is here chanting.";
                else if (timer.Action.Value == Dig.do_dig)
                    buffer += " is here digging.";
                else if (timer.Action.Value == Search.do_search)
                    buffer += " is searching the area for something.";
                else if (timer.Action.Value == DeTrap.do_detrap)
                    buffer += " is working with the trap here.";
                else
                    buffer += " is looking rather lost.";
            }
            else
            {
                switch (victim.CurrentPosition)
                {
                    case PositionTypes.Dead:
                        buffer += " is DEAD!!";
                        break;
                    case PositionTypes.Mortal:
                        buffer += " is mortally wounded.";
                        break;
                    case PositionTypes.Incapacitated:
                        buffer += " is incapacitated.";
                        break;
                    case PositionTypes.Stunned:
                        buffer += " is laying here stunned.";
                        break;
                    case PositionTypes.Sleeping:
                        if (ch.CurrentPosition == PositionTypes.Sitting || ch.CurrentPosition == PositionTypes.Resting)
                            buffer += " is sleeping nearby.";
                        else
                            buffer += " is deep in slumber here.";
                        break;
                    case PositionTypes.Sitting:
                        if (ch.CurrentPosition == PositionTypes.Sitting)
                            buffer += " sits here with you.";
                        else if (ch.CurrentPosition == PositionTypes.Resting)
                            buffer += " sits nearby as you lay around.";
                        else
                            buffer += " sits upright here.";
                        break;
                    case PositionTypes.Standing:
                        if (victim.IsImmortal())
                        {
                            buffer += " is here before you.";
                            break;
                        }
                        if (((victim.CurrentRoom.SectorType == SectorTypes.Underwater)
                             || (victim.CurrentRoom.SectorType == SectorTypes.OceanFloor))
                            && !victim.IsAffected(AffectedByTypes.AquaBreath)
                            && !victim.IsNpc())
                        {
                            buffer += " is drowning here.";
                            break;
                        }
                        if ((victim.CurrentRoom.SectorType == SectorTypes.Underwater)
                            || (victim.CurrentRoom.SectorType == SectorTypes.OceanFloor))
                        {
                            buffer += " is here in the water.";
                            break;
                        }
                        if (victim.IsAffected(AffectedByTypes.Floating)
                            || victim.IsAffected(AffectedByTypes.Flying))
                        {
                            buffer += " is hovering here.";
                            break;
                        }

                        buffer += " is standing here.";
                        break;
                    case PositionTypes.Shove:
                        buffer += " is being shoved around.";
                        break;
                    case PositionTypes.Drag:
                        buffer += " is being dragged around.";
                        break;
                    case PositionTypes.Mounted:
                        buffer += " is here, upon ";
                        if (victim.CurrentMount == null)
                            buffer += "thin air???";
                        else if (victim.CurrentMount == ch)
                            buffer += "your back.";
                        else if (victim.CurrentRoom == victim.CurrentMount.CurrentRoom)
                        {
                            buffer += Macros.PERS(victim.CurrentMount, ch);
                            buffer += ".";
                        }
                        else
                            buffer += "someone who left??";
                        break;
                    case PositionTypes.Fighting:
                    case PositionTypes.Evasive:
                    case PositionTypes.Defensive:
                    case PositionTypes.Aggressive:
                    case PositionTypes.Berserk:
                        buffer += " is here, fighting ";
                        if (victim.CurrentFighting != null)
                        {
                            buffer += "thin air???";

                            victim.CurrentPosition = victim.CurrentMount == null
                                                  ? PositionTypes.Standing
                                                  : PositionTypes.Mounted;
                        }
                        else if (fight.who_fighting(victim) == ch)
                            buffer += "YOU!";
                        else if (victim.CurrentRoom == victim.CurrentFighting.Who.CurrentRoom)
                        {
                            buffer += Macros.PERS(victim.CurrentFighting.Who, ch);
                            buffer += ".";
                        }
                        else
                            buffer += "someone who left??";
                        break;
                }
            }

            buffer += "\r\n";
            buffer = buffer.CapitalizeFirst();
            color.send_to_char(buffer, ch);
            show_visible_affects_to_char(victim, ch);
        }

        public static void show_char_to_char_1(CharacterInstance victim, CharacterInstance ch)
        {
            if (handler.can_see(victim, ch) && !ch.IsNpc()
                && !ch.Act.IsSet((int)PlayerFlags.WizardInvisibility))
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
            for (int i = 0; i < Program.MAX_WEAR; i++)
            {
                ObjectInstance obj = victim.GetEquippedItem(i);
                if (obj != null && handler.can_see_obj(ch, obj))
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
                        RaceData race = db.GetRace(victim.CurrentRace);
                        color.send_to_char(race.WhereNames[i], ch);
                    }
                    else
                        color.send_to_char(GameConstants.WhereNames[i], ch);
                    color.send_to_char(format_obj_to_char(obj, ch, true), ch);
                    color.send_to_char("\r\n", ch);
                }
            }

            if (ch.IsNpc() || victim == ch)
                return;

            if (ch.IsImmortal())
            {
                if (victim.IsNpc())
                    color.ch_printf(ch, "\r\nMobile #%d '%s' ", victim.MobIndex.Vnum, victim.Name);
                else
                    color.ch_printf(ch, "\r\n%s ", victim.Name);

                color.ch_printf(ch, "is a level %d %s %s.\r\n", victim.Level,
                                db.GetRace(victim.CurrentRace).Name, db.GetClass(victim.CurrentClass).Name);
            }

            if (SmaugRandom.Percent() < Macros.LEARNED(ch, 0)) // TODO: gsn_peek
            {
                color.ch_printf(ch, "\r\nYou peek at %s inventory:\r\n", victim.Gender.PossessivePronoun());
                show_list_to_char(victim.Carrying, ch, true, true);
                skills.learn_from_success(ch, 0);    // TODO gsn_peek
            }
            else if (ch.PlayerData.Learned[0] > 0)  // TODO gsn_peek
                skills.learn_from_failure(ch, 0);   // TODO gsn_peek
        }

        public static void show_char_to_char(IEnumerable<CharacterInstance> list, CharacterInstance ch)
        {
            foreach (CharacterInstance rch in list.Where(x => x != ch))
            {
                if (handler.can_see(ch, rch))
                    show_char_to_char_0(rch, ch);
                else if (ch.CurrentRoom.IsDark() && ch.IsAffected(AffectedByTypes.Infrared))
                {
                    color.set_char_color(ATTypes.AT_BLOOD, ch);
                    color.send_to_char("The red form of a living creature is here.\r\n", ch);
                }
            }
        }

        public static bool check_blind(CharacterInstance ch)
        {
            return !ch.IsBlind() || ((Func<bool>)(() =>
                {
                    color.send_to_char("You can't see a thing!\r\n", ch);
                    return false;
                }))();
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

        private static readonly Dictionary<int, string> SelfConditionTable = new Dictionary<int, string>()
            {
                {100, "{0} is in perfect health."},
                {90, "{0} is slightly scratched."},
                {80, "{0} has a few bruises."},
                {70, "{0} has some cuts."},
                {60, "{0} has several wounds."},
                {50, "{0} has many nasty wounds."},
                {40, "{0} is bleeding freely."},
                {30, "{0} is covered in blood."},
                {20, "{0} is leaking guts."},
                {10, "{0} is almost dead."},
                {0, "{0} is DYING."}
            };
        private static readonly Dictionary<int, string> OtherConditionTable = new Dictionary<int, string>()
            {
                {100, "{0} are in perfect health."},
                {90, "{0} are slightly scratched."},
                {80, "{0} have a few bruises."},
                {70, "{0} have some cuts."},
                {60, "{0} have several wounds."},
                {50, "{0} have many nasty wounds."},
                {40, "{0} are bleeding freely."},
                {30, "{0} are covered in blood."},
                {20, "{0} are leaking guts."},
                {10, "{0} are almost dead."},
                {0, "{0} are DYING."}
            };
        private static string GetConditionPhrase(int percent, bool isSelf)
        {
            Dictionary<int, string> localTable = isSelf ? SelfConditionTable : OtherConditionTable;

            foreach (int minCondition in localTable.Keys.Where(minCondition => percent >= minCondition))
            {
                return localTable[minCondition];
            }
            return string.Empty;
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
