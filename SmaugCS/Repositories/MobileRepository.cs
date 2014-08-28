using System.IO;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;

namespace SmaugCS.Repositories
{
    public class MobileRepository : Repository<long, MobTemplate>, ITemplateRepository<MobTemplate>
    {
        private MobTemplate LastMob { get; set; }

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
                CloneMobTemplate(newMob, cloneMob);

            return newMob;
        }

        private static void CloneMobTemplate(MobTemplate newMob, MobTemplate cloneMob)
        {
            newMob.LongDescription = cloneMob.LongDescription;
            newMob.Description = cloneMob.Description;
            newMob.ActFlags = cloneMob.ActFlags;
            newMob.AffectedBy = cloneMob.AffectedBy;
            newMob.SpecialFunction = cloneMob.SpecialFunction;
            newMob.Statistics[StatisticTypes.Alignment] = cloneMob.GetStatistic(StatisticTypes.Alignment);
            newMob.Level = cloneMob.Level;
            newMob.Statistics[StatisticTypes.ToHitArmorClass0] = cloneMob.GetStatistic(StatisticTypes.ToHitArmorClass0);
            newMob.Statistics[StatisticTypes.ArmorClass] = cloneMob.GetStatistic(StatisticTypes.ArmorClass);
            newMob.HitDice = DiceData.Clone(cloneMob.HitDice);
            newMob.DamageDice = DiceData.Clone(cloneMob.DamageDice);
            newMob.Gold = cloneMob.Gold;
            newMob.Experience = cloneMob.Experience;
            newMob.Position = cloneMob.Position;
            newMob.DefensivePosition = cloneMob.DefensivePosition;
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

        public MobTemplate Create(long vnum, string name)
        {
            Validation.Validate(vnum >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                });

            MobTemplate newMob = MobTemplate.Create(vnum, name);
            newMob.Statistics[StatisticTypes.Strength] = 13;
            newMob.Statistics[StatisticTypes.Dexterity] = 13;
            newMob.Statistics[StatisticTypes.Intelligence] = 13;
            newMob.Statistics[StatisticTypes.Wisdom] = 13;
            newMob.Statistics[StatisticTypes.Charisma] = 13;
            newMob.Statistics[StatisticTypes.Constitution] = 13;
            newMob.Statistics[StatisticTypes.Luck] = 13;
            newMob.ActFlags = string.Format("{0} {1}", ActFlags.IsNpc, ActFlags.Prototype);

            Add(vnum, newMob);
            LastMob = newMob;
            return newMob;
        }
    }
}
