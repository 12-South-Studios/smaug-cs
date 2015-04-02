using System;
using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Communication;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using EnumerationExtensions = Realm.Library.Common.EnumerationExtensions;

namespace SmaugCS.Commands
{
    public static class Channels
    {
        [Command(NoNpc = true)]
        public static void do_channels(CharacterInstance ch, string argument)
        {
            var pch = (PlayerInstance) ch;

            var firstWord = argument.FirstWord();
            if (firstWord.IsNullOrEmpty())
                ListChannels(pch, argument);
            else if (firstWord.StartsWith("+"))
                ProcessChannelStatus(pch, firstWord.Remove(0, 1), SetChannel, SetChannel);
            else if (firstWord.StartsWith("-"))
                ProcessChannelStatus(pch, firstWord.Remove(0, 1), RemoveChannel, RemoveChannel);
            else
                ch.SendTo("Channels -channel or +channel?");
        }

        private static void ListChannels(PlayerInstance ch, string argument)
        {
            if (string.IsNullOrEmpty(argument) && ch.Act.IsSet(PlayerFlags.Silence))
            {
                ch.SetColor(ATTypes.AT_GREEN);
                ch.SendTo("You are silenced.");
                return;
            }

            ch.SendTo(" %gChannels  %G:\r\n  ");

            foreach(var channelType in EnumerationExtensions.GetValues<ChannelTypes>())
            {
                var msg = GetChannelText(channelType, ch);
                if (!string.IsNullOrEmpty(msg))
                    ch.PrintfColor(msg);
            }
        }

        private static string GetChannelText(ChannelTypes channelType, PlayerInstance ch)
        {
            var attrib = channelType.GetAttribute<ChannelAttribute>();
            if (attrib == null)
                throw new InvalidOperationException();

            var minTrust = 0;
            var trustAttrib = channelType.GetAttribute<RequireTrustChannelAttribute>();
            if (trustAttrib != null)
                minTrust = GameConstants.GetConstant<int>(trustAttrib.TrustType);

            if (attrib.Verify(channelType, ch, minTrust))
            {
                var print = channelType.GetAttribute<ChannelPrintAttribute>();
                if (print == null)
                    throw new InvalidOperationException("ChannelPrint attribute missing from ChannelType");

                return !ch.Deaf.IsSet(channelType) ? print.On : print.Off;
            }
            return string.Empty;
        }

        private static readonly List<ChannelTypes> ClearAllList = new List<ChannelTypes>
        {
            ChannelTypes.RaceTalk,
            ChannelTypes.Auction,
            ChannelTypes.Chat,
            ChannelTypes.Quest,
            ChannelTypes.WarTalk,
            ChannelTypes.Pray,
            ChannelTypes.Traffic,
            ChannelTypes.Music,
            ChannelTypes.Ask,
            ChannelTypes.Shout,
            ChannelTypes.Yell,
            ChannelTypes.AvTalk
        };

        private static void SetChannel(CharacterInstance ch, ChannelTypes channelType)
        {
            ch.Deaf = ch.Deaf.SetBit(channelType);
        }

        private static void RemoveChannel(CharacterInstance ch, ChannelTypes channelType)
        {
            ch.Deaf = ch.Deaf.RemoveBit(channelType);
        }

        private static void ProcessChannelStatus(PlayerInstance ch, string argument,
            Action<CharacterInstance, ChannelTypes> clearAction, Action<CharacterInstance, ChannelTypes> verifyAction)
        {
            if (argument.EqualsIgnoreCase("all"))
            {
                foreach (var chType in ClearAllList)
                    clearAction.Invoke(ch, chType);
                return;
            }

            var channelType = EnumerationExtensions.GetEnumByName<ChannelTypes>(argument);

            var attrib = channelType.GetAttribute<ChannelAttribute>();
            if (attrib == null)
                throw new InvalidOperationException();

            var minTrust = 0;
            var trustAttrib = channelType.GetAttribute<RequireTrustChannelAttribute>();
            if (trustAttrib != null)
                minTrust = GameConstants.GetConstant<int>(trustAttrib.TrustType);

            if (attrib.Verify(channelType, ch, minTrust))
                verifyAction.Invoke(ch, channelType);
        }
    }
}
