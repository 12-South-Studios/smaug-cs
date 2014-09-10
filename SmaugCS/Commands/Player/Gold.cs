using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands.Player
{
    public static class Gold
    {
        public static void do_gold(CharacterInstance ch, string argument)
        {
            color.set_char_color(ATTypes.AT_GOLD, ch);
            color.ch_printf(ch, "You have {0} gold pieces.", ch.CurrentCoin.ToPunctuation());
        }
    }
}
