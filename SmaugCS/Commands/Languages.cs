using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Language;

namespace SmaugCS.Commands
{
    public static class Languages
    {
        public static void do_languages(CharacterInstance ch, string argument)
        {
            string firstArg = argument.FirstWord();
            if (!string.IsNullOrWhiteSpace(firstArg)
                && !firstArg.StartsWith("learn")
                && !ch.IsImmortal()
                && !ch.IsNpc())
            {
                string secondArg = argument.SecondWord();
                if (string.IsNullOrWhiteSpace(secondArg))
                {
                    color.send_to_char("Learn what language?\r\n", ch);
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
                    color.set_char_color(ATTypes.AT_SAY, ch);
                else
                    color.set_char_color(ATTypes.AT_PLAIN, ch);

                color.send_to_char(GameConstants.LanguageTable[lang], ch);
                color.send_to_char("\r\n", ch);
            }

            color.send_to_char("\r\n", ch);
        }
    }
}
