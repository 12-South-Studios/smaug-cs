using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Spells.Life;

class Weaken
{
  public static ReturnTypes spell_weaken(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    SkillData skill = Program.RepositoryManager.SKILLS.Get(sn);
    
    ch.SetColor(ATTypes.AT_MAGIC);
    if (CheckFunctions.CheckIfTrueCasting(victim.Immunity.IsSet(ResistanceTypes.Magic), 
          skill, ch, CastingFunctionType.Immune, victim, null))
      return ReturnTypes.SpellFailed;

    if (CheckFunctions.CheckIfTrue(ch, victim.IsAffected((AffectedByTypes)sn) || 
                                       victim.SavingThrows.CheckSaveVsWandRod(level, victim), 
          "Your magic fails to take hold.")) 
      return ReturnTypes.SpellFailed;

    AffectData af = new()
    {
      Type = (AffectedByTypes)sn,
      Duration = (int)(level / 2 * Program.DUR_CONV),
      Location = ApplyTypes.Strength,
      Modifier = -2
    };
    af.BitVector.ClearBits();
    victim.AddAffect(af);
    
    victim.SetColor(ATTypes.AT_MAGIC);
    victim.SendTo("Your muscles seem to atrophy!");

    if (ch != victim)
    {
      // TODO get prime attribute for class
      // magic.c lines 4063-4073
    }
    
    return ReturnTypes.None;
  }
}