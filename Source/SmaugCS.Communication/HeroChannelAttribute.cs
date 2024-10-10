using SmaugCS.Data.Instances;
using System;

namespace SmaugCS.Communication;

[AttributeUsage(AttributeTargets.Field)]
public sealed class HeroChannelAttribute : ChannelAttribute
{
    public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0) => ch.IsHero();
}