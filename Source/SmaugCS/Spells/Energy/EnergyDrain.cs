using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.MudProgs;

namespace SmaugCS.Spells.Energy;

class EnergyDrain
{
  public static ReturnTypes spell_energy_drain(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    SkillData skill = Program.RepositoryManager.SKILLS.Get(sn);

    if (Helpers.CheckFunctions.CheckIfTrueCasting(victim.Immunity.IsSet(ResistanceTypes.Magic), skill, 
          ch, CastingFunctionType.Immune, victim, null)) 
      return ReturnTypes.SpellFailed;
    
    int saveChance = victim.ModifySavingThrowWithResistance(victim.Level, ResistanceTypes.Drain);
    if (Helpers.CheckFunctions.CheckIfTrueCasting(saveChance == 1000 || victim.SavingThrows.CheckSaveVsSpellStaff(saveChance, victim), 
          skill, ch, CastingFunctionType.Failed, victim, null))
      return ReturnTypes.SpellFailed;

    ch.CurrentAlignment = -1000.GetHighestOfTwoNumbers(ch.CurrentAlignment - 200);

    int dam;
    if (victim.Level <= 2)
    {
      dam = ch.CurrentHealth + 1;
    }
    else
    {
      if (victim is PlayerInstance instance)
      {
        instance.GainXP(0 - SmaugRandom.Between(3 * level / 2, level / 2));
      }

      victim.MaximumMana /= 2;
      victim.MaximumMovement /= 2;
      dam = SmaugRandom.Roll(level, 1);
      ch.CurrentHealth += dam;
    }

    if (ch.CurrentHealth > ch.MaximumHealth)
      ch.CurrentHealth = ch.MaximumHealth;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}