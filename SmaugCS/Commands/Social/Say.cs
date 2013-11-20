using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Commands.Social
{
    public static class Say
    {
        public static void do_say(CharacterInstance ch, string argument)
        {
#if !SCRAMBLE
            int speaking = -1;
            foreach (int key in GameConstants.LanguageTable.Keys
                .Where(key => (key & ch.Speaking) > 0))
            {
                speaking = key;
                break;
            }
#endif

            if (string.IsNullOrEmpty(argument))
            {
                color.send_to_char("Say what?\r\n", ch);
                return;
            }

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.Silence))
            {
                color.send_to_char("You can't do that here.\r\n", ch);
                return;
            }

            ExtendedBitvector actflags = ch.Act;
            if (ch.IsNpc())
                ch.Act.RemoveBit((int)ActFlags.Secretive);

            foreach (CharacterInstance vch in ch.CurrentRoom.Persons.Where(vch => vch != ch))
            {
                string sbuf = argument;
                if (vch.IsIgnoring(ch))
                {
                    if (!ch.IsImmortal() || vch.Trust > ch.Trust)
                        continue;

                    color.set_char_color(ATTypes.AT_IGNORE, vch);
                    color.ch_printf(vch, "You attempt to ignore %s, but are unable to do so.\r\n", ch.Name);
                }

#if !SCRAMBLE
                if (speaking != -1 && (!ch.IsNpc() || ch.Speaking != 0))
                {
                    int speakswell = vch.KnowsLanguage(ch.Speaking, ch).GetLowestOfTwoNumbers(ch.KnowsLanguage(ch.Speaking, vch));
                    if (speakswell < 75)
                        sbuf = act_comm.TranslateLanguage(speakswell, argument, GameConstants.LanguageTable[speaking]);
                }
#else
                if (KnowsLanguage(vch, ch.Speaking, ch) == 0 &&
                    (!ch.IsNpc() || ch.Speaking != 0))
                    sbuf = ScrambleText(argument, ch.Speaking);
#endif
                sbuf = sbuf.Drunkify(ch);

                // TODO Toggle global mobtrigger flag
                comm.act(ATTypes.AT_SAY, "$n says '$t'", ch, sbuf, vch, ToTypes.Victim);
            }

            ch.Act = actflags;
            // TODO Toggle global mobtrigger flag
            comm.act(ATTypes.AT_SAY, "You say '%T'", ch, null, argument.Drunkify(ch), ToTypes.Character);

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.LogSpeech))
            {
                db.append_to_file(SystemConstants.GetSystemFile(SystemFileTypes.Log),
                                  string.Format("{0}: {1}", ch.IsNpc() ? ch.ShortDescription : ch.Name, argument));
            }

            mud_prog.mprog_speech_trigger(argument, ch);
            if (ch.CharDied())
                return;
            mud_prog.oprog_speech_trigger(argument, ch);
            if (ch.CharDied())
                return;
            mud_prog.rprog_speech_trigger(argument, ch);
        }
    }
}
