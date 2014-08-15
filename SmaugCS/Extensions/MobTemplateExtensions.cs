using System.Linq;
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
            int value = 0;
            string[] words = template.Resistance.Split(new[] { ' ' });
            foreach (ResistanceTypes resType in words.Select(Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>))
            {
                value = value.SetBit(resType);
            }
            return value;
        }

        public static int GetSusceptibility(this MobTemplate template)
        {
            int value = 0;
            string[] words = template.Susceptibility.Split(new[] { ' ' });
            foreach (ResistanceTypes resType in words.Select(Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>))
            {
                value = value.SetBit(resType);
            }
            return value;
        }

        public static int GetImmunity(this MobTemplate template)
        {
            int value = 0;
            string[] words = template.Immunity.Split(new[] { ' ' });
            foreach (ResistanceTypes resType in words.Select(Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ResistanceTypes>))
            {
                value = value.SetBit(resType);
            }
            return value;
        }

        public static int GetActFlags(this MobTemplate template)
        {
            int bv = 0;
            string[] words = template.ActFlags.Split(new[] { ' ' });
            foreach (string word in words)
            {
                bv.SetBit(Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ActFlags>(word));
            }
            return bv;
        }

        public static int GetAffected(this MobTemplate template)
        {
            int bv = 0;
            string[] words = template.AffectedBy.Split(new[] { ' ' });
            foreach (string word in words)
            {
                bv.SetBit(Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<AffectedByTypes>(word));
            }
            return bv;
        }

        public static ExtendedBitvector GetAttacks(this MobTemplate template)
        {
            ExtendedBitvector bv = new ExtendedBitvector();
            string[] words = template.Attacks.Split(new[] { ' ' });
            foreach (string word in words)
            {
                bv.SetBit((int)Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<AttackTypes>(word));
            }
            return bv;
        }

        public static int GetDefenses(this MobTemplate template)
        {
            int flags = 0;
            string[] words = template.Defenses.Split(new[] { ' ' });

            foreach (string word in words)
            {
                flags.SetBit((int)Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<DefenseTypes>(word));
            }

            return flags;
        }
    }
}
