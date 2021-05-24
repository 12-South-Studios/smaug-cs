using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
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
