using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands
{
    public static class Ide
    {
        public static void do_ide(CharacterInstance ch, string argument)
        {
            color.set_char_color(ATTypes.AT_PLAIN, ch);
            color.send_to_char("\r\nIf you want to send an idea, type 'idea <message>'.\r\n", ch);
            color.send_to_char("If you want to identify an object, use the identify spell.\r\n", ch);
        }
    }
}
