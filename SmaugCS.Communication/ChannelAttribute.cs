using SmaugCS.Data;
using System;

namespace SmaugCS.Communication
{
    public abstract class ChannelAttribute : Attribute
    {
        public abstract bool Verify(ChannelTypes channelType, CharacterInstance ch, int minTrust = 0);
    }
}
