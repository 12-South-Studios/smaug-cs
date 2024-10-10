using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;

namespace SmaugCS.Spells.Fire;

class FaerieFire
{
  public static ReturnTypes spell_faerie_fire(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;
    SkillData skill = Program.RepositoryManager.SKILLS.Get(sn);

    if (CheckFunctions.CheckIfTrueCasting(victim.Immunity.IsSet(ResistanceTypes.Magic), 
          skill, ch, CastingFunctionType.Immune, victim, null))
      return ReturnTypes.SpellFailed;
    if (CheckFunctions.CheckIfTrueCasting(victim.IsAffected(AffectedByTypes.FaerieFire),
          skill, ch, CastingFunctionType.Failed, victim, null))
      return ReturnTypes.SpellFailed;

    AffectData af = new()
    {
      Type = EnumerationExtensions.GetEnum<AffectedByTypes>(sn),
      Duration = (int)(level * Program.DUR_CONV),
      Location = ApplyTypes.ArmorClass,
      Modifier = 2 * level,
      BitVector = ExtendedBitvector.Meb((int)AffectedByTypes.FaerieFire)
    };
    victim.AddAffect(af);
    
    comm.act(ATTypes.AT_PINK, "You are surrounded by a pink outline.", victim, null, null, ToTypes.Character);
    comm.act(ATTypes.AT_PINK, "$n is surrounded by a pink outline.", victim, null, null, ToTypes.Room);

    return ReturnTypes.None;
  }
}