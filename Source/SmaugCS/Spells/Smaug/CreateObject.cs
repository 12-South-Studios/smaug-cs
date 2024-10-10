using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Objects;
using SmaugCS.Helpers;

namespace SmaugCS.Spells.Smaug;

public static class CreateObject
{
  public static ReturnTypes spell_create_obj(int sn, int level, CharacterInstance ch, object vo)
  {
    SkillData skill = Program.RepositoryManager.SKILLS.Get(sn);

    string targetName = Cast.TargetName;

    int lvl = GetObjectLevel(skill, level);
    int id = skill.value;

    if (id == 0)
    {
      if (!targetName.Equals("sword"))
        id = GameConstants.GetVnum("sword");
      else if (!targetName.Equals("shield"))
        id = GameConstants.GetVnum("shield");
    }

    ObjectTemplate oi = Program.RepositoryManager.OBJECTTEMPLATES.Get(id);
    if (CheckFunctions.CheckIfNullObjectCasting(oi, skill, ch)) return ReturnTypes.None;

    ObjectInstance obj = Program.RepositoryManager.OBJECTS.Create(oi);
    obj.Timer = !string.IsNullOrEmpty(skill.Dice) ? magic.ParseDiceExpression(ch, skill.Dice) : 0;
    obj.Level = lvl;

    ch.SuccessfulCast(skill, null, obj);

    if (obj.WearFlags.IsSet(ItemWearFlags.Take))
      obj.AddTo(ch);
    else
      ch.CurrentRoom.AddTo(obj);

    return ReturnTypes.None;
  }

  private static int GetObjectLevel(SkillData skill, int level)
  {
    return Macros.SPELL_POWER(skill) switch
    {
      (int)SpellPowerTypes.Major => level,
      (int)SpellPowerTypes.Greater => level / 2,
      (int)SpellPowerTypes.Minor => 0,
      _ => 10
    };
  }
}