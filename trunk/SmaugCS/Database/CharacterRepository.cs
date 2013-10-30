using System.IO;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class CharacterRepository : Repository<int, CharacterInstance>
    {
        private static int _idSpace = 1;
        private static int GetNextId { get { return _idSpace++; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public CharacterInstance Create(MobTemplate parent)
        {
            Validation.IsNotNull(parent, "parent");

            CharacterInstance mob = new CharacterInstance(GetNextId)
                {
                    Parent = parent,
                    Name = parent.Name,
                    ShortDescription = parent.ShortDescription,
                    LongDescription = parent.LongDescription,
                    Description = parent.Description,
                    SpecialFunction = parent.SpecialFunction,
                    Level = SmaugRandom.Fuzzy(parent.Level),
                    Act = parent.Act,
                    HomeVNum = -1,
                    ResetVnum = -1,
                    ResetNum = -1,
                    AffectedBy = parent.AffectedBy,
                    CurrentAlignment = parent.GetStatistic(StatisticTypes.Alignment),
                    Gender = EnumerationExtensions.GetEnum<GenderTypes>(parent.Gender)
                };

            if (!string.IsNullOrEmpty(parent.SpecialFunctionName))
                mob.SpecialFunctionName = parent.SpecialFunctionName;

            if (mob.Act.IsSet((int)ActFlags.MobInvisibility))
                mob.MobInvisible = mob.Level;

            mob.ArmorClass = parent.GetStatistic(StatisticTypes.ArmorClass) > 0
                                 ? parent.GetStatistic(StatisticTypes.ArmorClass)
                                 : mob.Level.Interpolate(100, -100);

            if (parent.HitDice == null || parent.HitDice.NumberOf == 0)
                mob.MaximumHealth = mob.Level * 8 + SmaugRandom.Between(mob.Level * mob.Level / 4, mob.Level * mob.Level);
            else
                mob.MaximumHealth = parent.HitDice.NumberOf * SmaugRandom.Between(1, parent.HitDice.SizeOf) + parent.HitDice.Bonus;

            mob.CurrentCoin = parent.Gold;
            mob.Experience = parent.Experience;
            mob.CurrentPosition = parent.Position;
            mob.CurrentDefensivePosition = parent.DefPosition;
            mob.BareDice = new DiceData { NumberOf = parent.DamageDice.NumberOf, SizeOf = parent.DamageDice.SizeOf };
            mob.ToHitArmorClass0 = parent.GetStatistic(StatisticTypes.ToHitArmorClass0);
            mob.HitRoll = new DiceData { Bonus = parent.HitDice.Bonus };
            mob.DamageRoll = new DiceData { Bonus = parent.DamageDice.Bonus };
            mob.PermanentStrength = parent.GetStatistic(StatisticTypes.Strength);
            mob.PermanentDexterity = parent.GetStatistic(StatisticTypes.Dexterity);
            mob.PermanentWisdom = parent.GetStatistic(StatisticTypes.Wisdom);
            mob.PermanentIntelligence = parent.GetStatistic(StatisticTypes.Intelligence);
            mob.PermanentConstitution = parent.GetStatistic(StatisticTypes.Constitution);
            mob.PermanentCharisma = parent.GetStatistic(StatisticTypes.Charisma);
            mob.PermanentLuck = parent.GetStatistic(StatisticTypes.Luck);
            mob.CurrentRace = EnumerationExtensions.GetEnum<RaceTypes>(parent.Race);
            mob.CurrentClass = EnumerationExtensions.GetEnum<ClassTypes>(parent.Class);
            mob.ExtraFlags = parent.ExtraFlags;
            mob.SavingThrows = new SavingThrowData(parent.SavingThrows);
            mob.Height = parent.Height;
            mob.Weight = parent.Weight;
            mob.Resistance = parent.Resistance;
            mob.Immunity = parent.Immunity;
            mob.Susceptibility = parent.Susceptibility;
            mob.Attacks = parent.Attacks;
            mob.Defenses = parent.Defenses;
            mob.NumberOfAttacks = parent.NumberOfAttacks;
            mob.Speaks = parent.Speaks;
            mob.Speaking = parent.Speaking;

            Add(mob.ID, mob);

            return mob;
        }
    }
}
