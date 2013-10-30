using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Exceptions;
using SmaugCS.Objects;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class MobileRepository : Repository<int, MobTemplate>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="cvnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public MobTemplate Create(int vnum, int cvnum, string name)
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
                newMob.ShortDescription = cloneMob.ShortDescription;
                newMob.LongDescription = cloneMob.LongDescription;
                newMob.Description = cloneMob.Description;
                newMob.Act = new ExtendedBitvector(cloneMob.Act);
                newMob.AffectedBy = new ExtendedBitvector(cloneMob.AffectedBy);
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
                newMob.Attacks = new ExtendedBitvector(cloneMob.Attacks);
                newMob.Defenses = new ExtendedBitvector(cloneMob.Defenses);
            }

            return newMob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public MobTemplate Create(int vnum, string name)
        {
            Validation.Validate(vnum >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                });

            MobTemplate newMob = new MobTemplate()
                {
                    Name = string.Format("A newly created {0}", name),
                    Vnum = vnum,
                    LongDescription = string.Format("Somebody abandoned a newly created {0} here.", name),
                    Level = 1,
                    Position = PositionTypes.Standing,
                    DefPosition = PositionTypes.Standing,
                    Class = 3
                };
            newMob.Statistics[StatisticTypes.Strength] = 13;
            newMob.Statistics[StatisticTypes.Dexterity] = 13;
            newMob.Statistics[StatisticTypes.Intelligence] = 13;
            newMob.Statistics[StatisticTypes.Wisdom] = 13;
            newMob.Statistics[StatisticTypes.Charisma] = 13;
            newMob.Statistics[StatisticTypes.Constitution] = 13;
            newMob.Statistics[StatisticTypes.Luck] = 13;
            newMob.Act.SetBit((int)ActFlags.IsNpc);
            newMob.Act.SetBit((int)ActFlags.Prototype);

            Add(vnum, newMob);
            return newMob;
        }
    }
}
