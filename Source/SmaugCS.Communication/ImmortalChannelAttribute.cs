using SmaugCS.Data.Instances;
using System;

namespace SmaugCS.Communication;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ImmortalChannelAttribute : RequireTrustChannelAttribute
{
    public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
        => ch.IsImmortal() && base.Verify(channelType, ch, minTrust);
}