using System.Linq;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Managers
{
    public sealed class ChatManager : GameSingleton
    {
        private static ChatManager _instance;
        private static readonly object Padlock = new object();

        private ChatManager()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static ChatManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new ChatManager());
                }
            }
        }

        public static void to_channel(string argument, ChannelTypes channel, string verb, int level)
        {
            if (db.DESCRIPTORS.Count == 0 || string.IsNullOrEmpty(argument))
                return;

            string buf = string.Format("{0}: {1}\r\n", verb, argument);

            foreach (DescriptorData d in db.DESCRIPTORS)
            {
                CharacterInstance och = d.Original ?? d.Character;
                CharacterInstance vch = d.Character;

                if (och == null || vch == null)
                    continue;
                if (!vch.IsImmortal()
                    || (vch.Trust < db.SystemData.GetMinimumLevel(PlayerPermissionTypes.BuildLevel)
                        && channel == ChannelTypes.Build)
                    || (vch.Trust < db.SystemData.GetMinimumLevel(PlayerPermissionTypes.LogLevel)
                        && (channel == ChannelTypes.Log || channel == ChannelTypes.High
                            || channel == ChannelTypes.Warn || channel == ChannelTypes.Comm)))
                    continue;

                if (d.ConnectionStatus == ConnectionTypes.Playing
                    && !och.Deaf.IsSet((int)channel)
                    && vch.Trust >= level)
                {
                    color.set_char_color(ATTypes.AT_LOG, vch);
                    color.send_to_char_color(buf, vch);
                }
            }
        }

        public static void SendToChat(CharacterInstance ch, string argument, ChannelTypes channel, string channelName)
        {
            if (Macros.NOT_AUTHORIZED(ch))
            {
                color.send_to_char("Huh?\r\n", ch);
                return;
            }

            talk_channel(ch, argument, channel, channelName);
        }

        public static void talk_channel(CharacterInstance ch, string argument, ChannelTypes channel, string verb)
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

            if (ch.IsNpc())
            {
                string message = string.Empty;
                switch (channel)
                {
                    case ChannelTypes.Clan:
                        message = "Mobs can't be in clans.\r\n";
                        break;
                    case ChannelTypes.Order:
                        message = "Mobs can't be in orders.\r\n";
                        break;
                    case ChannelTypes.Guild:
                        message = "Mobs can't be in guilds.\r\n";
                        break;
                    case ChannelTypes.Council:
                        message = "Mobs can't be in councils.\r\n";
                        break;
                    default:
                        if (ch.IsAffected(AffectedByTypes.Charm))
                        {
                            if (ch.Master != null)
                            {
                                color.send_to_char("I don't think so...\r\n", ch.Master);
                                return;
                            }
                        }
                        break;
                }

                if (!string.IsNullOrEmpty(message))
                {
                    color.send_to_char(message, ch);
                    return;
                }
            }

            if (!ch.IsPKill() && channel == ChannelTypes.WarTalk
                && !ch.IsImmortal())
            {
                color.send_to_char("Peacefuls have no need to use wartalk.\r\n", ch);
                return;
            }

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.Silence))
            {
                color.send_to_char("You can't do that here.\r\n", ch);
                return;
            }
            if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.Silence))
            {
                color.ch_printf(ch, "You can't %s.\r\n", verb);
                return;
            }

            if (string.IsNullOrEmpty(argument))
            {
                color.send_to_char(string.Format("{0} what?\r\n", verb).CapitalizeFirst(), ch);
                return;
            }

            ch.Deaf.RemoveBit((int)channel);

            color.set_char_color(GetColorForChannelTalk(channel), ch);

            string buffer = string.Empty;

            switch (channel)
            {
                case ChannelTypes.RaceTalk:
                    color.ch_printf(ch, "You %s '%s'\r\n", verb, argument);
                    buffer = string.Format("$n {0}s '$t'", verb);
                    break;
                case ChannelTypes.Traffic:
                    color.ch_printf(ch, "You %s:  %s\r\n", verb, argument);
                    buffer = string.Format("$n {0}s:  $t", verb);
                    break;
                case ChannelTypes.WarTalk:
                    color.ch_printf(ch, "You %s '%s'\r\n", verb, argument);
                    buffer = string.Format("$n {0}s '$t'", verb);
                    break;
                case ChannelTypes.AvTalk:
                case ChannelTypes.ImmTalk:
                    {
                        PositionTypes position = ch.Position;
                        comm.act(ATTypes.AT_IMMORT,
                                 string.Format("$n{0} $t", channel == ChannelTypes.ImmTalk ? '>' : ':'), ch, argument, null,
                                 ToTypes.Character);
                        ch.Position = position;
                    }
                    break;
                default:
                    color.ch_printf(ch, "You %s '%s'\r\n", verb, argument);
                    buffer = string.Format("$n {0}s '$t'", verb);
                    break;
            }

            if (ch.CurrentRoom.Flags.IsSet((int)RoomFlags.LogSpeech))
            {
                db.append_to_file(SystemConstants.GetSystemFile(SystemFileTypes.Log),
                                  string.Format("{0}: {1} ({2})", ch.IsNpc() ? ch.ShortDescription : ch.Name,
                                                argument,
                                                verb));
            }

            foreach (DescriptorData d in db.DESCRIPTORS)
            {
                CharacterInstance och = d.Original ?? d.Character;
                CharacterInstance vch = d.Character;

                if (d.ConnectionStatus == ConnectionTypes.Playing && vch != ch &&
                    !och.Deaf.IsSet((int)channel))
                {
                    string sbuf = argument;

                    if (och.IsIgnoring(ch) && (ch.Trust <= och.Trust))
                        continue;

                    if (channel != ChannelTypes.Newbie && Macros.NOT_AUTHORIZED(och))
                        continue;
                    if (channel == ChannelTypes.ImmTalk && !och.IsImmortal())
                        continue;
                    if (channel == ChannelTypes.WarTalk && Macros.NOT_AUTHORIZED(och))
                        continue;
                    if (channel == ChannelTypes.AvTalk && !och.IsHero())
                        continue;
                    if (channel == ChannelTypes.Highgod && och.Trust < db.SystemData.GetMinimumLevel(PlayerPermissionTypes.MuseLevel))
                        continue;
                    if (channel == ChannelTypes.High && och.Trust < db.SystemData.GetMinimumLevel(PlayerPermissionTypes.ThinkLevel))
                        continue;
                    if (channel == ChannelTypes.Traffic && !och.IsImmortal() && !ch.IsImmortal())
                    {
                        if ((ch.IsHero() && !och.IsHero()) ||
                            (!ch.IsHero() && och.IsHero()))
                            continue;
                    }

                    if (channel == ChannelTypes.Newbie &&
                        (!och.IsImmortal() && !Macros.NOT_AUTHORIZED(och)
                        && !(och.PlayerData.Council != null
                        && och.PlayerData.Council.Name.Equals("Newbie Council"))))
                        continue;
                    if (vch.CurrentRoom.Flags.IsSet((int)RoomFlags.Silence))
                        continue;
                    if (channel == ChannelTypes.Yell && vch.CurrentRoom.Area != ch.CurrentRoom.Area)
                        continue;
                    if ((channel == ChannelTypes.Clan || channel == ChannelTypes.Order || channel == ChannelTypes.Guild)
                        && (vch.IsNpc() || vch.PlayerData.Clan != ch.PlayerData.Clan))
                        continue;
                    if (channel == ChannelTypes.Council && (vch.IsNpc() || vch.PlayerData.Council != ch.PlayerData.Council))
                        continue;
                    if (channel == ChannelTypes.RaceTalk && vch.CurrentRace != ch.CurrentRace)
                        continue;

                    string lbuf = string.Empty;
                    if (ch.Act.IsSet((int)PlayerFlags.WizardInvisibility)
                        && handler.can_see(vch, ch) && vch.IsImmortal())
                    {
                        lbuf = string.Format("({0})", !ch.IsNpc()
                                                             ? ch.PlayerData.WizardInvisible
                                                             : ch.MobInvisible);
                    }

                    PositionTypes position = vch.Position;
                    if (channel != ChannelTypes.Shout && channel != ChannelTypes.Yell)
                        vch.Position = PositionTypes.Standing;

#if !SCRAMBLE
                    if (speaking != -1 && (!ch.IsNpc() || ch.Speaking > 0))
                    {
                        int speakswell = SmaugCS.Common.Check.Minimum(vch.KnowsLanguage(ch.Speaking, ch),
                                                     ch.KnowsLanguage(ch.Speaking, vch));
                        if (speakswell < 85)
                            sbuf = act_comm.TranslateLanguage(speakswell, argument, GameConstants.LanguageTable[speaking]);
                    }
#else
                    if (KnowsLanguage(vch, ch.Speaking, ch) == 0 &&
                        (!ch.IsNpc() || ch.Speaking != 0))
                        sbuf = ScrambleText(argument, ch.Speaking);
#endif
                    if (!ch.IsNpc()
                        && ch.PlayerData.Nuisance != null
                        && ch.PlayerData.Nuisance.Flags > 7
                        && (SmaugCS.Common.SmaugRandom.Percent() < ((ch.PlayerData.Nuisance.Flags - 7) * 10 * ch.PlayerData.Nuisance.Power)))
                        sbuf = argument.Scramble(SmaugCS.Common.SmaugRandom.Between(1, 10));

                    if (!vch.IsNpc() && vch.PlayerData.Nuisance != null
                        && vch.PlayerData.Nuisance.Flags > 7
                        && (SmaugCS.Common.SmaugRandom.Percent() < ((vch.PlayerData.Nuisance.Flags - 7) * 10 * vch.PlayerData.Nuisance.Power)))
                        sbuf = argument.Scramble(SmaugCS.Common.SmaugRandom.Between(1, 10));

                    // TODO Toggle global mobtrigger flag
                    lbuf = lbuf + buffer;
                    if (channel == ChannelTypes.ImmTalk || channel == ChannelTypes.AvTalk)
                        comm.act(ATTypes.AT_IMMORT, lbuf, ch, sbuf, vch, ToTypes.Victim);
                    else if (channel == ChannelTypes.WarTalk)
                        comm.act(ATTypes.AT_WARTALK, lbuf, ch, sbuf, vch, ToTypes.Victim);
                    else if (channel == ChannelTypes.RaceTalk)
                        comm.act(ATTypes.AT_RACETALK, lbuf, ch, sbuf, vch, ToTypes.Victim);
                    else
                        comm.act(ATTypes.AT_GOSSIP, lbuf, ch, sbuf, vch, ToTypes.Victim);
                    vch.Position = position;
                }
            }
        }

        private static ATTypes GetColorForChannelTalk(ChannelTypes channel)
        {
            if (channel == ChannelTypes.RaceTalk)
                return ATTypes.AT_RACETALK;
            if (channel == ChannelTypes.WarTalk)
                return ATTypes.AT_WARTALK;
            if (channel == ChannelTypes.ImmTalk || channel == ChannelTypes.AvTalk)
                return 0;
            return ATTypes.AT_GOSSIP;
        }

        public static void talk_auction(string argument)
        {
            string buffer = string.Format("Auction: {0}", argument);

            foreach (DescriptorData d in db.DESCRIPTORS)
            {
                CharacterInstance original = d.Original ?? d.Character;
                if (d.ConnectionStatus == ConnectionTypes.Playing
                    && !original.Deaf.IsSet((int)ChannelTypes.Auction)
                    && !original.CurrentRoom.Flags.IsSet((int)RoomFlags.Silence)
                    && !Macros.NOT_AUTHORIZED(original))
                    comm.act(ATTypes.AT_GOSSIP, buffer, original, null, null, ToTypes.Character);
            }
        }
    }
}
