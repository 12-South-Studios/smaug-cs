using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Language;
using SmaugCS.Managers;

namespace SmaugCS.Extensions
{
    public static class DatabaseManagerExtensions
    {
        public static LiquidData GetLiquid(this DatabaseManager dbManager, string str)
        {
            return str.IsNumber()
                       ? dbManager.GetEntity<LiquidData>(str.ToInt32())
                       : dbManager.GetEntity<LiquidData>(str);
        }

        public static bool IsValidHerb(this DatabaseManager dbManager, int sn)
        {
            return GetSkills(dbManager, SkillTypes.Herb).FirstOrDefault(x => x.ID == sn) != null;
        }

        public static IEnumerable<SkillData> GetSkills(this DatabaseManager dbManager, SkillTypes type)
        {
            return dbManager.SKILLS.Values.Where(x => x.Type == type);
        }

        public static int LookupSkill(this DatabaseManager dbManager, string name)
        {
            // Try to find an exact match for this skill
            SkillData skill = dbManager.GetEntity<SkillData>(name);
            if (skill == null)
            {
                // Try to find a prefix match
                IEnumerable<SkillData> skills = dbManager.SKILLS.Values.Where(x => x.Name.StartsWithIgnoreCase(name));
                if (!skills.Any())
                {
                    dbManager.LogManager.Bug("Skill entry {0} not found", name);
                    return -1;
                }

                skill = skills.First();
            }

            return (int)skill.ID;
        }

        public static int AddSkill(this DatabaseManager dbManager, string name)
        {
            if (dbManager.LookupSkill(name) > -1)
                return -1;

            long newId = dbManager.GenerateNewId<SkillData>();
            dbManager.AddToRepository(new SkillData(newId, name));
            return (int)newId;
        }

        public static int GetLanguageCount(this DatabaseManager dbManager, int languages)
        {
            List<LanguageData> langList = dbManager.LANGUAGES.Values.ToList();

            return
                langList.Count(
                    lang =>
                    lang.Type != LanguageTypes.Clan && lang.Type != LanguageTypes.Unknown &&
                    ((int) lang.Type & languages) > 0);
        }

        public static RaceData GetRace(this DatabaseManager dbManager, RaceTypes type)
        {
            return dbManager.RACES.Values.FirstOrDefault(x => x.Type == type);
        }

        public static ClassData GetClass(this DatabaseManager dbManager, ClassTypes type)
        {
            return dbManager.CLASSES.Values.FirstOrDefault(x => x.Type == type);
        }

    }
}
