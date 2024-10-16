﻿using System.Linq;
using SmaugCS.Commands;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Repository;

namespace SmaugCS.SpecFuns.Professions;

public static class Executioner
{
  public static bool Execute(MobileInstance ch, IManager dbManager)
  {
    if (!ch.IsAwake() || ch.CurrentFighting != null) return false;

    string crime = string.Empty;
    CharacterInstance victim = null;

    foreach (CharacterInstance vch in ch.CurrentRoom.Persons.Where(vch => vch != ch))
    {
      victim = vch;
      crime = GetCrime(victim);
      if (!string.IsNullOrEmpty(crime))
        break;
    }

    if (victim == null) return false;

    if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
    {
      Yell.do_yell(ch, $"{victim.Name} is a {crime}! As well as a COWARD!");
      return true;
    }

    Shout.do_shout(ch, $"{victim.Name} is a {crime}! PROTECT THE INNOCENT! MORE BLOOOOD!!!");
    fight.multi_hit(ch, victim, Program.TYPE_UNDEFINED);

    if (ch.CharDied()) return true;

    int vnum = GameConstants.GetVnum("MobileCityGuard");

    IRepositoryManager databaseMgr = (IRepositoryManager)(dbManager ?? Program.RepositoryManager);
    MobileTemplate cityguard = databaseMgr.GetEntity<MobileTemplate>(vnum);

    CharacterInstance newGuard = databaseMgr.CHARACTERS.Create(cityguard, null);
    ch.CurrentRoom.AddTo(newGuard);

    newGuard = databaseMgr.CHARACTERS.Create(cityguard, null);
    ch.CurrentRoom.AddTo(newGuard);
    return true;
  }

  private static string GetCrime(CharacterInstance victim)
  {
    if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Killer)) return "KILLER";
    if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Thief)) return "THIEF";
    return string.Empty;
  }
}