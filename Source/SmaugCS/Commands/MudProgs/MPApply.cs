using Realm.Library.Common.Extensions;
using SmaugCS.Common.Enumerations;
using SmaugCS.Communication;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.MudProgs
{
    public static class MPApply
    {
        public static void do_mpapply(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfTrue(ch,
                !ch.IsNpc() || ((PlayerInstance)ch).Descriptor != null || ch.IsAffected(AffectedByTypes.Charm), "Huh?")) return;

            if (CheckFunctions.CheckIfEmptyString(ch, argument, "Mpapply - bad syntax")) return;

            var victim = ch.GetCharacterInRoom(argument);
            if (CheckFunctions.CheckIfNullObject(ch, victim, "Mpapply - no such player in room.")) return;

            if (CheckFunctions.CheckIfNullObject(ch, !victim.IsNpc() && ((PlayerInstance)victim).Descriptor != null, "Not on link-dead players")) return;

            if (!victim.IsNotAuthorized()) return;
            if (!victim.IsNpc() && ((PlayerInstance)victim).PlayerData.AuthState != AuthorizationStates.None) return;

            var buf =
                $"{victim.Name}@{(victim.IsNpc() ? string.Empty : ((PlayerInstance)victim).Descriptor.host)} new {victim.CurrentRace.GetName()} {victim.CurrentClass.GetName()} {(victim.IsPKill() ? "(Deadly)" : "(Peaceful)")} applying...";

            ChatManager.to_channel(buf, ChannelTypes.Auth, "Auth", LevelConstants.ImmortalLevel);
            if (!victim.IsNpc())
                ((PlayerInstance)victim).PlayerData.AuthState = AuthorizationStates.None;
        }
    }
}
