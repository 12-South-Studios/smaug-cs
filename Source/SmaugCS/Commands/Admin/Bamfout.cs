using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Admin
{
    public static class Bamfout
    {
        public static void do_bamfout(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNpc(ch, ch)) return;

            //smash_tilde(argument);
            ((PlayerInstance)ch).PlayerData.bamfout = argument;
            ch.SendTo("&YBamfout set.");
        }
    }
}
