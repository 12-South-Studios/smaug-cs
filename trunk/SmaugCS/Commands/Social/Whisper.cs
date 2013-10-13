using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Commands.Social
{
    public static class Whisper
    {
        public static void do_whisper(CharacterInstance ch, string argument)
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

            ch.Deaf.RemoveBit((int)ChannelTypes.Whisper);
            string firstArgument = argument.FirstWord();
            string argumentString = argument.RemoveWord(1);
            if (string.IsNullOrWhiteSpace(argumentString))
            {
                color.send_to_char("Whisper to whom what?\r\n", ch);
                return;
            }

            CharacterInstance victim = handler.get_char_room(ch, firstArgument);
            if (victim == null)
            {
                color.send_to_char("They aren't here.\r\n", ch);
                return;
            }

            if (ch == victim)
            {
                color.send_to_char("You have a nice little chat with yourself.\r\n", ch);
                return;
            }

            if (!victim.IsNpc() && victim.Switched != null &&
                !victim.Switched.IsAffected(AffectedByTypes.Possess))
            {
                color.send_to_char("That player is switched.\r\n", ch);
                return;
            }

            if (!victim.IsNpc() && victim.Descriptor == null)
            {
                color.send_to_char("That player is link-dead.\r\n", ch);
                return;
            }

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.AwayFromKeyboard))
            {
                color.send_to_char("That player is afk.\r\n", ch);
                return;
            }

            if (victim.Deaf.IsSet((int)ChannelTypes.Whisper) &&
                (!ch.IsImmortal() || (ch.Trust < victim.Trust)))
            {
                comm.act(ATTypes.AT_PLAIN, "$E has $S whispers turned off.", ch, null, victim,
                         ToTypes.Character);
                return;
            }

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Silence))
                color.send_to_char("That player is silenced.  They will receive your message but cannot respond.\r\n", ch);

            if (victim.Descriptor != null
                && (victim.Descriptor.ConnectionStatus == ConnectionTypes.Editing)
                && (ch.Trust < Program.LEVEL_GOD))
            {
                comm.act(ATTypes.AT_PLAIN, "$E is currently in a writing buffer.  Please try again in a few minutes.",
                    ch, 0, victim, ToTypes.Character);
                return;
            }

            if (victim.IsIgnoring(ch))
            {
                if (!ch.IsImmortal() || victim.Trust > ch.Trust)
                {
                    color.set_char_color(ATTypes.AT_IGNORE, ch);
                    color.ch_printf(ch, "%s is ignoring you.\r\n", victim.Name);
                    return;
                }

                color.set_char_color(ATTypes.AT_IGNORE, victim);
                color.ch_printf(victim, "You attempt to ignore %s, but are unable to do so.\r\n", ch.Name);
            }

            comm.act(ATTypes.AT_WHISPER, "You whisper to $N '$t'", ch, argumentString, victim, ToTypes.Character);
            PositionTypes position = victim.Position;
            victim.Position = PositionTypes.Standing;

#if !SCRAMBLE
            if (speaking != -1 && (!ch.IsNpc() || ch.Speaking > 0))
            {
                int speakswell = SmaugCS.Common.Check.Minimum(victim.KnowsLanguage(ch.Speaking, ch),
                                             ch.KnowsLanguage(ch.Speaking, victim));
                if (speakswell < 85)
                    comm.act(ATTypes.AT_WHISPER, "$n whispers to you '$t'",
                             ch, act_comm.TranslateLanguage(speakswell, argumentString,
                                           GameConstants.LanguageTable[speaking]), victim, ToTypes.Victim);
                else
                    comm.act(ATTypes.AT_WHISPER, "$n whispers to you '$t'",
                             ch, argumentString, victim, ToTypes.Victim);
            }
            else
                comm.act(ATTypes.AT_WHISPER, "$n whispers to you '$t'", ch, argument, victim, ToTypes.Victim);
#else
            int speakswell = SmaugCS.Common.Check.Minimum(KnowsLanguage(victim, ch.Speaking, ch),
                                              KnowsLanguage(ch, ch.Speaking, victim));

            if (act_comm.KnowsLanguage(victim, ch.Speaking, ch) == 0
                && (!ch.IsNpc() || ch.Speaking != 0))
                comm.act(ATTypes.AT_WHISPER, "$n whispers to you '$t'", ch,
                         TranslateLanguage(speakswell, argument, GameConstants.LanguageTable[speaking]),
                         victim, ToTypes.Victim);
            else
                comm.act(ATTypes.AT_WHISPER, "$n whispers something to $N.",
                         ch, argument, victim, ToTypes.NotVictim);
#endif

            if (!ch.CurrentRoom.Flags.IsSet((int)RoomFlags.LogSpeech))
                db.append_to_file(SystemConstants.GetSystemFile(SystemFileTypes.Log),
                                  string.Format("{0}: {1} (whisper to) {2}",
                                                ch.IsNpc() ? ch.ShortDescription : ch.Name,
                                                argument, victim.IsNpc() ? victim.ShortDescription : victim.Name));

            mud_prog.mprog_tell_trigger(argument, ch);
        }
    }
}
