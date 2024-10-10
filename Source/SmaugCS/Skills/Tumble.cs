using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Player;
using SmaugCS.Repository;

namespace SmaugCS.Skills;

public static class Tumble
{
  public static bool CheckTumble(CharacterInstance ch, CharacterInstance victim, IRepositoryManager dbManager = null,
    IGameManager gameManager = null)
  {
    if (victim.CurrentClass != ClassTypes.Thief || !victim.IsAwake())
      return false;

    SkillData skill = (dbManager ?? Program.RepositoryManager).GetEntity<SkillData>("tumble");
    if (skill == null)
      throw new ObjectNotFoundException("Skill 'tumble' not found");

    if (!victim.IsNpc() && !(((PlayerInstance)victim).GetLearned((int)skill.Id) > 0))
      return false;

    int chances;
    if (victim.IsNpc())
      chances = 60.GetLowestOfTwoNumbers(2 * victim.Level);
    else
      chances = (int)Macros.LEARNED(victim, (int)skill.Id) /
                (gameManager ?? Program.GameManager).SystemData.TumbleMod +
                (victim.GetCurrentDexterity() - 13);

    if (chances != 0 && victim.CurrentMorph != null)
      chances += victim.CurrentMorph.Morph.TumbleChances;

    if (!victim.Chance(chances + victim.Level - ch.Level))
      return false;

    if (!victim.IsNpc() && !((PlayerInstance)victim).PlayerData.Flags.IsSet(PCFlags.Gag))
      comm.act(ATTypes.AT_SKILL, "You tumble away from $n's attack.", ch, null, victim, ToTypes.Victim);

    if (!ch.IsNpc() && !((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Gag))
      comm.act(ATTypes.AT_SKILL, "$N tumbles away from your attack.", ch, null, victim, ToTypes.Character);

    skill.LearnFromSuccess(victim);
    return true;
  }
}