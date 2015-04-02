using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Templates;

namespace SmaugCS.Extensions
{
    public static class MobTemplateExtensions
    {
        public static int GetRace(this MobTemplate template)
        {
            return (int)Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<RaceTypes>(template.Race);
        }

        public static PositionTypes GetPosition(this MobTemplate template)
        {
            return Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<PositionTypes>(template.Position);
        }

        public static PositionTypes GetDefensivePosition(this MobTemplate template)
        {
            return Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<PositionTypes>(template.DefensivePosition);
        }

        public static GenderTypes GetGender(this MobTemplate template)
        {
            return Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<GenderTypes>(template.Gender);
        }

        public static int GetResistance(this MobTemplate template)
        {
            var value = 0;
            var words = template.Resistance.Split(new[] { ' ' });
            foreach (var resType in words.Select(Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>))
            {
                value = value.SetBit(resType);
            }
            return value;
        }

        public static int GetSusceptibility(this MobTemplate template)
        {
            var value = 0;
            var words = template.Susceptibility.Split(new[] { ' ' });
            foreach (var resType in words.Select(Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>))
            {
                value = value.SetBit(resType);
            }
            return value;
        }

        public static int GetImmunity(this MobTemplate template)
        {
            var value = 0;
            var words = template.Immunity.Split(new[] { ' ' });
            foreach (var resType in words.Select(Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>))
            {
                value = value.SetBit(resType);
            }
            return value;
        }

        public static int GetActFlags(this MobTemplate template)
        {
            var bv = 0;
            var words = template.ActFlags.Split(new[] { ' ' });
            foreach (var word in words)
            {
                bv.SetBit(Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ActFlags>(word));
            }
            return bv;
        }

        public static int GetAffected(this MobTemplate template)
        {
            var bv = 0;
            var words = template.AffectedBy.Split(new[] { ' ' });
            foreach (var word in words)
            {
                bv.SetBit(Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<AffectedByTypes>(word));
            }
            return bv;
        }

        public static ExtendedBitvector GetAttacks(this MobTemplate template)
        {
            var bv = new ExtendedBitvector();
            var words = template.Attacks.Split(new[] { ' ' });
            foreach (var word in words)
            {
                bv.SetBit((int)Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<AttackTypes>(word));
            }
            return bv;
        }

        public static int GetDefenses(this MobTemplate template)
        {
            var flags = 0;
            var words = template.Defenses.Split(new[] { ' ' });

            foreach (var word in words)
            {
                flags.SetBit((int)Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<DefenseTypes>(word));
            }

            return flags;
        }
    }
}
