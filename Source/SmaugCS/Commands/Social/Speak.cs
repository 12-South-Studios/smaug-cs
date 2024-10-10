using Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Language;

namespace SmaugCS.Commands.Social;

public static class Speak
{
  public static void do_speak(CharacterInstance ch, string argument)
  {
    string firstArg = argument.FirstWord();
    if (firstArg.EqualsIgnoreCase("all")
        || ch.IsImmortal())
    {
      ch.SetColor(ATTypes.AT_SAY);
      ch.Speaking = ~(int)LanguageTypes.Clan;
      ch.SendTo("Now speaking all languages.\r\n");
      return;
    }
    /*foreach (int lang in GameConstants.LanguageTable.Keys)



         f (GameConstants.LanguageTable[lang].StartsWith(firstArg))



             f (ch.KnowsLanguage(lang, ch) > 0)



                 f (lang == (int)LanguageTypes.Clan

                     & (ch.IsNpc() || ch.PlayerData.Clan == null))

                     ontinue;


                 h.Speaking = lang;

                h.SetColor(ATTypes.AT_SAY, ch);

                 olor.ch_printf(ch, "You now speak %s.\r\n", GameConstants.LanguageTable[lang]);

                 eturn;





     */

    ch.SetColor(ATTypes.AT_SAY);
    ch.SendTo("You do not know that language.\r\n");
  }
}