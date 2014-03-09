using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands
{
    public static class Idea
    {
        public static void do_idea(CharacterInstance ch, string argument)
        {
            color.set_char_color(ATTypes.AT_PLAIN, ch);
            if (string.IsNullOrWhiteSpace(argument))
            {
                color.send_to_char("\r\nUsage:  'idea <message>'\r\n", ch);
                return;
            }

            db.append_file(ch, SystemConstants.GetSystemFile(SystemFileTypes.Idea), argument);
            color.send_to_char("Thanks, your idea has been recorded.\r\n", ch);
        }
    }
}
