using SmaugCS.Common;
using SmaugCS.Communication;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.MudProgs;

namespace SmaugCS.Commands
{
    public static class Reply
    {
        public static void do_reply(CharacterInstance ch, string argument)
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

            ch.Deaf.RemoveBit(ChannelTypes.Tells);
            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Silence))
            {
                ch.SendTo("You can't do that here.\r\n");
                return;
            }

            if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.Silence))
            {
                ch.SendTo("Your message didn't get through.\r\n");
                return;
            }

            var victim = ((PlayerInstance)ch).ReplyTo;
            if (victim == null)
            {
                ch.SendTo("They aren't here.\r\n");
                return;
            }

            if (!victim.IsNpc()
                && victim.Switched != null
                && ch.CanSee(victim)
                && ch.Trust > LevelConstants.AvatarLevel)
            {
                ch.SendTo("That player is switched.\r\n");
                return;
            }

            if (!victim.IsNpc() && ((PlayerInstance)victim).Descriptor == null)
            {
                ch.SendTo("That player is link-dead.\r\n");
                return;
            }

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.AwayFromKeyboard))
            {
                ch.SendTo("That player is afk.\r\n");
                return;
            }

            if (victim.Deaf.IsSet(ChannelTypes.Tells)
                && (!ch.IsImmortal() || ch.Trust < victim.Trust))
            {
                comm.act(ATTypes.AT_PLAIN, "$E has $S tells turned off.", ch, null, victim, ToTypes.Character);
                return;
            }

            if ((!ch.IsImmortal() && !victim.IsAwake())
                || (!victim.IsNpc() && victim.CurrentRoom.Flags.IsSet(RoomFlags.Silence)))
            {
                comm.act(ATTypes.AT_PLAIN, "$E can't hear you.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (((PlayerInstance)victim).Descriptor != null
                && ((PlayerInstance)victim).Descriptor.ConnectionStatus == ConnectionTypes.Editing
                && ch.Trust < LevelConstants.GetLevel(ImmortalTypes.God))
            {
                comm.act(ATTypes.AT_PLAIN, "$E is currently in a writing buffer. Please try again later.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (victim.IsIgnoring(ch))
            {
                if (!ch.IsImmortal() || victim.Trust > ch.Trust)
                {
                    ch.SetColor(ATTypes.AT_IGNORE);
                    ch.Printf("%s is ignoring you.\r\n", victim.Name);
                    return;
                }

                victim.SetColor(ATTypes.AT_IGNORE);
                victim.Printf("You attempt to ignore %s, but are unable to do so.\r\n", ch.Name);
            }

            comm.act(ATTypes.AT_TELL, "You tell $N '$t'", ch, argument, victim, ToTypes.Character);
            var position = victim.CurrentPosition;
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
            ((PlayerInstance)victim).ReplyTo = ch;
            ((PlayerInstance)ch).RetellTo = victim;

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.LogSpeech))
            {
                var buffer =
                    $"{(ch.IsNpc() ? ch.ShortDescription : ch.Name)}: {argument} (tell to) {(victim.IsNpc() ? victim.ShortDescription : victim.Name)}";
                db.append_to_file(SystemConstants.GetSystemFile(SystemFileTypes.Log), buffer);
            }

            MudProgHandler.ExecuteMobileProg(MudProgTypes.Tell, argument, ch);
        }
    }
}
