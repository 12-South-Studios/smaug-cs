using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Repositories
{
    public class CharacterRepository : Repository<long, CharacterInstance>, IInstanceRepository<CharacterInstance>
    {
        private static int _idSpace = 1;
        private static int GetNextId { get { return _idSpace++; } }

        public CharacterInstance Create(Template parent, params object[] args)
        {
            Validation.IsNotNull(parent, "parent");
            Validation.Validate(parent is MobTemplate, "Invalid Template Type");

            MobTemplate mobParent = parent.CastAs<MobTemplate>();
            CharacterInstance mob = CharacterInstance.Create(GetNextId, parent.Name);
            mob.Parent = parent;
            mob.ShortDescription = mobParent.ShortDescription;
            mob.LongDescription = mobParent.LongDescription;
            mob.Description = parent.Description;
            mob.SpecialFunction = mobParent.SpecialFunction;
            mob.Level = SmaugRandom.Fuzzy(mobParent.Level);
            mob.Act = mobParent.GetActFlags();
            mob.HomeVNum = -1;
            mob.ResetVnum = -1;
            mob.ResetNum = -1;
            mob.AffectedBy = mobParent.GetAffected();
            mob.CurrentAlignment = mobParent.GetStatistic(StatisticTypes.Alignment);
            mob.Gender = Realm.Library.Common.EnumerationExtensions.GetEnum<GenderTypes>(mobParent.Gender);

            if (!string.IsNullOrEmpty(mobParent.SpecFun))
                mob.SpecialFunctionName = mobParent.SpecFun;

            if (mob.Act.IsSet(ActFlags.MobInvisibility))
                mob.MobInvisible = mob.Level;

            mob.ArmorClass = mobParent.GetStatistic(StatisticTypes.ArmorClass) > 0
                                 ? mobParent.GetStatistic(StatisticTypes.ArmorClass)
                                 : mob.Level.Interpolate(100, -100);

            if (mobParent.HitDice == null || mobParent.HitDice.NumberOf == 0)
                mob.MaximumHealth = mob.Level * 8 + SmaugRandom.Between(mob.Level * mob.Level / 4, mob.Level * mob.Level);
            else
                mob.MaximumHealth = mobParent.HitDice.NumberOf * SmaugRandom.Between(1, mobParent.HitDice.SizeOf) + mobParent.HitDice.Bonus;

            mob.CurrentCoin = mobParent.Gold;
            mob.Experience = mobParent.Experience;
            mob.CurrentPosition = mobParent.GetPosition();
            mob.CurrentDefensivePosition = mobParent.GetDefensivePosition();
            mob.BareDice = new DiceData
            {
                NumberOf = mobParent.DamageDice.NumberOf, 
                SizeOf = mobParent.DamageDice.SizeOf
            };
            mob.ToHitArmorClass0 = mobParent.GetStatistic(StatisticTypes.ToHitArmorClass0);
            mob.HitRoll = new DiceData
            {
                Bonus = mobParent.HitDice.Bonus
            };
            mob.DamageRoll = new DiceData
            {
                Bonus = mobParent.DamageDice.Bonus
            };
            mob.PermanentStrength = mobParent.GetStatistic(StatisticTypes.Strength);
            mob.PermanentDexterity = mobParent.GetStatistic(StatisticTypes.Dexterity);
            mob.PermanentWisdom = mobParent.GetStatistic(StatisticTypes.Wisdom);
            mob.PermanentIntelligence = mobParent.GetStatistic(StatisticTypes.Intelligence);
            mob.PermanentConstitution = mobParent.GetStatistic(StatisticTypes.Constitution);
            mob.PermanentCharisma = mobParent.GetStatistic(StatisticTypes.Charisma);
            mob.PermanentLuck = mobParent.GetStatistic(StatisticTypes.Luck);
            mob.CurrentRace = Realm.Library.Common.EnumerationExtensions.GetEnum<RaceTypes>(mobParent.GetRace());
            mob.CurrentClass = Realm.Library.Common.EnumerationExtensions.GetEnum<ClassTypes>(mobParent.Class);
            mob.ExtraFlags = mobParent.ExtraFlags;
            mob.SavingThrows = SavingThrowData.Clone(mobParent.SavingThrows);
            mob.Height = mobParent.Height;
            mob.Weight = mobParent.Weight;
            mob.Resistance = mobParent.GetResistance();
            mob.Immunity = mobParent.GetImmunity();
            mob.Susceptibility = mobParent.GetSusceptibility();
            mob.Attacks = new ExtendedBitvector(mobParent.GetAttacks());
            mob.Defenses = mobParent.GetDefenses();
            mob.NumberOfAttacks = mobParent.NumberOfAttacks;
            //mob.Speaks = build.get_langflag(mobParent.Speaks);
            //mob.Speaking = build.get_langflag(mobParent.Speaking);

            Add(mob.ID, mob);

            return mob;
        }
    }
}
