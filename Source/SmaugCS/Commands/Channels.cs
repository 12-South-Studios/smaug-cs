using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Communication;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using System;
using System.Collections.Generic;
using SmaugCS.Extensions.Character;
using EnumerationExtensions = Library.Common.Extensions.EnumerationExtensions;

namespace SmaugCS.Commands;

public static class Channels
{
  [Command(NoNpc = true)]
  public static void do_channels(CharacterInstance ch, string argument)
  {
    PlayerInstance pch = (PlayerInstance)ch;

    string firstWord = argument.FirstWord();
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
    if (string.IsNullOrEmpty(argument) && ch.Act.IsSet((int)PlayerFlags.Silence))
    {
      ch.SetColor(ATTypes.AT_GREEN);
      ch.SendTo("You are silenced.");
      return;
    }

    ch.SendTo(" %gChannels  %G:\r\n  ");

    foreach (ChannelTypes channelType in EnumerationExtensions.GetValues<ChannelTypes>())
    {
      string msg = GetChannelText(channelType, ch);
      if (!string.IsNullOrEmpty(msg))
        ch.PrintfColor(msg);
    }
  }

  private static string GetChannelText(ChannelTypes channelType, PlayerInstance ch)
  {
    ChannelAttribute attrib = channelType.GetAttribute<ChannelAttribute>();
    if (attrib == null)
      throw new InvalidOperationException();

    int minTrust = 0;
    RequireTrustChannelAttribute trustAttrib = channelType.GetAttribute<RequireTrustChannelAttribute>();
    if (trustAttrib != null)
      minTrust = GameConstants.GetConstant<int>(trustAttrib.TrustType);

    if (!attrib.Verify(channelType, ch, minTrust)) return string.Empty;
    ChannelPrintAttribute print = channelType.GetAttribute<ChannelPrintAttribute>();
    if (print == null)
      throw new InvalidOperationException("ChannelPrint attribute missing from ChannelType");

    return !ch.Deaf.IsSet(channelType) ? print.On : print.Off;
  }

  private static readonly List<ChannelTypes> ClearAllList = new()
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
      foreach (ChannelTypes chType in ClearAllList)
        clearAction.Invoke(ch, chType);
      return;
    }

    ChannelTypes channelType = EnumerationExtensions.GetEnumByName<ChannelTypes>(argument);

    ChannelAttribute attrib = channelType.GetAttribute<ChannelAttribute>();
    if (attrib == null)
      throw new InvalidOperationException();

    int minTrust = 0;
    RequireTrustChannelAttribute trustAttrib = channelType.GetAttribute<RequireTrustChannelAttribute>();
    if (trustAttrib != null)
      minTrust = GameConstants.GetConstant<int>(trustAttrib.TrustType);

    if (attrib.Verify(channelType, ch, minTrust))
      verifyAction.Invoke(ch, channelType);
  }
}