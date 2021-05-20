using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Organizations;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Commands.Social
{
    public static class Where
    {
        public static void do_where(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (string.IsNullOrEmpty(firstArg))
                ViewPlayersNearby(ch);
            else
                ViewIndividualPlayer(ch, firstArg);
        }

        private static void ViewIndividualPlayer(CharacterInstance ch, string firstArg)
        {
            var victim = ch.GetCharacterInWorld(firstArg);
            if (victim?.CurrentRoom != null
                && victim.CurrentRoom.Area == ch.CurrentRoom.Area
                && !victim.IsAffected(AffectedByTypes.Hide)
                && !victim.IsAffected(AffectedByTypes.Sneak)
                && ch.CanSee(victim))
            {
                ch.PagerPrintf("{0} {1}", Macros.PERS(victim, ch).PadRight(28, ' '), victim.CurrentRoom.Name);
                return;
            }

            comm.act(ATTypes.AT_PLAIN, "You didn't find any $T.", ch, null, firstArg, ToTypes.Character);
        }

        private static void ViewPlayersNearby(CharacterInstance ch)
        {
            ch.PagerPrintf("Players near you in {0}:", ch.CurrentRoom.Area.Name);

            var victimList = GetVisiblePlayersInArea(ch);
            if (!victimList.Any())
            {
                ch.SendTo("None");
                return;
            }

            foreach (var victim in victimList)
            {
                ch.PagerPrintfColor("&P{0}  ", victim.Name.PadRight(13, ' '));
                if (victim.IsImmortal() && victim.Level > LevelConstants.AvatarLevel)
                   ch.SendToPagerColor("&P(&WImmortal&P)\t");
                else if (victim.CanPKill() && ((PlayerInstance)victim).PlayerData.Clan != null
                         && ((PlayerInstance)victim).PlayerData.Clan.ClanType != ClanTypes.Order
                         && ((PlayerInstance)victim).PlayerData.Clan.ClanType != ClanTypes.Guild)
                    ch.PagerPrintfColor("{0}\t", ((PlayerInstance)victim).PlayerData.Clan.Badge.PadRight(18, ' '));
                else if (victim.CanPKill())
                    ch.SendToPagerColor("(&wUnclanned&P)\t");
                else
                    ch.SendTo("\t\t\t");

                ch.PagerPrintfColor("&P{0}\r\n", victim.CurrentRoom.Name);
            }
        }

        private static IEnumerable<CharacterInstance> GetVisiblePlayersInArea(CharacterInstance ch)
        {
            var list = new List<CharacterInstance>();
            foreach (var room in ch.CurrentRoom.Area.Rooms)
            {
                foreach (var victim in room.Persons.Where(x => !x.IsNpc()))
                {
                    if (((PlayerInstance)victim).PlayerData != null && ((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.DoNotDisturb))
                        continue;
                    if (ch.Trust < victim.Trust)
                        continue;
                    if (((PlayerInstance)victim).Descriptor == null || ((PlayerInstance)victim).Descriptor.ConnectionStatus != ConnectionTypes.Playing || ((PlayerInstance)victim).Descriptor.ConnectionStatus != ConnectionTypes.Editing)
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
