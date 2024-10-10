using Library.Common.Extensions;
using Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Communication;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Managers;

public sealed class ChatManager : GameSingleton
{
  private static ChatManager _instance;
  private static readonly object Padlock = new();

  private ChatManager()
  {
  }

  public static ChatManager Instance
  {
    get
    {
      lock (Padlock)
      {
        return _instance ??= new ChatManager();
      }
    }
  }

  public static void to_channel(string argument, ChannelTypes channel, string verb, int level)
  {
    if (db.DESCRIPTORS.Count == 0 || string.IsNullOrEmpty(argument))
      return;

    string buf = $"{verb}: {argument}\r\n";

    foreach (DescriptorData d in db.DESCRIPTORS)
    {
      CharacterInstance och = d.Original ?? d.Character;
      CharacterInstance vch = d.Character;

      if (och == null || vch == null)
        continue;
      if (!vch.IsImmortal()
          || (vch.Trust < Program.GameManager.SystemData.GetMinimumLevel(PlayerPermissionTypes.BuildLevel)
              && channel == ChannelTypes.Build)
          || (vch.Trust < Program.GameManager.SystemData.GetMinimumLevel(PlayerPermissionTypes.LogLevel)
              && channel is ChannelTypes.Log or ChannelTypes.High or ChannelTypes.Warn or ChannelTypes.Comm))
        continue;

      if (d.ConnectionStatus != ConnectionTypes.Playing
          || och.Deaf.IsSet((int)channel)
          || vch.Trust < level) continue;
      vch.SetColor(ATTypes.AT_LOG);
      vch.SendTo(buf);
    }
  }

  public static void SendToChat(CharacterInstance ch, string argument, ChannelTypes channel, string channelName)
  {
    if (CheckFunctions.CheckIfTrue(ch, ch.IsNotAuthorized(), "Huh?")) return;

    talk_channel(ch, argument, channel, channelName);
  }

