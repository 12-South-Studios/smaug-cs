using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using EnumerationExtensions = Library.Common.Extensions.EnumerationExtensions;

namespace SmaugCS.Spells.Smaug;

public static class CreateMob
{
  public static ReturnTypes spell_create_mob(int sn, int level, CharacterInstance ch, object vo)
  {
    SkillData skill = Program.RepositoryManager.SKILLS.Get(sn);

    string targetName = Cast.TargetName;

    int lvl = GetMobLevel(skill, level);
    int id = skill.value;

    if (id == 0)
    {
      if (!targetName.Equals("cityguard"))
        id = GameConstants.GetVnum("cityguard");
      else if (!targetName.Equals("vampire"))
        id = GameConstants.GetVnum("vampire");
    }

    MobileTemplate mi = Program.RepositoryManager.MOBILETEMPLATES.Get(id);
    if (CheckFunctions.CheckIfNullObjectCasting(mi, skill, ch)) return ReturnTypes.None;

    CharacterInstance mob = Program.RepositoryManager.CHARACTERS.Create(mi);
    if (CheckFunctions.CheckIfNullObjectCasting(mob, skill, ch)) return ReturnTypes.None;

    mob.Level = lvl.GetLowestOfTwoNumbers(!string.IsNullOrEmpty(skill.Dice)
      ? magic.ParseDiceExpression(ch, skill.Dice)
      : mob.Level);
    mob.ArmorClass = mob.Level.Interpolate(100, -100);
    mob.MaximumHealth = mob.Level * 8 + SmaugRandom.Between(mob.Level * mob.Level / 4, mob.Level * mob.Level);
    mob.CurrentHealth = mob.MaximumHealth;
    mob.CurrentCoin = 0;

    ch.SuccessfulCast(skill, mob);
    ch.CurrentRoom.AddTo(mob);
    mob.AddFollower(ch);

    AffectData af = new()
    {
      Type = EnumerationExtensions.GetEnum<AffectedByTypes>((int)skill.Id),
      Duration = (SmaugRandom.Fuzzy((level + 1) / 3) + 1) *
                 GameConstants.GetConstant<int>("AffectDurationConversionValue")
    };
    mob.AddAffect(af);

    return ReturnTypes.None;
  }

  private static int GetMobLevel(SkillData skill, int level)
  {
    return Macros.SPELL_POWER(skill) switch
    {
      (int)SpellPowerTypes.Major => level,
      (int)SpellPowerTypes.Greater => level / 2,
      (int)SpellPowerTypes.Minor => 5,
      _ => 20
    };
  }
}