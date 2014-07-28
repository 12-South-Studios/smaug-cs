using System;

namespace SmaugCS.Constants.Enums
{
    [Flags]
    public enum ConditionTypes
    {
        [Descriptor(new[]
        {
            "You are sober.\r\n",
            "You are feeling a little less light headed.\r\n"
        })] 
        Drunk = 1 << 0,

        [Descriptor(new[]
        {
            "You are STARVING!\r\n",
            "$n is starved half to death!",
            "You are really hungry.\r\n",
            "You can hear $n's stomach growling.",
            "You are hungry.\r\n",
            "",
            "You are a mite peckish.\r\n",
            ""
        })] 
        Full = 1 << 1,

        [Descriptor(new[]
        {
            "You are DYING of THIRST!\r\n",
            "$n is dying of thirst!",
            "You are really thirsty.\r\n",
            "$n looks a little parched.",
            "You are thirsty.\r\n",
            "",
            "You could use a sip of something refreshing.\r\n",
            ""
        })] 
        Thirsty = 1 << 2,

        [Descriptor(new[]
        {
            "You are starved to feast on blood!\r\n",
            "$n is suffering from lack of blood!",
            "You have a growing need to feast on blood!\r\n",
            "$n gets a strange look in $s eyes...",
            "You feel an urgent need for blood.\r\n",
            "",
            "You feel an aching in your fangs.\r\n",
            ""
        })] 
        Bloodthirsty = 1 << 3
    }
}
