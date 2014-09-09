using System;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;

using SmaugCS.Managers;

namespace SmaugCS.Commands.Organizations
{
    public static class Roster
    {
        public static void do_roster(CharacterInstance ch, string argument)
        {
            if (ch.IsNpc())
            {
                color.send_to_char("NPCs can't use this command.\r\n", ch);
                return;
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                color.send_to_char("Usage: roster <clanname>\r\n", ch);
                color.send_to_char("Usage: roster <clanname> remove <name>\r\n", ch);
                return;
            }

            Tuple<string, string> tuple = argument.FirstArgument();
            argument = tuple.Item2;
            string arg = tuple.Item1;

            ClanData clan = DatabaseManager.Instance.GetEntity<ClanData>(arg);
            if (clan == null)
            {
                color.ch_printf(ch, "No such guild or clan known as %s.\r\n", arg);
                return;
            }

            if (string.IsNullOrWhiteSpace(argument))
            {
                color.ch_printf(ch, "Membership roster for the %s %s\r\n\r\n",
                    clan.ClanType == ClanTypes.Order ? "Guild" : "Clan");
                color.ch_printf(ch, "%-15.15s  %-15.15s %-6.6s %-6.6s %-6.6s %s\r\n",
                    "Name", "Class", "Level", "Kills", "Deaths", "Joined on");
                color.send_to_char("-------------------------------------------------------------------------------------\r\n", ch);

                int total = 0;
                foreach (RosterData member in clan.Members)
                {
                    color.ch_printf(ch, "%-15.15s  %-15.15s %-6d %-6d %-6d %s",
                                    member.Name, LookupManager.Instance.GetLookup("NPCClasses", member.Class).CapitalizeFirst(),
                                    member.Level, member.Kills, member.Deaths, member.Joined.ToShortTimeString());
                    total++;
                }

                color.ch_printf(ch, "\r\nThere are %d member%s in %s\r\n", total, total == 1 ? "" : "s", clan.Name);
            }

            tuple = argument.FirstArgument();
            argument = tuple.Item2;
            string arg2 = tuple.Item1;

            if (arg2.EqualsIgnoreCase("remove"))
            {
                if (string.IsNullOrWhiteSpace(argument))
                {
                    color.send_to_char("Remove who from the roster?\r\n", ch);
                    return;
                }

                clan.RemoveFromRoster(argument);
                //clan.Save();
                color.ch_printf(ch, "%s has been removed from the roster for %s.\r\n", argument, clan.Name);
            }

            do_roster(ch, "");
        }
    }
}
