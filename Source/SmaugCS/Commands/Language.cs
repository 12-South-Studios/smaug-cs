using SmaugCS.Data.Instances;

namespace SmaugCS.Commands;

public static class Language
{
  public static void do_languages(CharacterInstance ch, string argument)
  {
    /*string firstArg = argument.FirstWord();

     f (!string.IsNullOrWhiteSpace(firstArg)

         & !firstArg.StartsWith("learn")

         & !ch.IsImmortal()

         & !ch.IsNpc())



         tring secondArg = argument.SecondWord();

         f (string.IsNullOrWhiteSpace(secondArg))



             h.SetColor("Learn what language?\r\n", ch);

             eturn;




         nt foundLanguage =

             ameConstants.LanguageTable.Keys.Where(lang => lang != (int)LanguageTypes.Clan)

                          FirstOrDefault(lang => secondArg.StartsWith(GameConstants.LanguageTable[lang]));





     oreach (int lang in GameConstants.LanguageTable.Keys

         Where(lang => ch.KnowsLanguage(lang, ch) > 0))



         f (((ch.Speaking & lang) > 0) ||

             ch.IsNpc() && ch.Speaking == 0))

            h.SetColor(ATTypes.AT_SAY, ch);

         lse

            h.SetColor(ATTypes.AT_PLAIN, ch);


         h.SetColor(GameConstants.LanguageTable[lang], ch);

         h.SetColor("\r\n", ch);




     h.SetColor("\r\n", ch);*/
  }
}