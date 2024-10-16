﻿using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;

namespace SmaugCS.SpecFuns.Breaths;

public static class BreathAny
{
  private static readonly Dictionary<int, string> BreathTable = new()
  {
    { 0, "fire breath" },
    { 1, "lightning breath" },
    { 2, "lightning breath" },
    { 4, "acid breath" }
  };

  public static bool Execute(MobileInstance ch, IManager dbManager)
  {
    if (!ch.IsInCombatPosition())
      return false;

    int bits = SmaugRandom.Bits(3);
    return bits == 2
      ? Dragon.Execute(ch, "lightning breath", dbManager)
      : Dragon.Execute(ch, BreathTable.GetValueOrDefault(bits, "frost breath"), dbManager);
  }
}