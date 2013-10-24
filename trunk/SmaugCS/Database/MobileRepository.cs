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
                newMob.Alignment = cloneMob.Alignment;
                newMob.Level = cloneMob.Level;
                newMob.ToHitArmorClass0 = cloneMob.ToHitArmorClass0;
                newMob.ArmorClass = cloneMob.ArmorClass;
                newMob.HitDice = new DiceData(cloneMob.HitDice);
                newMob.DamageDice = new DiceData(cloneMob.DamageDice);
                newMob.Gold = cloneMob.Gold;
                newMob.Experience = cloneMob.Experience;
                newMob.Position = cloneMob.Position;
                newMob.DefPosition = cloneMob.DefPosition;
                newMob.Gender = cloneMob.Gender;
                newMob.PermanentStrength = cloneMob.PermanentStrength;
                newMob.PermanentDexterity = cloneMob.PermanentDexterity;
                newMob.PermanentIntelligence = cloneMob.PermanentIntelligence;
                newMob.PermanentWisdom = cloneMob.PermanentWisdom;
                newMob.PermanentCharisma = cloneMob.PermanentCharisma;
                newMob.PermanentConstitution = cloneMob.PermanentConstitution;
                newMob.PermanentLuck = cloneMob.PermanentLuck;
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
                    PermanentStrength = 13,
                    PermanentDexterity = 13,
                    PermanentIntelligence = 13,
                    PermanentWisdom = 13,
                    PermanentCharisma = 13,
                    PermanentConstitution = 13,
                    PermanentLuck = 13,
                    Class = 3
                };
            newMob.Act.SetBit((int)ActFlags.IsNpc);
            newMob.Act.SetBit((int)ActFlags.Prototype);

            Add(vnum, newMob);
            return newMob;
        }
    }
}
