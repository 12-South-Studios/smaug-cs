using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Skills;

namespace SmaugCS.Spells.Smaug;

public static class Smaug
{
  public static ReturnTypes spell_smaug(int sn, int level, CharacterInstance ch, object vo)
  {
    SkillData skill = Program.RepositoryManager.SKILLS.Get(sn);
    if (skill == null)
      return ReturnTypes.Error;

    return skill.Target switch
    {
      TargetTypes.Ignore => CastIgnoreTargetSpell(skill, level, ch, vo),
      TargetTypes.OffensiveCharacter => CastOffensiveSpell(skill, level, ch, vo),
      TargetTypes.DefensiveCharacter or TargetTypes.Self => CastDefensiveOrSelfSpell(skill, level, ch, vo),
      TargetTypes.InventoryObject => InventoryObject.spell_obj_inv(sn, level, ch, vo),
      _ => ReturnTypes.None
    };
  }

  private static ReturnTypes CastIgnoreTargetSpell(SkillData skill, int level, CharacterInstance ch, object vo)
  {
    if (skill.Flags.IsSet(SkillFlags.Area)
        && ((Macros.SPELL_ACTION(skill) == (int)SpellActTypes.Destroy
             && Macros.SPELL_CLASS(skill) == (int)SpellClassTypes.Life)
            || (Macros.SPELL_ACTION(skill) == (int)SpellActTypes.Create
                && Macros.SPELL_CLASS(skill) == (int)SpellClassTypes.Death)))
      return AreaAttack.spell_area_attack((int)skill.Id, level, ch, vo);

    if (Macros.SPELL_ACTION(skill) == (int)SpellActTypes.Create)
    {
      if (skill.Flags.IsSet(SkillFlags.Object))
        return CreateObject.spell_create_obj((int)skill.Id, level, ch, vo);
      if (Macros.SPELL_CLASS(skill) == (int)SpellClassTypes.Life)
        return CreateMob.spell_create_mob((int)skill.Id, level, ch, vo);
    }

    if (skill.Flags.IsSet(SkillFlags.Distant))
    {
      CharacterInstance victim = ch.GetCharacterInWorld(Cast.TargetName);
      if (victim != null && !victim.CurrentRoom.Flags.IsSet(RoomFlags.NoAstral)
                         && skill.Flags.IsSet(SkillFlags.Character))
        return Affect.spell_affect((int)skill.Id, level, ch, victim);
    }

    if (skill.Flags.IsSet(SkillFlags.Character))
    {
      CharacterInstance victim = ch.GetCharacterInWorld(Cast.TargetName);
      if (victim != null)
        return Affect.spell_affect((int)skill.Id, level, ch, victim);
    }

    if (skill.range > 0
        && ((Macros.SPELL_ACTION(skill) == (int)SpellActTypes.Destroy
             && Macros.SPELL_CLASS(skill) == (int)SpellClassTypes.Life)
            || (Macros.SPELL_ACTION(skill) == (int)SpellActTypes.Create
                && Macros.SPELL_CLASS(skill) == (int)SpellClassTypes.Death)))
      return skills.ranged_attack(ch, Cast.RangedTargetName, null, null, (int)skill.Id, skill.range);

    return Affect.spell_affect((int)skill.Id, level, ch, vo);
  }

  private static ReturnTypes CastOffensiveSpell(SkillData skill, int level, CharacterInstance ch, object vo)
  {
    if ((Macros.SPELL_ACTION(skill) == (int)SpellActTypes.Destroy
         && Macros.SPELL_CLASS(skill) == (int)SpellClassTypes.Life)
        || (Macros.SPELL_ACTION(skill) == (int)SpellActTypes.Create
            && Macros.SPELL_CLASS(skill) == (int)SpellClassTypes.Death))
      return Attack.spell_attack((int)skill.Id, level, ch, vo);
    return Affect.spell_affect((int)skill.Id, level, ch, vo);
  }

  private static ReturnTypes CastDefensiveOrSelfSpell(SkillData skill, int level, CharacterInstance ch, object vo)
  {
    if (CheckFunctions.CheckIfTrue(ch, skill.Flags.IsSet(SkillFlags.NoFight) && ch.IsInCombatPosition(),
          "You can't concentrate enough for that!")) return ReturnTypes.None;
    if (vo == null || Macros.SPELL_ACTION(skill) != (int)SpellActTypes.Destroy)
      return Affect.spell_affect((int)skill.Id, level, ch, vo);
    if (Macros.SPELL_DAMAGE(skill) == (int)SpellDamageTypes.Poison)
    {
      SkillData poisonSkill = Program.RepositoryManager.GetEntity<SkillData>("poison");
      if (poisonSkill == null)
        throw new ObjectNotFoundException("Skill 'poison' not found");

      // TODO Do something with poison skill
    }

    if (Macros.SPELL_CLASS(skill) == (int)SpellClassTypes.Illusion)
    {
      SkillData blindSkill = Program.RepositoryManager.GetEntity<SkillData>("blindness");
      if (blindSkill == null)
        throw new ObjectNotFoundException("Skill 'blindness' not found");

      // TODO Do something with blindness skill
    }

    return Affect.spell_affect((int)skill.Id, level, ch, vo);
  }
}