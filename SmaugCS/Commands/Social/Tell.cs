using System;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Commands.Social
{
    public static class Tell
    {
        public static void do_tell(CharacterInstance ch, string argument)
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

            ch.Deaf.RemoveBit((int)ChannelTypes.Tells);
            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.Silence))
            {
                color.send_to_char("You can't do that here.\r\n", ch);
                return;
            }

            if (!ch.IsNpc() &&
                (ch.Act.IsSet((int)PlayerFlags.Silence) || ch.Act.IsSet((int)PlayerFlags.NoTell)))
            {
                color.send_to_char("You can't do that.\r\n", ch);
                return;
            }

            string firstArgument = argument.FirstWord();
            string argumentString = argument.RemoveWord(1);
            if (string.IsNullOrWhiteSpace(argumentString))
            {
                color.send_to_char("Tell whom what?\r\n", ch);
                return;
            }

            CharacterInstance victim = handler.get_char_world(ch, firstArgument);
            if (victim == null ||
                (victim.IsNpc() && victim.CurrentRoom != ch.CurrentRoom)
                || (!Macros.NOT_AUTHORIZED(ch) && Macros.NOT_AUTHORIZED(victim) && !ch.IsImmortal()))
            {
                color.send_to_char("They aren't here.\r\n", ch);
                return;
            }

            if (ch == victim)
            {
                color.send_to_char("You have a nice little chat with yourself.\r\n", ch);
                return;
            }

            if (Macros.NOT_AUTHORIZED(ch) && !Macros.NOT_AUTHORIZED(victim) && !victim.IsImmortal())
            {
                color.send_to_char("They can't hear you because you are not authorized.\r\n", ch);
                return;
            }

            if (!victim.IsNpc() && victim.Switched != null
                && (ch.Trust > Program.LEVEL_AVATAR)
                && !victim.Switched.IsAffected(AffectedByTypes.Possess))
            {
                color.send_to_char("That player is switched.\r\n", ch);
                return;
            }

            CharacterInstance switchedVictim = null;
            if (!victim.IsNpc() && victim.Switched != null
                && victim.Switched.IsAffected(AffectedByTypes.Possess))
                switchedVictim = victim.Switched;
            else if (!victim.IsNpc() && victim.Descriptor == null)
            {
                color.send_to_char("That player is link-dead.\r\n", ch);
                return;
            }

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.AwayFromKeyboard))
            {
                color.send_to_char("That player is afk.\r\n", ch);
                return;
            }

            if (victim.Deaf.IsSet((int)ChannelTypes.Tells)
                && (!ch.IsImmortal() || ch.Trust < victim.Trust))
            {
                comm.act(ATTypes.AT_PLAIN, "$E has $S tells turned off.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Silence))
                color.send_to_char("That player is silenced. They will receive your message but cannot respond.\r\n", ch);

            if (!ch.IsImmortal() && !victim.IsAwake())
            {
                comm.act(ATTypes.AT_PLAIN, "$E is too tired to discuss such matters with you.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (!victim.IsNpc() && victim.CurrentRoom.Flags.IsSet((int)RoomFlags.Silence))
            {
                comm.act(ATTypes.AT_PLAIN, "A magic force prevents your message from being heard.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (victim.Descriptor != null
                && victim.Descriptor.ConnectionStatus == ConnectionTypes.Editing
                && ch.Trust < Program.LEVEL_GOD)
            {
                comm.act(ATTypes.AT_PLAIN, "$E is currently in a writing buffer. Please try again later.", ch, null, victim, ToTypes.Character);
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

            ch.RetellTo = victim;

            if (!victim.IsNpc() && victim.IsImmortal()
                && victim.PlayerData.TellHistory != null
                && Char.IsLetter(ch.IsNpc() ? ch.ShortDescription.ToCharArray()[0] : ch.Name.ToCharArray()[0]))
            {
                string buffer = string.Format("{0} told you '{1}'\r\n",
                                              ch.IsNpc()
                                                  ? ch.ShortDescription.CapitalizeFirst()
                                                  : ch.Name.CapitalizeFirst(),
                                                  argumentString);
                victim.PlayerData.TellHistory.Add(buffer);
            }

            if (switchedVictim != null)
                victim = switchedVictim;

            //MOBTrigger = false;

            comm.act(ATTypes.AT_TELL, "You tell $N '$t'", ch, argumentString, victim, ToTypes.Character);
            PositionTypes position = victim.Position;
            victim.Position = PositionTypes.Standing;

            if (speaking != -1 && (!ch.IsNpc() || ch.Speaking > 0))
            {
                int speakswell = SmaugCS.Common.Check.Minimum(victim.KnowsLanguage(ch.Speaking, ch),
                                                  ch.KnowsLanguage(ch.Speaking, victim));
                if (speakswell < 85)
                    comm.act(ATTypes.AT_TELL, "$n tells you '$t'", ch, act_comm.TranslateLanguage(speakswell, argumentString,
                        GameConstants.LanguageTable[speaking]), victim, ToTypes.Victim);
                else
                    comm.act(ATTypes.AT_TELL, "$n tells you '$t'", ch, argumentString, victim, ToTypes.Victim);
            }
            else
                comm.act(ATTypes.AT_TELL, "$n tells you '$t'", ch, argumentString, victim, ToTypes.Victim);

            //MOBtrigger = true;

            victim.Position = position;
            victim.ReplyTo = ch;

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.LogSpeech))
            {
                string buffer = string.Format("{0}: {1} (tell to) {2}",
                                              ch.IsNpc() ? ch.ShortDescription : ch.Name,
                                              argumentString,
                                              victim.IsNpc() ? victim.ShortDescription : victim.Name);
                db.append_to_file(SystemConstants.GetSystemFile(SystemFileTypes.Log), buffer);
            }

            mud_prog.mprog_tell_trigger(argumentString, ch);
        }
    }
}
