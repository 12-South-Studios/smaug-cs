using SmaugCS.Constants;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Commands
{
    public static class Save
    {
        [Command(NoNpc = true)]
        public static void do_save(CharacterInstance ch, string argument)
        {
            if (ch.Level < 2)
            {
                color.send_to_char_color("%BYou must be at least 2nd level to save.\r\n", ch);
                return;
            }

            Macros.WAIT_STATE(ch, 2);
            ch.update_aris();
            save.save_char_obj(ch);
            color.send_to_char("Saved...\r\n", ch);
        }
    }
}
