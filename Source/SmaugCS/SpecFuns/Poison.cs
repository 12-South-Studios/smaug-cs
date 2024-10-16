﻿using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Repository;

namespace SmaugCS.SpecFuns;

public static class Poison
{
  public static bool Execute(MobileInstance ch, IManager dbManager)
  {
    if (!ch.IsInCombatPosition()) return false;

    CharacterInstance victim = ch.GetMyTarget();
    if (victim == null || SmaugRandom.D100() > 2 * ch.Level) return false;

    comm.act(ATTypes.AT_HIT, "You bite $N!", ch, null, victim, ToTypes.Character);
    comm.act(ATTypes.AT_ACTION, "$n bites $N!", ch, null, victim, ToTypes.NotVictim);
    comm.act(ATTypes.AT_POISON, "$n bites you!", ch, null, victim, ToTypes.Victim);

    IRepositoryManager databaseMgr = (IRepositoryManager)(dbManager ?? Program.RepositoryManager);
    Spells.Poison.Poison.spell_poison((int)databaseMgr.GetEntity<SkillData>("poison").Id, ch.Level, ch, victim);

    return true;
  }
}