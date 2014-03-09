using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

// ReSharper disable CheckNamespace
namespace SmaugCS
// ReSharper restore CheckNamespace
{
    public static class MobTemplateExtensions
    {
        public static int GetRace(this MobTemplate template)
        {
            return (int)EnumerationExtensions.GetEnumIgnoreCase<RaceTypes>(template.Race);
        }

        public static PositionTypes GetPosition(this MobTemplate template)
        {
            return EnumerationExtensions.GetEnumIgnoreCase<PositionTypes>(template.Position);
        }

        public static PositionTypes GetDefensivePosition(this MobTemplate template)
        {
            return EnumerationExtensions.GetEnumIgnoreCase<PositionTypes>(template.DefensivePosition);
        }

        public static GenderTypes GetGender(this MobTemplate template)
        {
            return EnumerationExtensions.GetEnumIgnoreCase<GenderTypes>(template.Gender);
        }

        public static int GetResistance(this MobTemplate template)
        {
            int value = 0;
            string[] words = template.Resistance.Split(new[] { ' ' });
            foreach (ResistanceTypes resType in words.Select(EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>))
            {
                value = value.SetBit((int)resType);
            }
            return value;
        }

        public static int GetSusceptibility(this MobTemplate template)
        {
            int value = 0;
            string[] words = template.Susceptibility.Split(new[] { ' ' });
            foreach (ResistanceTypes resType in words.Select(EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>))
            {
                value = value.SetBit((int)resType);
            }
            return value;
        }

        public static int GetImmunity(this MobTemplate template)
        {
            int value = 0;
            string[] words = template.Immunity.Split(new[] { ' ' });
            foreach (ResistanceTypes resType in words.Select(EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>))
            {
                value = value.SetBit((int)resType);
            }
            return value;
        }

        public static ExtendedBitvector GetActFlags(this MobTemplate template)
        {
            ExtendedBitvector bv = new ExtendedBitvector();
            string[] words = template.ActFlags.Split(new[] { ' ' });
            foreach (string word in words)
            {
                bv.SetBit((int)EnumerationExtensions.GetEnumIgnoreCase<ActFlags>(word));
            }
            return bv;
        }

        public static ExtendedBitvector GetAffected(this MobTemplate template)
        {
            ExtendedBitvector bv = new ExtendedBitvector();
            string[] words = template.AffectedBy.Split(new[] { ' ' });
            foreach (string word in words)
            {
                bv.SetBit((int)EnumerationExtensions.GetEnumIgnoreCase<AffectedByTypes>(word));
            }
            return bv;
        }

        public static ExtendedBitvector GetAttacks(this MobTemplate template)
        {
            ExtendedBitvector bv = new ExtendedBitvector();
            string[] words = template.Attacks.Split(new[] { ' ' });
            foreach (string word in words)
            {
                bv.SetBit((int)EnumerationExtensions.GetEnumIgnoreCase<AttackTypes>(word));
            }
            return bv;
        }

        public static ExtendedBitvector GetDefenses(this MobTemplate template)
        {
            ExtendedBitvector bv = new ExtendedBitvector();
            string[] words = template.Defenses.Split(new[] { ' ' });
            foreach (string word in words)
            {
                bv.SetBit((int)EnumerationExtensions.GetEnumIgnoreCase<DefenseTypes>(word));
            }
            return bv;
        }
    }
}
