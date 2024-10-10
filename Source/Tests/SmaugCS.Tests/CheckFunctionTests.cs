using FluentAssertions;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using System.Collections.Generic;
using SmaugCS.Helpers;
using Xunit;

namespace SmaugCS.Tests;

public class CheckFunctionTests
{
  [Fact]
  public void CheckIfNpc()
  {
    CharacterInstance actor = new(1, "TestNpc");
    actor.Act.SetBit((int)ActFlags.IsNpc);

    CheckFunctions.CheckIfNpc(actor, actor).Should().BeTrue();
  }

  [Fact]
  public void CheckIfEmptyString()
  {
    CharacterInstance actor = new(1, "TestNpc");

    CheckFunctions.CheckIfEmptyString(actor, string.Empty, string.Empty).Should().BeTrue();
  }

  [Fact]
  public void CheckIfNullObject()
  {
    CharacterInstance actor = new(1, "TestNpc");

    CheckFunctions.CheckIfNullObject(actor, null, string.Empty).Should().BeTrue();
  }

  [Fact]
  public void CheckIfNotNullObject()
  {
    CharacterInstance actor = new(1, "TestNpc");

    CheckFunctions.CheckIfNotNullObject(actor, new object(), string.Empty).Should().BeTrue();
  }

  [Fact]
  public void CheckIfEquivalent()
  {
    CharacterInstance actor = new(1, "TestNpc");

    CheckFunctions.CheckIfEquivalent(actor, actor, actor, string.Empty).Should().BeTrue();
  }

  [Fact]
  public void CheckIf_NoArgs()
  {
    CharacterInstance actor = new(1, "TestNpc") { PermanentStrength = 25 };

    CheckFunctions.CheckIf(actor, () => 5 * 10 == 50, string.Empty).Should().BeTrue();
  }

  [Fact]
  public void CheckIf_WithArgs()
  {
    CharacterInstance actor = new(1, "TestNpc") { PermanentStrength = 25 };


    CheckFunctions.CheckIf(actor, args => ((CharacterInstance)args[0]).PermanentStrength == 25, string.Empty,
      new List<object> { actor }).Should().BeTrue();
  }

  [Fact]
  public void CheckIfNotAuthorized_IsNpc()
  {
    CharacterInstance actor = new(1, "TestNpc");
    actor.Act.SetBit((int)ActFlags.IsNpc);

    CheckFunctions.CheckIfNotAuthorized(actor, actor).Should().BeFalse();
  }

  [Fact]
  public void CheckIfNotAuthorized_IsUnauthorized()
  {
    PlayerInstance actor = new(1, "TestNpc")
    {
      PlayerData = new PlayerData(1, 1)
      {
        AuthState = AuthorizationStates.Denied
      }
    };
    actor.PlayerData.Flags = actor.PlayerData.Flags.SetBit(PCFlags.Unauthorized);

    CheckFunctions.CheckIfNotAuthorized(actor, actor).Should().BeTrue();
  }

  [Fact]
  public void CheckIfSet()
  {
    CharacterInstance actor = new(1, "TestNpc");

    const int bitField = 2 | 4 | 8;

    CheckFunctions.CheckIfSet(actor, bitField, 4).Should().BeTrue();
  }

  [Fact]
  public void CheckIfNotSet()
  {
    CharacterInstance actor = new(1, "TestNpc");

    const int bitField = 2 | 4 | 8;

    CheckFunctions.CheckIfNotSet(actor, bitField, 16).Should().BeTrue();
  }

  [Fact]
  public void CheckIfTrue_ExpressionIsTrue()
  {
    CharacterInstance actor = new(1, "TestNpc");

    CheckFunctions.CheckIfTrue(actor, 5 > 1).Should().BeTrue();
  }

  [Fact]
  public void CheckIfTrue_ExpressionIsFalse()
  {
    CharacterInstance actor = new(1, "TestNpc");

    CheckFunctions.CheckIfTrue(actor, 5 < 1).Should().BeFalse();
  }
}