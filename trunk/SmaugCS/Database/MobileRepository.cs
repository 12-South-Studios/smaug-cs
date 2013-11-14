using System;
using System.IO;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using SmaugCS.Exceptions;
using SmaugCS.Managers;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class MobileRepository : Repository<long, MobTemplate>
    {
        private MobTemplate LastMob { get; set; }

        [LuaFunction("LProcessMob", "Processes a mob script", "script text")]
        public static MobTemplate LuaProcessMob(string text)
        {
            LuaManager.Instance.Proxy.DoString(text);
            return DatabaseManager.Instance.MOBILE_INDEXES.LastMob;
        }

        [LuaFunction("LCreateMobile", "Creates a new mob", "Id of the Mobile", "Name of the Mobile")]
        public static MobTemplate LuaCreateMob(string id, string name)
        {
            long mobId = Convert.ToInt64(id);
            if (DatabaseManager.Instance.MOBILE_INDEXES.Contains(mobId))
                throw new DuplicateEntryException("Repository contains Mob with Id {0}", mobId);

            MobTemplate newMob = DatabaseManager.Instance.MOBILE_INDEXES.Create(mobId, name);
            LuaManager.Instance.Proxy.CreateTable("mobile");
            return newMob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="cvnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public MobTemplate Create(long vnum, long cvnum, string name)
        {
            Validation.Validate(cvnum >= 1 && cvnum != vnum && vnum >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                    if (!Contains(cvnum))
                        throw new InvalidDataException(string.Format("Clone vnum {0} is not present", cvnum));
                });

            MobTemplate newMob = Create(vnum, name);
            MobTemplate cloneMob = Get(cvnum);
            if (cloneMob != null)
            {
                newMob.LongDescription = cloneMob.LongDescription;
                newMob.Description = cloneMob.Description;
                newMob.Act = cloneMob.Act;
                newMob.AffectedBy = cloneMob.AffectedBy;
                newMob.SpecialFunction = cloneMob.SpecialFunction;
                newMob.Statistics[StatisticTypes.Alignment] = cloneMob.GetStatistic(StatisticTypes.Alignment);
                newMob.Level = cloneMob.Level;
                newMob.Statistics[StatisticTypes.ToHitArmorClass0] = cloneMob.GetStatistic(StatisticTypes.ToHitArmorClass0);
                newMob.Statistics[StatisticTypes.ArmorClass] = cloneMob.GetStatistic(StatisticTypes.ArmorClass);
                newMob.HitDice = new DiceData(cloneMob.HitDice);
                newMob.DamageDice = new DiceData(cloneMob.DamageDice);
                newMob.Gold = cloneMob.Gold;
                newMob.Experience = cloneMob.Experience;
                newMob.Position = cloneMob.Position;
                newMob.DefPosition = cloneMob.DefPosition;
                newMob.Gender = cloneMob.Gender;
                newMob.Statistics[StatisticTypes.Strength] = cloneMob.GetStatistic(StatisticTypes.Strength);
                newMob.Statistics[StatisticTypes.Dexterity] = cloneMob.GetStatistic(StatisticTypes.Dexterity);
                newMob.Statistics[StatisticTypes.Intelligence] = cloneMob.GetStatistic(StatisticTypes.Intelligence);
                newMob.Statistics[StatisticTypes.Wisdom] = cloneMob.GetStatistic(StatisticTypes.Wisdom);
                newMob.Statistics[StatisticTypes.Charisma] = cloneMob.GetStatistic(StatisticTypes.Charisma);
                newMob.Statistics[StatisticTypes.Constitution] = cloneMob.GetStatistic(StatisticTypes.Constitution);
                newMob.Statistics[StatisticTypes.Luck] = cloneMob.GetStatistic(StatisticTypes.Luck);
                newMob.Race = cloneMob.Race;
                newMob.Class = cloneMob.Class;
                newMob.ExtraFlags = cloneMob.ExtraFlags;
                newMob.Resistance = cloneMob.Resistance;
                newMob.Susceptibility = cloneMob.Susceptibility;
                newMob.Immunity = cloneMob.Immunity;
                newMob.NumberOfAttacks = cloneMob.NumberOfAttacks;
                newMob.Attacks = cloneMob.Attacks;
                newMob.Defenses = cloneMob.Defenses;
            }

            return newMob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public MobTemplate Create(long vnum, string name)
        {
            Validation.Validate(vnum >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                });

            MobTemplate newMob = new MobTemplate(vnum, name)
                                     {
                                         ShortDescription = string.Format("A newly created {0}", name),
                                         LongDescription =
                                             string.Format("Somebody abandoned a newly created {0} here.", name),
                                         Level = 1,
                                         Position = "standing",
                                         DefPosition = "standing",
                                         Class = "warrior",
                                         Race = "human",
                                         Gender = "male"
                                     };
            newMob.Statistics[StatisticTypes.Strength] = 13;
            newMob.Statistics[StatisticTypes.Dexterity] = 13;
            newMob.Statistics[StatisticTypes.Intelligence] = 13;
            newMob.Statistics[StatisticTypes.Wisdom] = 13;
            newMob.Statistics[StatisticTypes.Charisma] = 13;
            newMob.Statistics[StatisticTypes.Constitution] = 13;
            newMob.Statistics[StatisticTypes.Luck] = 13;
            newMob.Act = string.Format("{0} {1}", ActFlags.IsNpc, ActFlags.Prototype);

            Add(vnum, newMob);
            LastMob = newMob;
            return newMob;
        }
    }
}
