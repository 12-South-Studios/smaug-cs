using SmaugCS.Communication;
using SmaugCS.Data.Instances;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social;

public static class CouncilTalk
{
  public static void do_counciltalk(CharacterInstance ch, string argument)
  {
    if (CheckFunctions.CheckIfNotAuthorized(ch, ch, "Huh?")) return;
    if (CheckFunctions.CheckIfTrue(ch, ch.IsNpc() || ((PlayerInstance)ch).PlayerData.Council == null, "Huh?")) return;

    ChatManager.talk_channel(ch, argument, ChannelTypes.Council, "counciltalk");
  }
}