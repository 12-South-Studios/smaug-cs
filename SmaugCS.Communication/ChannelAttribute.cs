using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Communication
{
    public abstract class ChannelAttribute : Attribute
    {
        public abstract bool Verify(ChannelTypes channelType, CharacterInstance ch, int minTrust = 0);
    }
}
