﻿using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Spells.Fire;

class ScorchingSurge
{
  private static readonly List<int> DamageValues =
  [
    0,
    0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    0, 0, 0, 0, 30, 35, 40, 45, 50, 55,
    60, 65, 70, 75, 80, 82, 84, 86, 88, 90,
    92, 94, 96, 98, 100, 102, 104, 106, 108, 110,
    112, 114, 116, 118, 120, 122, 124, 126, 128, 130,
    132, 134, 136, 138, 140, 142, 144, 146, 148, 150,
    152, 154, 156, 158, 160, 162, 164, 166, 168, 170
  ];

  public static ReturnTypes spell_scorching_surge(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    level = level.GetHighestOfTwoNumbers(DamageValues.Count - 1);
    level = 0.GetLowestOfTwoNumbers(level);

    int dam = SmaugRandom.Between(DamageValues[level] / 2, DamageValues[level] * 2);

    if (victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
      dam /= 2;
    
    comm.act(ATTypes.AT_MAGIC, "A fiery current lashes through $n's body!", ch, null, null, ToTypes.Room);
    comm.act(ATTypes.AT_MAGIC, "A fiery current lashes through your body!", ch, null, null, ToTypes.Character);

    return ch.CauseDamageTo(victim, (int)(dam * 1.4), sn);
  }
}