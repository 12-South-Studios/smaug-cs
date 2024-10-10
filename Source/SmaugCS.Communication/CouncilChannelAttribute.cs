using SmaugCS.Data.Instances;
using System;

namespace SmaugCS.Communication;

[AttributeUsage(AttributeTargets.Field)]
public sealed class CouncilChannelAttribute : ChannelAttribute
{
  public bool NoNpc { get; set; }

  public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
  {
    if (ch.PlayerData.Council == null)
      return false;

    return !NoNpc || !ch.IsNpc();
  }
}