  public static void talk_channel(CharacterInstance ch, string argument, ChannelTypes channel, string verb)
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
              ch.Master.SendTo("I don't think so...");
              return;
            }
          }

          break;
      }

      if (!string.IsNullOrEmpty(message))
      {
        ch.SendTo(message);
        return;
      }
    }

    if (!ch.IsPKill() && channel == ChannelTypes.WarTalk
                      && !ch.IsImmortal())
    {
      ch.SendTo("Peacefuls have no need to use wartalk.");
      return;
    }

    if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Silence))
    {
      ch.SendTo("You can't do that here.");
      return;
    }

    if (!ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.Silence))
    {
      ch.Printf("You can't %s.\r\n", verb);
      return;
    }

    if (string.IsNullOrEmpty(argument))
    {
      ch.SendTo($"{verb} what?\r\n".CapitalizeFirst());
      return;
    }

    ch.Deaf.RemoveBit((int)channel);

    ch.SetColor(GetColorForChannelTalk(channel));

    string buffer = string.Empty;

    switch (channel)
    {
      case ChannelTypes.RaceTalk:
        ch.Printf("You %s '%s'\r\n", verb, argument);
        buffer = $"$n {verb}s '$t'";
        break;
      case ChannelTypes.Traffic:
        ch.Printf("You %s:  %s\r\n", verb, argument);
        buffer = $"$n {verb}s:  $t";
        break;
      case ChannelTypes.WarTalk:
        ch.Printf("You %s '%s'\r\n", verb, argument);
        buffer = $"$n {verb}s '$t'";
        break;
      case ChannelTypes.AvTalk:
      case ChannelTypes.ImmTalk:
      {
        PositionTypes position = ch.CurrentPosition;
        comm.act(ATTypes.AT_IMMORT,
          $"$n{(channel == ChannelTypes.ImmTalk ? '>' : ':')} $t", ch, argument, null,
          ToTypes.Character);
        ch.CurrentPosition = position;
      }
        break;
      default:
        ch.Printf("You %s '%s'\r\n", verb, argument);
        buffer = $"$n {verb}s '$t'";
        break;
    }

    if (ch.CurrentRoom.Flags.IsSet(RoomFlags.LogSpeech))
    {
      // TODO
      // db.append_to_file(SystemConstants.GetSystemFile(SystemFileTypes.Log),
      //    $"{(ch.IsNpc() ? ch.ShortDescription : ch.Name)}: {argument} ({verb})");
    }

    foreach (DescriptorData d in db.DESCRIPTORS)
    {
      PlayerInstance och = d.Original ?? d.Character;
      PlayerInstance vch = d.Character;

      if (d.ConnectionStatus == ConnectionTypes.Playing && vch != ch &&
          !och.Deaf.IsSet((int)channel))
      {
        string sbuf = argument;

        if (och.IsIgnoring(ch) && ch.Trust <= och.Trust)
          continue;

        if (channel != ChannelTypes.Newbie && och.IsNotAuthorized())
          continue;
        if (channel == ChannelTypes.ImmTalk && !och.IsImmortal())
          continue;
        if (channel == ChannelTypes.WarTalk && och.IsNotAuthorized())
          continue;
        if (channel == ChannelTypes.AvTalk && !och.IsHero())
          continue;
        if (channel == ChannelTypes.Highgod &&
            och.Trust < Program.GameManager.SystemData.GetMinimumLevel(PlayerPermissionTypes.MuseLevel))
          continue;
        if (channel == ChannelTypes.High &&
            och.Trust < Program.GameManager.SystemData.GetMinimumLevel(PlayerPermissionTypes.ThinkLevel))
          continue;
        if (channel == ChannelTypes.Traffic && !och.IsImmortal() && !ch.IsImmortal())
        {
          if ((ch.IsHero() && !och.IsHero()) ||
              (!ch.IsHero() && och.IsHero()))
            continue;
        }

        if (channel == ChannelTypes.Newbie && !och.IsImmortal() && !och.IsNotAuthorized() &&
            !(och.PlayerData.Council != null
              && och.PlayerData.Council.Name.Equals("Newbie Council")))
          continue;
        if (vch.CurrentRoom.Flags.IsSet((int)RoomFlags.Silence))
          continue;
        if (channel == ChannelTypes.Yell && vch.CurrentRoom.Area != ch.CurrentRoom.Area)
          continue;
        // TODO fix
        //if ((channel == ChannelTypes.Clan || channel == ChannelTypes.Order || channel == ChannelTypes.Guild)
        //    && (vch.IsNpc() || vch.PlayerData.Clan != ((PlayerInstance)ch).PlayerData.Clan))
        //    continue;
        if (channel == ChannelTypes.Council &&
            (vch.IsNpc() || vch.PlayerData.Council != ((PlayerInstance)ch).PlayerData.Council))
          continue;
        if (channel == ChannelTypes.RaceTalk && vch.CurrentRace != ch.CurrentRace)
          continue;

        string lbuf = string.Empty;
        if (ch.Act.IsSet((int)PlayerFlags.WizardInvisibility)
            && vch.CanSee(ch) && vch.IsImmortal())
        {
          lbuf = $"({(!ch.IsNpc() ? ((PlayerInstance)ch).PlayerData.WizardInvisible : ch.MobInvisible)})";
        }

        PositionTypes position = vch.CurrentPosition;
        if (channel != ChannelTypes.Shout && channel != ChannelTypes.Yell)
          vch.CurrentPosition = PositionTypes.Standing;

#if !SCRAMBLE
        /*if (speaking != -1 && (!ch.IsNpc() || ch.Speaking > 0))
        {
            int speakswell = vch.KnowsLanguage(ch.Speaking, ch).GetLowestOfTwoNumbers(ch.KnowsLanguage(ch.Speaking, vch));
            if (speakswell < 85)
                sbuf = act_comm.TranslateLanguage(speakswell, argument, GameConstants.LanguageTable[speaking]);
        }*/
#else
                    if (KnowsLanguage(vch, ch.Speaking, ch) == 0 &&
                        (!ch.IsNpc() || ch.Speaking != 0))
                        sbuf = ScrambleText(argument, ch.Speaking);
#endif
        if (!ch.IsNpc()
            && ((PlayerInstance)ch).PlayerData.Nuisance is { Flags: > 7 }
            && SmaugRandom.D100() < (((PlayerInstance)ch).PlayerData.Nuisance.Flags - 7) * 10 *
            ((PlayerInstance)ch).PlayerData.Nuisance.Power)
          sbuf = argument.Scramble(SmaugRandom.Between(1, 10));

        if (!vch.IsNpc() && vch.PlayerData.Nuisance is { Flags: > 7 }
                         && SmaugRandom.D100() <
                         (vch.PlayerData.Nuisance.Flags - 7) * 10 * vch.PlayerData.Nuisance.Power)
          sbuf = argument.Scramble(SmaugRandom.Between(1, 10));

        // TODO Toggle global mobtrigger flag
        lbuf = lbuf + buffer;
        switch (channel)
        {
          case ChannelTypes.ImmTalk:
          case ChannelTypes.AvTalk:
            comm.act(ATTypes.AT_IMMORT, lbuf, ch, sbuf, vch, ToTypes.Victim);
            break;
          case ChannelTypes.WarTalk:
            comm.act(ATTypes.AT_WARTALK, lbuf, ch, sbuf, vch, ToTypes.Victim);
            break;
          case ChannelTypes.RaceTalk:
            comm.act(ATTypes.AT_RACETALK, lbuf, ch, sbuf, vch, ToTypes.Victim);
            break;
          default:
            comm.act(ATTypes.AT_GOSSIP, lbuf, ch, sbuf, vch, ToTypes.Victim);
            break;
        }

        vch.CurrentPosition = position;
      }
    }
  }

  private static ATTypes GetColorForChannelTalk(ChannelTypes channel)
  {
    return channel switch
    {
      ChannelTypes.RaceTalk => ATTypes.AT_RACETALK,
      ChannelTypes.WarTalk => ATTypes.AT_WARTALK,
      ChannelTypes.ImmTalk or ChannelTypes.AvTalk => 0,
      _ => ATTypes.AT_GOSSIP
    };
  }

  public static void talk_auction(string argument)
  {
    string buffer = $"Auction: {argument}";

    foreach (DescriptorData d in db.DESCRIPTORS)
    {
      CharacterInstance original = d.Original ?? d.Character;
      if (d.ConnectionStatus == ConnectionTypes.Playing
          && !original.Deaf.IsSet((int)ChannelTypes.Auction)
          && !original.CurrentRoom.Flags.IsSet((int)RoomFlags.Silence)
          && !original.IsNotAuthorized())
        comm.act(ATTypes.AT_GOSSIP, buffer, original, null, null, ToTypes.Character);
    }
  }
}