using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Language;

namespace SmaugCS.Commands.Social
{
    public static class Speak
    {
        public static void do_speak(CharacterInstance ch, string argument)
        {
            var firstArg = argument.FirstWord();
            if (firstArg.EqualsIgnoreCase("all")
                || ch.IsImmortal())
            {
               ch.SetColor(ATTypes.AT_SAY);
                ch.Speaking = ~(int)LanguageTypes.Clan;
                ch.SendTo("Now speaking all languages.\r\n");
                return;
            }

            /*foreach (int lang in GameConstants.LanguageTable.Keys)
            {
                if (GameConstants.LanguageTable[lang].StartsWith(firstArg))
                {
                    if (ch.KnowsLanguage(lang, ch) > 0)
                    {
                        if (lang == (int)LanguageTypes.Clan
                            && (ch.IsNpc() || ch.PlayerData.Clan == null))
                            continue;

                        ch.Speaking = lang;
                       ch.SetColor(ATTypes.AT_SAY, ch);
                        color.ch_printf(ch, "You now speak %s.\r\n", GameConstants.LanguageTable[lang]);
                        return;
                    }
                }
            }*/

           ch.SetColor(ATTypes.AT_SAY);
           ch.SendTo("You do not know that language.\r\n");
        }
    }
}
