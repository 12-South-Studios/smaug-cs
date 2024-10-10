using SmaugCS.Data.Instances;
using System;

namespace SmaugCS.Communication;

public abstract class ChannelAttribute : Attribute
{
    public abstract bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0);
}