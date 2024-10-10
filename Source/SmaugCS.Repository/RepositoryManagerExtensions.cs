using Library.Common.Extensions;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Language;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Repository;

public static class RepositoryManagerExtensions
{
  public static LiquidData GetLiquid(this RepositoryManager repoManager, string str)
  {
    return str.IsNumber()
      ? repoManager.GetEntity<LiquidData>(str.ToInt32())
      : repoManager.GetEntity<LiquidData>(str);
  }

  public static bool IsValidHerb(this RepositoryManager repoManager, int sn)
  {
    return GetSkills(repoManager, SkillTypes.Herb).FirstOrDefault(x => x.Id == sn) != null;
  }

  public static IEnumerable<SkillData> GetSkills(this RepositoryManager repoManager, SkillTypes type)
  {
    return repoManager.SKILLS.Values.Where(x => x.Type == type);
  }

  public static int LookupSkill(this IRepositoryManager repoManager, string name)
  {
    SkillData skill = repoManager.GetEntity<SkillData>(name);
    if (skill != null) return (int)skill.Id;
    IEnumerable<SkillData> skills = repoManager.SKILLS.Values.Where(x => x.Name.StartsWithIgnoreCase(name));
    if (!skills.Any())
    {
      repoManager.LogManager.Bug("Skill entry {0} not found", name);
      return -1;
    }

    skill = skills.First();

    return (int)skill.Id;
  }

  public static int AddSkill(this RepositoryManager repoManager, string name)
  {
    if (repoManager.LookupSkill(name) > -1)
      return -1;

    long newId = repoManager.GenerateNewId<SkillData>();
    repoManager.AddToRepository(new SkillData(newId, name));
    return (int)newId;
  }

  public static int GetLanguageCount(this RepositoryManager repoManager, int languages)
  {
    List<LanguageData> langList = repoManager.LANGUAGES.Values.ToList();

    return
      langList.Count(
        lang =>
          lang.Type != LanguageTypes.Clan && lang.Type != LanguageTypes.None &&
          ((int)lang.Type & languages) > 0);
  }

  public static RaceData GetRace(this IRepositoryManager repoManager, RaceTypes type)
  {
    return repoManager.RACES.Values.FirstOrDefault(x => x.Type == type);
  }

  public static ClassData GetClass(this IRepositoryManager repoManager, ClassTypes type)
  {
    return repoManager.CLASSES.Values.FirstOrDefault(x => x.Type == type);
  }
}