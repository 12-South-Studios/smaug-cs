using Autofac;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Communication;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.MudProgs;

namespace SmaugCS.Commands
{
    public static class Tell
    {
        public static void do_tell(CharacterInstance ch, string argument)
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

            if (!ch.IsNpc() &&
                (ch.Act.IsSet((int)PlayerFlags.Silence) || ch.Act.IsSet((int)PlayerFlags.NoTell)))
            {
                ch.SendTo("You can't do that.\r\n");
                return;
            }

            var firstArgument = argument.FirstWord();
            var argumentString = argument.RemoveWord(1);
            if (string.IsNullOrWhiteSpace(argumentString))
            {
                ch.SendTo("Tell whom what?\r\n");
                return;
            }

            var victim = ch.GetCharacterInWorld(firstArgument);
            if (victim == null ||
                (victim.IsNpc() && victim.CurrentRoom != ch.CurrentRoom)
                || (!ch.IsNotAuthorized() && victim.IsNotAuthorized() && !ch.IsImmortal()))
            {
                ch.SendTo("They aren't here.\r\n");
                return;
            }

            if (ch == victim)
            {
                ch.SendTo("You have a nice little chat with yourself.\r\n");
                return;
            }

            if (ch.IsNotAuthorized() && !victim.IsNotAuthorized() && !victim.IsImmortal())
            {
                ch.SendTo("They can't hear you because you are not authorized.\r\n");
                return;
            }

            if (!victim.IsNpc() && victim.Switched != null
                && (ch.Trust > LevelConstants.AvatarLevel)
                && !victim.Switched.IsAffected(AffectedByTypes.Possess))
            {
                ch.SendTo("That player is switched.\r\n");
                return;
            }

            CharacterInstance switchedVictim = null;
            if (!victim.IsNpc() && victim.Switched != null
                && victim.Switched.IsAffected(AffectedByTypes.Possess))
                switchedVictim = victim.Switched;
            else if (!victim.IsNpc() && ((PlayerInstance)victim).Descriptor == null)
            {
                ch.SendTo("That player is link-dead.\r\n");
                return;
            }

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.AwayFromKeyboard))
            {
                ch.SendTo("That player is afk.");
                return;
            }

            if (victim.Deaf.IsSet((int)ChannelTypes.Tells)
                && (!ch.IsImmortal() || ch.Trust < victim.Trust))
            {
                comm.act(ATTypes.AT_PLAIN, "$E has $S tells turned off.", ch, null, victim, ToTypes.Character);
                return;
            }

            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Silence))
                ch.SendTo("That player is silenced. They will receive your message but cannot respond.");

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

            ((PlayerInstance)ch).RetellTo = victim;

            if (!victim.IsNpc() && victim.IsImmortal()
                && ((PlayerInstance)victim).PlayerData.TellHistory != null
                && char.IsLetter(ch.IsNpc() ? ch.ShortDescription.ToCharArray()[0] : ch.Name.ToCharArray()[0]))
            {
                var buffer =
                    $"{(ch.IsNpc() ? ch.ShortDescription.CapitalizeFirst() : ch.Name.CapitalizeFirst())} told you '{argumentString}'\r\n";
                ((PlayerInstance)victim).PlayerData.TellHistory.Add(buffer);
            }

            if (switchedVictim != null)
                victim = switchedVictim;

            //MOBTrigger = false;

            comm.act(ATTypes.AT_TELL, "You tell $N '$t'", ch, argumentString, victim, ToTypes.Character);
            var position = victim.CurrentPosition;
            victim.CurrentPosition = PositionTypes.Standing;

            /*if (speaking != -1 && (!ch.IsNpc() || ch.Speaking > 0))
            {
                int speakswell = victim.KnowsLanguage(ch.Speaking, ch).GetLowestOfTwoNumbers(ch.KnowsLanguage(ch.Speaking, victim));
                if (speakswell < 85)
                    comm.act(ATTypes.AT_TELL, "$n tells you '$t'", ch, act_comm.TranslateLanguage(speakswell, argumentString,
                        GameConstants.LanguageTable[speaking]), victim, ToTypes.Victim);
                else
                    comm.act(ATTypes.AT_TELL, "$n tells you '$t'", ch, argumentString, victim, ToTypes.Victim);
            }
            else
                comm.act(ATTypes.AT_TELL, "$n tells you '$t'", ch, argumentString, victim, ToTypes.Victim);*/

            //MOBtrigger = true;

            victim.CurrentPosition = position;
            ((PlayerInstance)victim).ReplyTo = ch;

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.LogSpeech))
            {
                var buffer =
                    $"{(ch.IsNpc() ? ch.ShortDescription : ch.Name)}: {argumentString} (tell to) {(victim.IsNpc() ? victim.ShortDescription : victim.Name)}";
                // TODO
                // db.append_to_file(SystemConstants.GetSystemFile(SystemFileTypes.Log), buffer);
            }

            MudProgHandler.ExecuteMobileProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Tell, argumentString, ch);
        }
    }
}
