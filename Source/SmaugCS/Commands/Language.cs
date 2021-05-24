using SmaugCS.Data.Instances;

namespace SmaugCS.Commands
{
    public static class Language
    {
        public static void do_languages(CharacterInstance ch, string argument)
        {
            /*string firstArg = argument.FirstWord();
            if (!string.IsNullOrWhiteSpace(firstArg)
                && !firstArg.StartsWith("learn")
                && !ch.IsImmortal()
                && !ch.IsNpc())
            {
                string secondArg = argument.SecondWord();
                if (string.IsNullOrWhiteSpace(secondArg))
                {
                    ch.SetColor("Learn what language?\r\n", ch);
                    return;
                }

                int foundLanguage =
                    GameConstants.LanguageTable.Keys.Where(lang => lang != (int)LanguageTypes.Clan)
                                 .FirstOrDefault(lang => secondArg.StartsWith(GameConstants.LanguageTable[lang]));

            }

            foreach (int lang in GameConstants.LanguageTable.Keys
                .Where(lang => ch.KnowsLanguage(lang, ch) > 0))
            {
                if (((ch.Speaking & lang) > 0) ||
                    (ch.IsNpc() && ch.Speaking == 0))
                   ch.SetColor(ATTypes.AT_SAY, ch);
                else
                   ch.SetColor(ATTypes.AT_PLAIN, ch);

                ch.SetColor(GameConstants.LanguageTable[lang], ch);
                ch.SetColor("\r\n", ch);
            }

            ch.SetColor("\r\n", ch);*/
        }
    }
}
