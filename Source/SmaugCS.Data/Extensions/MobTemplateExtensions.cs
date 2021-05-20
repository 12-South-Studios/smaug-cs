using System.Linq;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Templates;
using EnumerationExtensions = Realm.Library.Common.Extensions.EnumerationExtensions;

namespace SmaugCS.Data.Extensions
{
    public static class MobTemplateExtensions
    {
        public static int GetRace(this MobileTemplate template)
            => (int) EnumerationExtensions.GetEnumIgnoreCase<RaceTypes>(template.Race);

        public static PositionTypes GetPosition(this MobileTemplate template)
            => EnumerationExtensions.GetEnumIgnoreCase<PositionTypes>(template.Position);

        public static PositionTypes GetDefensivePosition(this MobileTemplate template)
            => EnumerationExtensions.GetEnumIgnoreCase<PositionTypes>(template.DefensivePosition);

        public static GenderTypes GetGender(this MobileTemplate template)
            =>
                EnumerationExtensions.GetEnumIgnoreCase<GenderTypes>(template.GetStatistic<string>(StatisticTypes.Gender));

        public static int GetResistance(this MobileTemplate template)
            => template.Resistance.Split(' ')
                    .Select(EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>)
                    .Aggregate(0, (current, resType) => current.SetBit(resType));

        public static int GetSusceptibility(this MobileTemplate template)
            => template.Susceptibility.Split(' ')
                .Select(EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>)
                .Aggregate(0, (current, resType) => current.SetBit(resType));

        public static int GetImmunity(this MobileTemplate template)
            => template.Immunity.Split(' ')
                .Select(EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>)
                .Aggregate(0, (current, resType) => current.SetBit(resType));

        public static int GetActFlags(this MobileTemplate template)
        {
            var bv = 0;
            var words = template.GetStatistic<string>(StatisticTypes.ActFlags).Split(' ');
            foreach (var word in words)
            {
                bv.SetBit(EnumerationExtensions.GetEnumIgnoreCase<ActFlags>(word));
            }
            return bv;
        }

        public static int GetAffected(this MobileTemplate template)
        {
            var bv = 0;
            var words = template.GetStatistic<string>(StatisticTypes.AffectedByFlags).Split(' ');
            foreach (var word in words)
            {
                bv.SetBit(EnumerationExtensions.GetEnumIgnoreCase<AffectedByTypes>(word));
            }
            return bv;
        }

        public static ExtendedBitvector GetAttacks(this MobileTemplate template)
        {
            var bv = new ExtendedBitvector();
            var words = template.Attacks.Split(' ');
            foreach (var word in words)
            {
                bv.SetBit((int)EnumerationExtensions.GetEnumIgnoreCase<AttackTypes>(word));
            }
            return bv;
        }

        public static int GetDefenses(this MobileTemplate template)
        {
            var flags = 0;
            var words = template.Defenses.Split(' ');

            foreach (var word in words)
            {
                flags.SetBit((int)EnumerationExtensions.GetEnumIgnoreCase<DefenseTypes>(word));
            }

            return flags;
        }
    }
}
