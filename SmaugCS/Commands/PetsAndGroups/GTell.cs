using System.Linq;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

using SmaugCS.Managers;

namespace SmaugCS.Commands.PetsAndGroups
{
    public static class GTell
    {
        public static void do_gtell(CharacterInstance ch, string argument)
        {
#if !SCRAMBLE
            int speaking = -1;
            /*foreach (int key in GameConstants.LanguageTable.Keys
                .Where(key => (key & ch.Speaking) > 0))
            {
                speaking = key;
                break;
            }*/
#endif

            if (string.IsNullOrEmpty(argument))
            {
                color.send_to_char("Tell your group what?\r\n", ch);
                return;
            }

            if (ch.Act.IsSet((int)PlayerFlags.NoTell))
            {
                color.send_to_char("Your message didn't get through!\r\n", ch);
                return;
            }

            foreach (CharacterInstance gch in DatabaseManager.Instance.CHARACTERS.CastAs<Repository<long, CharacterInstance>>().Values.Where(x => x.IsSameGroup(ch)))
            {
                color.set_char_color(ATTypes.AT_GTELL, gch);

#if !SCRAMBLE
                if (speaking != -1 && (!ch.IsNpc() || ch.Speaking > 0))
                {
                    /*int speakswell = gch.KnowsLanguage(ch.Speaking, ch).GetLowestOfTwoNumbers(ch.KnowsLanguage(ch.Speaking, gch));
                    color.ch_printf(gch, "%s tells the group '%s'.\r\n", ch.Name,
                                    speakswell < 85
                                        ? act_comm.TranslateLanguage(speakswell, argument, GameConstants.LanguageTable[speaking])
                                        : argument);*/
                }
                else
                    color.ch_printf(gch, "%s tells the group '%s'.\r\n", ch.Name, argument);
#else
                if (act_comm.KnowsLanguage(gch, ch.Spekaing, gch) || (ch.IsNpc() && ch.Speaking == 0))
                    color.ch_printf(gch, "%s tells the group '%s'.\r\n", ch.Name, argument);
                else 
                    color.ch_printf(gch, "%s tells the group '%s'.\r\n", ch.Name, act_comm.scramble(argument, ch.Speaking));
#endif
            }
        }
    }
}
