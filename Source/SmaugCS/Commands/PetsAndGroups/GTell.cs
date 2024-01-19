using Realm.Library.Common.Objects;
using Realm.Standard.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;
using System.Linq;

namespace SmaugCS.Commands
{
    public static class GTell
    {
        public static void do_gtell(CharacterInstance ch, string argument)
        {
#if !SCRAMBLE
            var speaking = -1;
            /*foreach (int key in GameConstants.LanguageTable.Keys
                .Where(key => (key & ch.Speaking) > 0))
            {
                speaking = key;
                break;
            }*/
#endif

            if (string.IsNullOrEmpty(argument))
            {
                ch.SendTo("Tell your group what?\r\n");
                return;
            }

            if (ch.Act.IsSet((int)PlayerFlags.NoTell))
            {
                ch.SendTo("Your message didn't get through!\r\n");
                return;
            }

            foreach (
                var gch in
                    Program.RepositoryManager.CHARACTERS.CastAs<Repository<long, CharacterInstance>>()
                        .Values.Where(x => x.IsSameGroup(ch)))
            {
                gch.SetColor(ATTypes.AT_GTELL);

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
                    gch.Printf("%s tells the group '%s'.\r\n", ch.Name, argument);
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
