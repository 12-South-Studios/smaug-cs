using System;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Language;

namespace SmaugCS.Commands.Social
{
    public static class Speak
    {
        public static void do_speak(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (firstArg.Equals("all", StringComparison.OrdinalIgnoreCase)
                || ch.IsImmortal())
            {
                color.set_char_color(ATTypes.AT_SAY, ch);
                ch.Speaking = ~(int)LanguageTypes.Clan;
                color.send_to_char("Now speaking all languages.\r\n", ch);
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
                        color.set_char_color(ATTypes.AT_SAY, ch);
                        color.ch_printf(ch, "You now speak %s.\r\n", GameConstants.LanguageTable[lang]);
                        return;
                    }
                }
            }*/

            color.set_char_color(ATTypes.AT_SAY, ch);
            color.send_to_char("You do not know that language.\r\n", ch);
        }
    }
}
