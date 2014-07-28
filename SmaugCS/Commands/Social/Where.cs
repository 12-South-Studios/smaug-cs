using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;

namespace SmaugCS.Commands
{
    public static class Where
    {
        public static void do_where(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (string.IsNullOrEmpty(firstArg))
                ViewPlayersNearby(ch);
            else
                ViewIndividualPlayer(ch, firstArg);
        }

        private static void ViewIndividualPlayer(CharacterInstance ch, string firstArg)
        {
            CharacterInstance victim = handler.get_char_world(ch, firstArg);
            if (victim != null)
            {
                if (victim.CurrentRoom != null
                    && victim.CurrentRoom.Area == ch.CurrentRoom.Area
                    && !victim.IsAffected(AffectedByTypes.Hide)
                    && !victim.IsAffected(AffectedByTypes.Sneak)
                    && ch.CanSee(victim))
                {
                    color.pager_printf(ch, "{0} {1}", Macros.PERS(victim, ch).PadRight(28, ' '), victim.CurrentRoom.Name);
                    return;
                }
            }

            comm.act(ATTypes.AT_PLAIN, "You didn't find any $T.", ch, null, firstArg, ToTypes.Character);
        }

        private static void ViewPlayersNearby(CharacterInstance ch)
        {
            color.pager_printf(ch, "Players near you in {0}:", ch.CurrentRoom.Area.Name);

            IEnumerable<CharacterInstance> victimList = GetVisiblePlayersInArea(ch);
            if (!victimList.Any())
            {
                color.send_to_char("None", ch);
                return;
            }

            foreach (CharacterInstance victim in victimList)
            {
                color.pager_printf_color(ch, "&P{0}  ", victim.Name.PadRight(13, ' '));
                if (victim.IsImmortal() && victim.Level > LevelConstants.GetLevel("Avatar"))
                    color.send_to_pager_color("&P(&WImmortal&P)\t", ch);
                else if (victim.CanPKill() && victim.PlayerData.Clan != null
                         && victim.PlayerData.Clan.ClanType != ClanTypes.Order
                         && victim.PlayerData.Clan.ClanType != ClanTypes.Guild)
                    color.pager_printf_color(ch, "{0}\t", victim.PlayerData.Clan.Badge.PadRight(18, ' '));
                else if (victim.CanPKill())
                    color.send_to_pager_color("(&wUnclanned&P)\t", ch);
                else 
                    color.send_to_char("\t\t\t", ch);

                color.pager_printf_color(ch, "&P{0}\r\n", victim.CurrentRoom.Name);
            }
        }

        private static IEnumerable<CharacterInstance> GetVisiblePlayersInArea(CharacterInstance ch)
        {
            List<CharacterInstance> list = new List<CharacterInstance>();
            foreach (RoomTemplate room in ch.CurrentRoom.Area.Rooms)
            {
                foreach (CharacterInstance victim in room.Persons.Where(x => !x.IsNpc()))
                {
                    if (victim.PlayerData != null && victim.PlayerData.Flags.IsSet((int)PCFlags.DoNotDisturb))
                        continue;
                    if (ch.Trust < victim.Trust)
                        continue;
                    if (victim.Descriptor == null ||
                        (victim.Descriptor.ConnectionStatus != ConnectionTypes.Playing ||
                         victim.Descriptor.ConnectionStatus != ConnectionTypes.Editing))
                        continue;
                    if (!ch.CanSee(victim))
                        continue;

                    list.Add(victim);
                }
            }

            return list;
        }
    }
}
