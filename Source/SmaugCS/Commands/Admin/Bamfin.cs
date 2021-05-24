using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
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
