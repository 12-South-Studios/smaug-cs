using SmaugCS.Communication;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;

namespace SmaugCS.Commands.Social;

public static class Muse
{
  public static void do_muse(CharacterInstance ch, string argument)
  {
    ChatManager.SendToChat(ch, argument, ChannelTypes.Highgod, "muse");
  }
}