using System.IO;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Templates;

namespace SmaugCS.Repositories
{
    public class MobileRepository : Repository<long, MobTemplate>, ITemplateRepository<MobTemplate>
    {
        private MobTemplate LastMob { get; set; }

        public MobTemplate Create(long id, long cloneId, string name)
        {
            Validation.Validate(cloneId >= 1 && cloneId != id && id >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(id))
                        throw new DuplicateIndexException("Invalid ID {0}, Index already exists", id);
                    if (!Contains(cloneId))
                        throw new InvalidDataException(string.Format("Clone ID {0} is not present", cloneId));
                });

            var newMob = Create(id, name);

            var cloneMob = Get(cloneId);
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
            newMob.HitDice = new DiceData(cloneMob.HitDice);
            newMob.DamageDice = new DiceData(cloneMob.DamageDice);
            newMob.Gold = cloneMob.Gold;
            newMob.Experience = cloneMob.Experience;
            newMob.Position = cloneMob.Position;
            newMob.DefensivePosition = cloneMob.DefensivePosition;
            newMob.Gender = cloneMob.Gender;
            newMob.Statistics[StatisticTypes.PermanentStrength] = cloneMob.GetStatistic(StatisticTypes.PermanentStrength);
            newMob.Statistics[StatisticTypes.PermanentDexterity] = cloneMob.GetStatistic(StatisticTypes.PermanentDexterity);
            newMob.Statistics[StatisticTypes.PermanentIntelligence] = cloneMob.GetStatistic(StatisticTypes.PermanentIntelligence);
            newMob.Statistics[StatisticTypes.PermanentWisdom] = cloneMob.GetStatistic(StatisticTypes.PermanentWisdom);
            newMob.Statistics[StatisticTypes.PermanentCharisma] = cloneMob.GetStatistic(StatisticTypes.PermanentCharisma);
            newMob.Statistics[StatisticTypes.PermanentConstitution] = cloneMob.GetStatistic(StatisticTypes.PermanentConstitution);
            newMob.Statistics[StatisticTypes.PermanentLuck] = cloneMob.GetStatistic(StatisticTypes.PermanentLuck);
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

        public MobTemplate Create(long id, string name)
        {
            Validation.Validate(id >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(id))
                        throw new DuplicateIndexException("Invalid ID {0}, Index already exists", id);
                });

            var newMob = new MobTemplate(id, name);
            newMob.Statistics[StatisticTypes.PermanentStrength] = 13;
            newMob.Statistics[StatisticTypes.PermanentDexterity] = 13;
            newMob.Statistics[StatisticTypes.PermanentIntelligence] = 13;
            newMob.Statistics[StatisticTypes.PermanentWisdom] = 13;
            newMob.Statistics[StatisticTypes.PermanentCharisma] = 13;
            newMob.Statistics[StatisticTypes.PermanentConstitution] = 13;
            newMob.Statistics[StatisticTypes.PermanentLuck] = 13;
            newMob.ActFlags = string.Format("{0} {1}", ActFlags.IsNpc, ActFlags.Prototype);

            Add(id, newMob);
            LastMob = newMob;
            return newMob;
        }
    }
}
