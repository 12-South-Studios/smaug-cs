using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Commands.Player
{
    public static class Score
    {
        public static void do_score(CharacterInstance ch, string argument)
        {
            ch.SetPagerColor(ATTypes.AT_SCORE);
            ch.Printf("\r\nScore for {0}{1}.\r\n", ch.Name, ((PlayerInstance)ch).PlayerData.Title);
            if (ch.Trust != ch.Level)
                ch.PagerPrintf("You are trusted at level {0}.\r\n", ch.Trust);


            // TODO
        }
    }
}
