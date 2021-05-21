using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Commands.Admin
{
    public static class Bamfin
    {
        public static void do_bamfin(CharacterInstance ch, string argument)
        {
            if (CheckFunctions.CheckIfNpc(ch, ch)) return;

            //smash_tilde(argument);
            ((PlayerInstance)ch).PlayerData.bamfin = argument;
            ch.SendTo("&YBamfin set.");
        }
    }
}
