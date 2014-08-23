using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
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
            if (CheckFunctions.CheckIfTrue(ch, ch.PlayerData.Favor < ch.PlayerData.CurrentDeity.SRecall,
                "Your favor is inadequate for such a supplication.")) return;
            if (CheckFunctions.CheckIfSet(ch, ch.CurrentRoom.Flags, RoomFlags.NoSupplicate, "You have been forsaken!"))
                return;

            TimerData timer = ch.GetTimer(TimerTypes.RecentFight);
            if (CheckFunctions.CheckIfTrue(ch, timer != null && !ch.IsImmortal(),
                "You cannot supplicate recall under adrenaline!")) return;

            RoomTemplate location = null;

            if (!ch.IsNpc() && ch.PlayerData.Clan != null)
                location = DatabaseManager.Instance.ROOMS.Get(ch.PlayerData.Clan.RecallRoom);

            if (!ch.IsNpc() && location == null && ch.Level >= 5 && ch.PlayerData.Flags.IsSet(PCFlags.Deadly))
                location = DatabaseManager.Instance.ROOMS.Get(VnumConstants.ROOM_VNUM_DEADLY);

            // TODO Race Recall Room

            if (location == null)
                location = DatabaseManager.Instance.ROOMS.Get(VnumConstants.ROOM_VNUM_TEMPLE);

            if (CheckFunctions.CheckIfNullObject(ch, location, "You are completely lost.")) return;

            // TODO More
        }
        private static void SupplicateForAvatar(CharacterInstance ch, string argument)
        {

        }
        private static void SupplicateForObject(CharacterInstance ch, string argument)
        {

        }
        private static void SupplicateForRecall(CharacterInstance ch, string argument)
        {

        }
    }
}
