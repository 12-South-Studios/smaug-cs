using System;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Extensions;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using EnumerationExtensions = Realm.Library.Common.EnumerationExtensions;

namespace SmaugCS.Repository
{
    public class CharacterRepository : Repository<long, CharacterInstance>, IInstanceRepository<CharacterInstance>
    {
        private static long _idSpace = 1;
        private static long GetNextId { get { return _idSpace++; } }

        public CharacterInstance Create(Template parent, params object[] args)
        {
            Validation.IsNotNull(parent, "parent");
            Validation.Validate(parent is MobTemplate, "Invalid Template Type");

            var mobParent = parent.CastAs<MobTemplate>();

            long id;
            if (args != null && args.Length > 0)
                id = Convert.ToInt64(args[0]);
            else
                id = GetNextId;

            var name = parent.Name;
            if (args != null && args.Length > 1)
                name = args[1].ToString();

            var isMobile = false;
            if (args != null && args.Length > 2)
                isMobile = (bool) args[2];

            var mob = new CharacterInstance(id, name)
            {
                Parent = parent,
                ShortDescription = mobParent.ShortDescription,
                LongDescription = mobParent.LongDescription,
                Description = parent.Description,
                Level = SmaugRandom.Fuzzy(mobParent.Level),
                Act = mobParent.GetActFlags(),
                HomeVNum = -1,
                ResetVnum = -1,
                ResetNum = -1,
                AffectedBy = mobParent.GetAffected(),
                CurrentAlignment = mobParent.GetStatistic<int>(StatisticTypes.Alignment),
                Gender = EnumerationExtensions.GetEnum<GenderTypes>(mobParent.GetStatistic<string>(StatisticTypes.Gender))
            };

            if (isMobile)
            {
                var mi = (MobileInstance) mob;
                mi.SpecialFunction = mobParent.SpecialFunction;

                if (!string.IsNullOrEmpty(mobParent.SpecFun))
                    mi.SpecialFunctionName = mobParent.SpecFun;
            }

            if (mob.Act.IsSet(ActFlags.MobInvisibility))
                mob.MobInvisible = mob.Level;

            mob.ArmorClass = mobParent.GetStatistic<int>(StatisticTypes.ArmorClass) > 0
                                 ? mobParent.GetStatistic<int>(StatisticTypes.ArmorClass)
                                 : mob.Level.Interpolate(100, -100);

            if (mobParent.HitDice == null || mobParent.HitDice.NumberOf == 0)
                mob.MaximumHealth = mob.Level*8 + SmaugRandom.Between(mob.Level*mob.Level/4, mob.Level*mob.Level);
            else
                mob.MaximumHealth = mobParent.HitDice.NumberOf*SmaugRandom.Between(1, mobParent.HitDice.SizeOf) +
                                    mobParent.HitDice.Bonus;

            mob.CurrentCoin = mobParent.GetStatistic<int>(StatisticTypes.Coin);
            mob.Experience = mobParent.GetStatistic<int>(StatisticTypes.Experience);
            mob.CurrentPosition = mobParent.GetPosition();
            mob.CurrentDefensivePosition = mobParent.GetDefensivePosition();
            mob.BareDice = new DiceData
            {
                NumberOf = mobParent.DamageDice.NumberOf, 
                SizeOf = mobParent.DamageDice.SizeOf
            };
            mob.ToHitArmorClass0 = mobParent.GetStatistic<int>(StatisticTypes.ToHitArmorClass0);
            mob.HitRoll = new DiceData
            {
                Bonus = mobParent.HitDice.Bonus
            };
            mob.DamageRoll = new DiceData
            {
                Bonus = mobParent.DamageDice.Bonus
            };
            mob.PermanentStrength = mobParent.GetStatistic<int>(StatisticTypes.PermanentStrength);
            mob.PermanentDexterity = mobParent.GetStatistic<int>(StatisticTypes.PermanentDexterity);
            mob.PermanentWisdom = mobParent.GetStatistic<int>(StatisticTypes.PermanentWisdom);
            mob.PermanentIntelligence = mobParent.GetStatistic<int>(StatisticTypes.PermanentIntelligence);
            mob.PermanentConstitution = mobParent.GetStatistic<int>(StatisticTypes.PermanentConstitution);
            mob.PermanentCharisma = mobParent.GetStatistic<int>(StatisticTypes.PermanentCharisma);
            mob.PermanentLuck = mobParent.GetStatistic<int>(StatisticTypes.PermanentLuck);
            mob.CurrentRace = EnumerationExtensions.GetEnum<RaceTypes>(mobParent.GetRace());
            mob.CurrentClass = EnumerationExtensions.GetEnum<ClassTypes>(mobParent.Class);
            mob.ExtraFlags = mobParent.ExtraFlags;
            mob.SavingThrows = new SavingThrowData(mobParent.SavingThrows);
            mob.Height = mobParent.GetStatistic<int>(StatisticTypes.Height);
            mob.Weight = mobParent.GetStatistic<int>(StatisticTypes.Weight);
            mob.Resistance = mobParent.GetResistance();
            mob.Immunity = mobParent.GetImmunity();
            mob.Susceptibility = mobParent.GetSusceptibility();

            if (isMobile)
                ((MobileInstance)mob).Attacks = new ExtendedBitvector(mobParent.GetAttacks());

            mob.Defenses = mobParent.GetDefenses();
            mob.NumberOfAttacks = mobParent.GetStatistic<int>(StatisticTypes.NumberOfAttacks);
            //mob.Speaks = build.get_langflag(mobParent.Speaks);
            //mob.Speaking = build.get_langflag(mobParent.Speaking);

            Add(mob.ID, mob);

            return mob;
        }

        public CharacterInstance Clone(CharacterInstance source, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
