using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Idea
    {
        public static void do_idea(CharacterInstance ch, string argument)
        {
            ch.SetColor(ATTypes.AT_PLAIN);
            if (CheckFunctions.CheckIfEmptyString(ch, argument, "Usage:  'idea <message>'\r\n")) return;

            // TODO
            // db.append_file(ch, SystemConstants.GetSystemFile(SystemFileTypes.Idea), argument);
            ch.SendTo("Thanks, your idea has been recorded.");
        }
    }
}
