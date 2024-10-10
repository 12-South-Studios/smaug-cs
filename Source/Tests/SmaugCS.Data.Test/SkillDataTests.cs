using FluentAssertions;
using SmaugCS.Common.Enumerations;
using Xunit;

namespace SmaugCS.Data.Test;

public class SkillDataTests
{
  [Fact]
  public void Compare_NullSkill1_Test()
  {
    SkillData sk = new(1, "Test");

    SkillData.Compare(null, sk).Should().Be(1);
  }

  [Fact]
  public void Compare_NullSkill2_Test()
  {
    SkillData sk = new(1, "Test");

    SkillData.Compare(sk, null).Should().Be(-1);
  }

  [Fact]
  public void Compare_BothNull_Test()
  {
    SkillData.Compare(null, null).Should().Be(0);
  }

  [InlineData("Test", "Tester", -1)]
  [InlineData("Tester", "Test", 1)]
  [InlineData("Test", "Test", 0)]
  public void Compare_Test(string name1, string name2, int expectedValue)
  {
    SkillData sk1 = new(1, name1);
    SkillData sk2 = new(2, name2);

    SkillData.Compare(sk1, sk2).Should().Be(expectedValue);
  }

  [Fact]
  public void CompareByType_NullSkill1_Test()
  {
    SkillData sk = new(1, "Test");

    SkillData.CompareByType(null, sk).Should().Be(1);
  }

  [Fact]
  public void CompareByType_NullSkill2_Test()
  {
    SkillData sk = new(1, "Test");

    SkillData.CompareByType(sk, null).Should().Be(-1);
  }

  [Fact]
  public void CompareByType_BothNull_Test()
  {
    SkillData.CompareByType(null, null).Should().Be(0);
  }

  [InlineData(SkillTypes.Spell, SkillTypes.Disease, -1)]
  [InlineData(SkillTypes.Tongue, SkillTypes.Skill, 1)]
  public void CompareByType_Test(SkillTypes type1, SkillTypes type2, int expectedValue)
  {
    SkillData sk1 = new(1, "Test1") { Type = type1 };
    SkillData sk2 = new(2, "Test2") { Type = type2 };

    SkillData.CompareByType(sk1, sk2).Should().Be(expectedValue);
  }

  [InlineData("Test", "Tester", -1)]
  [InlineData("Tester", "Test", 1)]
  [InlineData("Test", "Test", 0)]
  public void CompareByType_CheckName_Test(string name1, string name2, int expectedValue)
  {
    SkillData sk1 = new(1, name1) { Type = SkillTypes.Herb };
    SkillData sk2 = new(2, name2) { Type = SkillTypes.Herb };

    SkillData.CompareByType(sk1, sk2).Should().Be(expectedValue);
  }
}