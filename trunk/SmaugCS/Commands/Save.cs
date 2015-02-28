using SmaugCS.Constants;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Commands
{
    public static class Save
    {
        [Command(NoNpc = true)]
        public static void do_save(CharacterInstance ch, string argument)
        {
            if (ch.Level < 2)
            {
                ch.SendTo("%BYou must be at least 2nd level to save.");
                return;
            }

            Macros.WAIT_STATE(ch, 2);
            ch.update_aris();
            save.save_char_obj(ch);
            ch.SendTo("Saved...");
        }
    }
}
