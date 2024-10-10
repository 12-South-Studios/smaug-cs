using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Repository;

namespace SmaugCS.Spells;

public static class Blindness
{
  public static ReturnTypes spell_blindness(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    SkillData skill = Program.RepositoryManager.GetEntity<SkillData>(sn);

    int tmp = skill.Flags.IsSet(SkillFlags.PKSensitive) ? level / 2 : level;

    if (victim.IsImmune(ResistanceTypes.Magic))
    {
      ch.ImmuneCast(skill, victim);
      return ReturnTypes.SpellFailed;
    }

    if (ch.IsAffected(AffectedByTypes.Blind) || ch.SavingThrows.CheckSaveVsSpellStaff(tmp, victim))
    {
      ch.FailedCast(skill, victim);
      return ReturnTypes.SpellFailed;
    }

    AffectData af = new()
    {
      SkillNumber = sn,
      Location = ApplyTypes.HitRoll,
      Modifier = -4,
      Duration = GetDuration(level)
    };

    victim.AddAffect(af);
    victim.SetColor(ATTypes.AT_MAGIC);
    victim.SendTo("You are blinded!");

    if (ch == victim) return ReturnTypes.None;
    comm.act(ATTypes.AT_MAGIC, "You weave a spell of blindness around $N.", ch, null, victim, ToTypes.Character);
    comm.act(ATTypes.AT_MAGIC, "$n weaves a spell of blindness about $N.", ch, null, victim, ToTypes.NotVictim);

    return ReturnTypes.None;
  }

  private static int GetDuration(int level)
  {
    return (1 + level / 3) * GameConstants.GetConstant<int>("AffectDurationConversionValue");
  }
}