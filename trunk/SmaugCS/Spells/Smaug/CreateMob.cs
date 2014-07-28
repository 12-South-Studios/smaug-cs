﻿using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.Spells.Smaug
{
    public static class CreateMob
    {
        public static ReturnTypes spell_create_mob(int sn, int level, CharacterInstance ch, object vo)
        {
            SkillData skill = DatabaseManager.Instance.SKILLS.Get(sn);

            string targetName = string.Empty; // TODO Get this from do_cast somehow!

            int lvl = GetMobLevel(skill, level);
            int id = skill.value;

            if (id == 0)
            {
                if (!targetName.Equals("cityguard"))
                    id = GameConstants.GetVnum("cityguard");
                else if (!targetName.Equals("vampire"))
                    id = GameConstants.GetVnum("vampire");
            }

            MobTemplate mi = DatabaseManager.Instance.MOBILE_INDEXES.Get(id);
            if (mi == null)
            {
                magic.failed_casting(skill, ch, null, null);
                return ReturnTypes.None;
            }

            CharacterInstance mob = DatabaseManager.Instance.CHARACTERS.Create(mi);
            if (mob == null)
            {
                magic.failed_casting(skill, ch, null, null);
                return ReturnTypes.None;
            }

            mob.Level = lvl.GetLowestOfTwoNumbers(!string.IsNullOrEmpty(skill.Dice) ? magic.dice_parse(ch, level, skill.Dice) : mob.Level);
            mob.ArmorClass = mob.Level.Interpolate(100, -100);
            mob.MaximumHealth = mob.Level*8 + SmaugRandom.Between(mob.Level*mob.Level/4, mob.Level*mob.Level);
            mob.CurrentHealth = mob.MaximumHealth;
            mob.CurrentCoin = 0;

            magic.successful_casting(skill, ch, mob, null);
            ch.CurrentRoom.ToRoom(mob);
            mob.AddFollower(ch);

            AffectData af = new AffectData
            {
                Type = Realm.Library.Common.EnumerationExtensions.GetEnum<AffectedByTypes>((int) skill.ID),
                Duration = (SmaugRandom.Fuzzy((level + 1)/3) + 1)*
                           GameConstants.GetIntegerConstant("AffectDurationConversionValue")
            };
            mob.AddAffect(af);

            return ReturnTypes.None;
        }

        private static int GetMobLevel(SkillData skill, int level)
        {
            switch (Macros.SPELL_POWER(skill))
            {
                case (int)SpellPowerTypes.Major:
                    return level;
                case (int)SpellPowerTypes.Greater:
                    return level/2;
                case (int)SpellPowerTypes.Minor:
                    return 5;
                default:
                    return 20;
            }
        }
    }
}
