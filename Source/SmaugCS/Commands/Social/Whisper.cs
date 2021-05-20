using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Communication;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.MudProgs;

namespace SmaugCS.Commands.Social
{
    public static class Whisper
    {
        public static void do_whisper(CharacterInstance ch, string argument)
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

            ch.Deaf.RemoveBit((int)ChannelTypes.Whisper);
            var firstArgument = argument.FirstWord();
            var argumentString = argument.RemoveWord(1);
            if (string.IsNullOrWhiteSpace(argumentString))
            {
                ch.SendTo("Whisper to whom what?");
                return;
            }

            var victim = ch.GetCharacterInRoom(firstArgument);
            if (victim == null)
            {
                ch.SendTo("They aren't here.");
                return;
            }

            if (ch == victim)
            {
                ch.SendTo("You have a nice little chat with yourself.");
                return;
            }

            if (!victim.IsNpc() && victim.Switched != null &&
                !victim.Switched.IsAffected(AffectedByTypes.Possess))
            {
                ch.SendTo("That player is switched.");
                return;
            }

            if (!victim.IsNpc() && ((PlayerInstance)victim).Descriptor == null)
            {
                ch.SendTo("That player is link-dead.");
                return;
            }

            if (!victim.IsNpc() && victim.Act.IsSet(PlayerFlags.AwayFromKeyboard))
            {
                ch.SendTo("That player is afk.");
                return;
            }

            if (victim.Deaf.IsSet(ChannelTypes.Whisper) &&
                (!ch.IsImmortal() || (ch.Trust < victim.Trust)))
            {
                comm.act(ATTypes.AT_PLAIN, "$E has $S whispers turned off.", ch, null, victim,
                         ToTypes.Character);
                return;
            }

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Silence))
                ch.SendTo("That player is silenced.  They will receive your message but cannot respond.");

            if (((PlayerInstance)victim).Descriptor != null
                && (((PlayerInstance)victim).Descriptor.ConnectionStatus == ConnectionTypes.Editing)
                && (ch.Trust < LevelConstants.GetLevel(ImmortalTypes.God)))
            {
                comm.act(ATTypes.AT_PLAIN, "$E is currently in a writing buffer.  Please try again in a few minutes.",
                    ch, 0, victim, ToTypes.Character);
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

            comm.act(ATTypes.AT_WHISPER, "You whisper to $N '$t'", ch, argumentString, victim, ToTypes.Character);
            var position = victim.CurrentPosition;
            victim.CurrentPosition = PositionTypes.Standing;

#if !SCRAMBLE
           /* if (speaking != -1 && (!ch.IsNpc() || ch.Speaking > 0))
            {
                int speakswell = victim.KnowsLanguage(ch.Speaking, ch).GetLowestOfTwoNumbers(ch.KnowsLanguage(ch.Speaking, victim));
                if (speakswell < 85)
                    comm.act(ATTypes.AT_WHISPER, "$n whispers to you '$t'",
                             ch, act_comm.TranslateLanguage(speakswell, argumentString,
                                           GameConstants.LanguageTable[speaking]), victim, ToTypes.Victim);
                else
                    comm.act(ATTypes.AT_WHISPER, "$n whispers to you '$t'",
                             ch, argumentString, victim, ToTypes.Victim);
            }
            else
                comm.act(ATTypes.AT_WHISPER, "$n whispers to you '$t'", ch, argument, victim, ToTypes.Victim);*/
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
                    $"{(ch.IsNpc() ? ch.ShortDescription : ch.Name)}: {argument} (whisper to) {(victim.IsNpc() ? victim.ShortDescription : victim.Name)}");

            MudProgHandler.ExecuteMobileProg(MudProgTypes.Tell, argument, ch);
        }
    }
}
