
using System;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class Group
    {
        public static void do_group(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (string.IsNullOrWhiteSpace(firstArg))
                display_group(ch);
            else if (firstArg.Equals("disband"))
                disband_group(ch);
            else if (firstArg.Equals("all"))
                group_all(ch);
            else
                group_player(ch, argument.SecondWord());
        }

        private static void display_group(CharacterInstance ch)
        {
            CharacterInstance leader = ch.Leader ?? ch;
            color.set_char_color(ATTypes.AT_DGREEN, ch);
            color.ch_printf(ch, "\r\nFollowing %-12.12s     [hitpnts]   [ magic ] [mst] [mvs] [race]%s\r\n",
                            Macros.PERS(leader, ch), ch.Level < Program.LEVEL_AVATAR ? " [to lvl]" : "");

            foreach (CharacterInstance gch in DatabaseManager.Instance.CHARACTERS.Values.Where(x => x.IsSameGroup(ch)))
            {
                color.set_char_color(ATTypes.AT_DGREEN, ch);
                string buffer = string.Empty;

                if (gch.IsAffected(AffectedByTypes.Possess))
                {
                    color.ch_printf(ch,
                                    "[%2d %s] %-16s %4s/%4s hp %4s/%4s %s %4s/%4s mv %5s xp\r\n",
                                    gch.Level,
                                    gch.IsNpc() ? "Mob" : db.GetClass(gch.CurrentClass).Name,
                                    Macros.PERS(gch, ch).CapitalizeFirst(),
                                    "????", "????", "????", "????", gch.IsVampire() ? "bp" : "mana", "????",
                                    "????", "?????");
                }
                else
                    buffer = GetAlignmentString(gch.CurrentAlignment);

                color.set_char_color(ATTypes.AT_DGREEN, ch);
                color.send_to_char("[", ch);
                color.set_char_color(ATTypes.AT_DGREEN, ch);
                color.ch_printf(ch, "%-2d %2.2s %3.3s", gch.Level, buffer,
                                gch.IsNpc() ? "Mob" : db.GetClass(gch.CurrentClass).Name);
                color.set_char_color(ATTypes.AT_DGREEN, ch);
                color.send_to_char("]  ", ch);
                color.set_char_color(ATTypes.AT_DGREEN, ch);
                color.ch_printf(ch, "%-12.12s ", Macros.PERS(gch, ch).CapitalizeFirst());

                if (gch.CurrentHealth < gch.MaximumHealth / 4)
                    color.set_char_color(ATTypes.AT_DANGER, ch);
                else if (gch.CurrentHealth < gch.MaximumHealth / 2.5f)
                    color.set_char_color(ATTypes.AT_YELLOW, ch);
                else
                    color.set_char_color(ATTypes.AT_GREY, ch);

                color.ch_printf(ch, "%5d", gch.CurrentHealth);
                color.set_char_color(ATTypes.AT_GREY, ch);
                color.ch_printf(ch, "/%-5d ", gch.MaximumHealth);

                color.set_char_color(gch.IsVampire()
                                         ? ATTypes.AT_BLOOD
                                         : ATTypes.AT_LBLUE, ch);

                if (gch.CurrentClass != ClassTypes.Warrior)
                {
                    color.ch_printf(ch, "%5d/%-5d ",
                                    gch.IsVampire()
                                        ? gch.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty]
                                        : gch.CurrentMana,
                                    gch.IsVampire() ? 10 + gch.Level : gch.MaximumMana);
                }
                else
                    color.send_to_char("            ", ch);

                if (gch.MentalState < -25 || gch.MentalState > 25)
                    color.set_char_color(ATTypes.AT_YELLOW, ch);
                else
                    color.set_char_color(ATTypes.AT_GREEN, ch);

                color.ch_printf(ch, "%3.3s  ",
                                gch.MentalState > 75
                                    ? "+++"
                                    : gch.MentalState > 50
                                          ? "=++"
                                          : gch.MentalState > 25
                                                ? "==+"
                                                : gch.MentalState > -25
                                                      ? "==="
                                                      : gch.MentalState > -50 ? "-==" : gch.MentalState > -75 ? "--=" : "---");

                color.set_char_color(ATTypes.AT_DGREEN, ch);
                color.ch_printf(ch, "%5d ", gch.CurrentMovement);
                color.ch_printf(ch, "%6s ", GetRaceShortName(gch.CurrentRace));
                color.set_char_color(ATTypes.AT_GREEN, ch);

                if (gch.Level < Program.LEVEL_AVATAR)
                    color.ch_printf(ch, "%8d ", gch.GetExperienceLevel(gch.Level + 1) - gch.Experience);

                color.send_to_char("\r\n", ch);
            }
        }

        private static string GetRaceShortName(RaceTypes race)
        {
            switch (race)
            {
                case RaceTypes.Human:
                    return "human";
                case RaceTypes.Elf:
                    return "elf";
                case RaceTypes.Dwarf:
                    return "Dwarf";
                case RaceTypes.Halfling:
                    return "hlflng";
                case RaceTypes.Pixie:
                    return "pixie";
                case RaceTypes.HalfOgre:
                    return "h-ogre";
                case RaceTypes.HalfOrc:
                    return "h-orc";
                case RaceTypes.HalfTroll:
                    return "h-trol";
                case RaceTypes.HalfElf:
                    return "h-elf";
                case RaceTypes.Gith:
                    return "gith";
                case RaceTypes.Drow:
                    return "drow";
                case RaceTypes.SeaElf:
                    return "seaelf";
                case RaceTypes.Lizardman:
                    return "lizard";
                case RaceTypes.Gnome:
                    return "gnome";
                default:
                    return string.Empty;
            }
        }

        private static string GetAlignmentString(int alignment)
        {
            if (alignment > 750)
                return " A";
            if (alignment > 350)
                return "-A";
            if (alignment > 150)
                return "+N";
            if (alignment > -150)
                return " N";
            if (alignment > -350)
                return "-N";
            if (alignment > -750)
                return "+S";
            return " S";
        }
        private static void disband_group(CharacterInstance ch)
        {
            if (ch.Leader != null || ch.Master != null)
            {
                color.send_to_char("You cannot disband a group if you're following someone.\r\n", ch);
                return;
            }

            int count = 0;
            foreach (CharacterInstance gch in DatabaseManager.Instance.CHARACTERS.Values.Where(x => x.IsSameGroup(ch) && x != ch))
            {
                gch.Leader = null;
                gch.Master = null;
                count++;
                color.send_to_char("Your group is disbanded.\r\n", gch);
            }

            color.send_to_char(count == 0
                                   ? "You have no group members to disband.\r\n"
                                   : "You disband your group.\r\n", ch);
        }

        private static void group_all(CharacterInstance ch)
        {
            int count = 0;
            foreach (CharacterInstance rch in ch.CurrentRoom.Persons.Where(x =>
                x != ch
                && !x.IsNpc()
                && handler.can_see(ch, x)
                && x.Master == ch
                && ch.Master == null
                && ch.Leader == null
                && Math.Abs(ch.Level - x.Level) > 9
                && !x.IsSameGroup(ch)
                && ch.IsPKill() == x.IsPKill()))
            {
                rch.Leader = ch;
                count++;
            }

            if (count == 0)
                color.send_to_char("You have no eligible group members.\r\n", ch);
            else
            {
                comm.act(ATTypes.AT_ACTION, "$n groups $s followers.", ch, null, null, ToTypes.Room);
                color.send_to_char("You group your followers.\r\n", ch);
            }
        }

        private static void group_player(CharacterInstance ch, string argument)
        {
            CharacterInstance victim = handler.get_char_room(ch, argument);
            if (victim == null)
            {
                color.send_to_char("They aren't here.\r\n", ch);
                return;
            }

            if (ch.Master != null || (ch.Leader != null && ch.Leader != ch))
            {
                color.send_to_char("But you are following someone else!\r\n", ch);
                return;
            }

            if (victim.Master != ch && ch != victim)
            {
                comm.act(ATTypes.AT_PLAIN, "$N isn't following you.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (victim == ch)
            {
                comm.act(ATTypes.AT_PLAIN, "You can't group yourself.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (victim.IsSameGroup(ch) && ch != victim)
            {
                victim.Leader = null;
                comm.act(ATTypes.AT_ACTION, "$n removes $N from $s group.", ch, null, victim, ToTypes.NotVictim);
                comm.act(ATTypes.AT_ACTION, "$n removes you from $s group.", ch, null, victim, ToTypes.Victim);
                comm.act(ATTypes.AT_ACTION, "You remove $N from your group.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (ch.Level - victim.Level < -8
                || ch.Level - victim.Level > 8
                || (ch.IsPKill() != victim.IsPKill()))
            {
                comm.act(ATTypes.AT_PLAIN, "$N cannot join $n's group.", ch, null, victim, ToTypes.NotVictim);
                comm.act(ATTypes.AT_PLAIN, "You cannot join $n's group.", ch, null, victim, ToTypes.Victim);
                comm.act(ATTypes.AT_PLAIN, "$N cannot join your group.", ch, null, victim, ToTypes.Character);
                return;
            }

            victim.Leader = ch;
            comm.act(ATTypes.AT_ACTION, "$N oins $n's group.", ch, null, victim, ToTypes.NotVictim);
            comm.act(ATTypes.AT_ACTION, "You join $n's group.", ch, null, victim, ToTypes.Victim);
            comm.act(ATTypes.AT_ACTION, "$N joins your group.", ch, null, victim, ToTypes.Character);
        }
    }
}
