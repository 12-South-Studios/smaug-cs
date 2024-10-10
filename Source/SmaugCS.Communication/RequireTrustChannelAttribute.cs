using SmaugCS.Data.Instances;

namespace SmaugCS.Communication;

public abstract class RequireTrustChannelAttribute(string trustType = "") : ChannelAttribute
{
    public string TrustType { get; set; } = trustType;

    public override bool Verify(ChannelTypes channelType, PlayerInstance ch, int minTrust = 0)
        => ch.Trust >= minTrust;
}