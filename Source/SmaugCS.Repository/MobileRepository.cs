using System.IO;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;

namespace SmaugCS.Repository
{
    public class MobileRepository : Repository<long, MobileTemplate>, ITemplateRepository<MobileTemplate>
    {
        private MobileTemplate LastMob { get; set; }

        public MobileTemplate Create(long id, long cloneId, string name)
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

        private static void CloneMobTemplate(MobileTemplate newMob, MobileTemplate cloneMob)
        {
            newMob.LongDescription = cloneMob.LongDescription;
            newMob.Description = cloneMob.Description;
            newMob.Statistics[StatisticTypes.ActFlags] = cloneMob.GetStatistic<string>(StatisticTypes.ActFlags);
            newMob.Statistics[StatisticTypes.AffectedByFlags] = cloneMob.GetStatistic<string>(StatisticTypes.AffectedByFlags);
            newMob.SpecialFunction = cloneMob.SpecialFunction;
            newMob.Statistics[StatisticTypes.Alignment] = cloneMob.GetStatistic<int>(StatisticTypes.Alignment);
            newMob.Level = cloneMob.Level;
            newMob.Statistics[StatisticTypes.ToHitArmorClass0] = cloneMob.GetStatistic<int>(StatisticTypes.ToHitArmorClass0);
            newMob.Statistics[StatisticTypes.ArmorClass] = cloneMob.GetStatistic<int>(StatisticTypes.ArmorClass);
            newMob.HitDice = new DiceData(cloneMob.HitDice);
            newMob.DamageDice = new DiceData(cloneMob.DamageDice);
            newMob.Statistics[StatisticTypes.Coin] = cloneMob.GetStatistic<int>(StatisticTypes.Coin);
            newMob.Statistics[StatisticTypes.Experience] = cloneMob.GetStatistic<int>(StatisticTypes.Experience);
            newMob.Position = cloneMob.Position;
            newMob.DefensivePosition = cloneMob.DefensivePosition;
            newMob.Statistics[StatisticTypes.Gender] = cloneMob.GetStatistic<string>(StatisticTypes.Gender);
            newMob.Statistics[StatisticTypes.PermanentStrength] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentStrength);
            newMob.Statistics[StatisticTypes.PermanentDexterity] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentDexterity);
            newMob.Statistics[StatisticTypes.PermanentIntelligence] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentIntelligence);
            newMob.Statistics[StatisticTypes.PermanentWisdom] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentWisdom);
            newMob.Statistics[StatisticTypes.PermanentCharisma] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentCharisma);
            newMob.Statistics[StatisticTypes.PermanentConstitution] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentConstitution);
            newMob.Statistics[StatisticTypes.PermanentLuck] = cloneMob.GetStatistic<int>(StatisticTypes.PermanentLuck);
            newMob.Race = cloneMob.Race;
            newMob.Class = cloneMob.Class;
            newMob.ExtraFlags = cloneMob.ExtraFlags;
            newMob.Resistance = cloneMob.Resistance;
            newMob.Susceptibility = cloneMob.Susceptibility;
            newMob.Immunity = cloneMob.Immunity;
            newMob.Statistics[StatisticTypes.NumberOfAttacks] = cloneMob.GetStatistic<int>(StatisticTypes.NumberOfAttacks);
            newMob.Attacks = cloneMob.Attacks;
            newMob.Defenses = cloneMob.Defenses;
        }

        public MobileTemplate Create(long id, string name)
        {
            Validation.Validate(id >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(id))
                        throw new DuplicateIndexException("Invalid ID {0}, Index already exists", id);
                });

            var newMob = new MobileTemplate(id, name);
            newMob.Statistics[StatisticTypes.PermanentStrength] = 13;
            newMob.Statistics[StatisticTypes.PermanentDexterity] = 13;
            newMob.Statistics[StatisticTypes.PermanentIntelligence] = 13;
            newMob.Statistics[StatisticTypes.PermanentWisdom] = 13;
            newMob.Statistics[StatisticTypes.PermanentCharisma] = 13;
            newMob.Statistics[StatisticTypes.PermanentConstitution] = 13;
            newMob.Statistics[StatisticTypes.PermanentLuck] = 13;
            newMob.Statistics[StatisticTypes.ActFlags] = string.Format("{0} {1}", ActFlags.IsNpc, ActFlags.Prototype);

            Add(id, newMob);
            LastMob = newMob;
            return newMob;
        }
    }
}
