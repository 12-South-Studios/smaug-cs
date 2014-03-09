using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands
{
    public static class Rent
    {
        public static void do_rent(CharacterInstance ch, string argument)
        {
            color.set_char_color(ATTypes.AT_WHITE, ch);
            color.send_to_char("There is no rent here. Just save and quit.\r\n", ch);
        }
    }
}
