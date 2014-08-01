using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.MudProgs
{
    public static class MPApply
    {
        public static void do_mpapply(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch,
                !ch.IsNpc() || ch.Descriptor != null || ch.IsAffected(AffectedByTypes.Charm), "Huh?")) return;

            if (CheckFunctions.CheckIfEmptyString(ch, argument, "Mpapply - bad syntax")) return;

            CharacterInstance victim = handler.get_char_room(ch, argument);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "Mpapply - no such player in room.")) return;

            if (CheckFunctions.CheckIfNullObject(ch, victim.Descriptor, "Not on link-dead players")) return;

            if (!victim.IsNotAuthorized()) return;
            if (victim.PlayerData.AuthState >= -1) return;

            string buf = string.Format("{0}@{1} new {2} {3} {4} applying...",
                victim.Name, victim.Descriptor.host, victim.CurrentRace.GetName(),
                victim.CurrentClass.GetName(), victim.IsPKill() ? "(Deadly)" : "(Peaceful)");

            ChatManager.to_channel(buf, ChannelTypes.Auth, "Auth", LevelConstants.GetLevel("Immortal"));
            victim.PlayerData.AuthState = -1;
        }
    }
}
