using FluentAssertions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using System.Collections.Generic;
using SmaugCS.Helpers;
using Xunit;

namespace SmaugCS.Tests;

public class HelperFunctionTests
{
  [Theory]
  [InlineData(50, 5, true)]
  [InlineData(3, 5, false)]
  public void HasSufficientBloodPower(int currentBlood, int useBlood, bool expectedValue)
  {
    PlayerInstance actor = new(1, "TestNpc")
    {
      CurrentRace = RaceTypes.Vampire,
      PlayerData = new PlayerData(1, 1)
    };
    actor.PlayerData.ConditionTable.Clear();
    actor.PlayerData.ConditionTable[ConditionTypes.Bloodthirsty] = currentBlood;

    List<object> list = [actor, useBlood];

    HelperFunctions.HasSufficientBloodPower(list).Should().Be(expectedValue);
  }

  [Theory]
  [InlineData(50, 5, true)]
  [InlineData(3, 5, false)]
  public void HasSufficientMana(int currentMana, int useMana, bool expectedValue)
  {
    CharacterInstance actor = new(1, "TestNpc")
    {
      CurrentMana = currentMana
    };

    List<object> list = [actor, useMana];

    HelperFunctions.HasSufficientMana(list).Should().Be(expectedValue);
  }

  [Theory]
  [InlineData(AffectedByTypes.Charm, true)]
  [InlineData(AffectedByTypes.Possess, true)]
  [InlineData(AffectedByTypes.Curse, false)]
  public void IsCharmedOrPossessed(AffectedByTypes affectedBy, bool expectedValue)
  {
    CharacterInstance actor = new(1, "TestNpc");
    actor.Act.SetBit((int)ActFlags.IsNpc);
    actor.AffectedBy.SetBit((int)affectedBy);

    List<object> list = [actor];

    HelperFunctions.IsCharmedOrPossessed(list).Should().Be(expectedValue);
  }

  [Fact]
  public void IsFighting()
  {
    CharacterInstance defender = new(2, "TestNpc");

    CharacterInstance actor = new(1, "TestNpc")
    {
      CurrentFighting = new FightingData
      {
        Who = defender
      }
    };

    List<object> list = [actor];

    HelperFunctions.IsFighting(list).Should().BeTrue();
  }

  [Theory]
  [InlineData(PositionTypes.Aggressive, true)]
  [InlineData(PositionTypes.Berserk, true)]
  [InlineData(PositionTypes.Defensive, true)]
  [InlineData(PositionTypes.Evasive, true)]
  [InlineData(PositionTypes.Fighting, true)]
  [InlineData(PositionTypes.Dead, false)]
  public void IsInFightingPosition(PositionTypes position, bool expectedValue)
  {
    CharacterInstance actor = new(1, "TestNpc") { CurrentPosition = position };

    List<object> list = [actor];

    HelperFunctions.IsInFightingPosition(list).Should().Be(expectedValue);
  }
}