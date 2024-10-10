using SmaugCS.Data;
using SmaugCS.Data.Instances;
using System;

namespace SmaugCS.Communication;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ClanChannelAttribute : ChannelAttribute
{
  public bool NoNpc { get; set; }
  public ClanTypes ClanType { get; set; }

  public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
  {
    if (ch.PlayerData.Clan == null)
      return false;

    if (NoNpc && ch.IsNpc())
      return false;

    return ClanType == ch.PlayerData.Clan.ClanType;
  }
}