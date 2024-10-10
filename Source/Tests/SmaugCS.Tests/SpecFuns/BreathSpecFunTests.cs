using FakeItEasy;
using FluentAssertions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.SpecFuns;
using SmaugCS.SpecFuns.Breaths;
using Xunit;

namespace SmaugCS.Tests.SpecFuns;

public class BreathSpecFunTests
{
  private static MobileInstance _character;

  public BreathSpecFunTests()
  {
    _character = new MobileInstance(1, "Tester");
  }

  [Fact]
  public void DoBreathAny_NotFighting_Test()
  {
    _character.CurrentPosition = PositionTypes.Incapacitated;

    IManager mockDbManager = A.Fake<IManager>();

    BreathAny.Execute(_character, mockDbManager).Should().Be(false);
  }
}