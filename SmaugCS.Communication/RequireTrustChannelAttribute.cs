using SmaugCS.Data;

namespace SmaugCS.Communication
{
    public abstract class RequireTrustChannelAttribute : ChannelAttribute
    {
        public string TrustType { get; set; }

        protected RequireTrustChannelAttribute(string TrustType = "")
        {
            this.TrustType = TrustType;
        }

        public override bool Verify(ChannelTypes channelType, CharacterInstance ch, int minTrust = 0)
        {
            return ch.Trust >= minTrust;
        }
    }
}
