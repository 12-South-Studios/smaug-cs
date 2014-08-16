using SmaugCS.Common;
using SmaugCS.Communication;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Commands.Social
{
    public static class Reply
    {
        public static void do_reply(CharacterInstance ch, string argument)
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

            ch.Deaf.RemoveBit((int)ChannelTypes.Tells);
            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.Silence))
            {
                color.send_to_char("You can't do that here.\r\n", ch);
                return;
            }

            if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.Silence))
            {
                color.send_to_char("Your message didn't get through.\r\n", ch);
                return;
            }

            CharacterInstance victim = ch.ReplyTo;
            if (victim == null)
            {
                color.send_to_char("They aren't here.\r\n", ch);
                return;
            }

            if (!victim.IsNpc()
                && victim.Switched != null
                && ch.CanSee(victim)
                && ch.Trust > LevelConstants.AvatarLevel)
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

            if (victim.Deaf.IsSet((int)ChannelTypes.Tells)
                && (!ch.IsImmortal() || ch.Trust < victim.Trust))
            {
                comm.act(ATTypes.AT_PLAIN, "$E has $S tells turned off.", ch, null, victim, ToTypes.Character);
                return;
            }

            if ((!ch.IsImmortal() && !victim.IsAwake())
                || (!victim.IsNpc() && victim.CurrentRoom.Flags.IsSet((int)RoomFlags.Silence)))
            {
                comm.act(ATTypes.AT_PLAIN, "$E can't hear you.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (victim.Descriptor != null
                && victim.Descriptor.ConnectionStatus == ConnectionTypes.Editing
                && ch.Trust < LevelConstants.GetLevel(ImmortalTypes.God))
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

            comm.act(ATTypes.AT_TELL, "You tell $N '$t'", ch, argument, victim, ToTypes.Character);
            PositionTypes position = victim.CurrentPosition;
            victim.CurrentPosition = PositionTypes.Standing;

#if !SCRAMBLE
            /*if (speaking != -1 && (!ch.IsNpc() || ch.Speaking > 0))
            {
                int speakswell = victim.KnowsLanguage(ch.Speaking, ch).GetLowestOfTwoNumbers(ch.KnowsLanguage(ch.Speaking, victim));
                if (speakswell < 85)
                    comm.act(ATTypes.AT_TELL, "$n tells you '$t'",
                             ch, act_comm.TranslateLanguage(speakswell, argument,
                                           GameConstants.LanguageTable[speaking]), victim, ToTypes.Victim);
                else
                    comm.act(ATTypes.AT_TELL, "$n tells you '$t'",
                             ch, argument, victim, ToTypes.Victim);
            }
            else
                comm.act(ATTypes.AT_TELL, "$n tells you '$t'", ch, argument, victim, ToTypes.Victim);*/
#else
            if (act_comm.KnowsLanguage(victim, ch.Speaking, ch) == 0
                && (ch.IsNpc() || ch.Speaking != 0))
                comm.act(ATTypes.AT_TELL, "$n tells you '$t'", ch,
                         TranslateLanguage(speakswell, argument, GameConstants.LanguageTable[speaking]),
                         victim, ToTypes.Victim);
            else
                comm.act(ATTypes.AT_TELL, "$n tells you '$t'",
                         ch, scramble(argument, ch.Speaking), victim, ToTypes.Victim);
#endif

            victim.CurrentPosition = position;
            victim.ReplyTo = ch;
            ch.RetellTo = victim;

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.LogSpeech))
            {
                string buffer = string.Format("{0}: {1} (tell to) {2}",
                                              ch.IsNpc() ? ch.ShortDescription : ch.Name,
                                              argument,
                                              victim.IsNpc() ? victim.ShortDescription : victim.Name);
                db.append_to_file(SystemConstants.GetSystemFile(SystemFileTypes.Log), buffer);
            }

            mud_prog.mprog_tell_trigger(argument, ch);
        }
    }
}
