using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Spells.Smaug
{
    public static class CreateMob
    {
        public static ReturnTypes spell_create_mob(int sn, int level, CharacterInstance ch, object vo)
        {
            SkillData skill = DatabaseManager.Instance.SKILLS.Get(sn);

            string targetName = Cast.TargetName;

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
            if (CheckFunctions.CheckIfNullObjectCasting(mi, skill, ch)) return ReturnTypes.None;

            CharacterInstance mob = DatabaseManager.Instance.CHARACTERS.Create(mi);
            if (CheckFunctions.CheckIfNullObjectCasting(mob, skill, ch)) return ReturnTypes.None;

            mob.Level = lvl.GetLowestOfTwoNumbers(!string.IsNullOrEmpty(skill.Dice) ? magic.ParseDiceExpression(ch, skill.Dice) : mob.Level);
            mob.ArmorClass = mob.Level.Interpolate(100, -100);
            mob.MaximumHealth = mob.Level*8 + SmaugRandom.Between(mob.Level*mob.Level/4, mob.Level*mob.Level);
            mob.CurrentHealth = mob.MaximumHealth;
            mob.CurrentCoin = 0;

            ch.SuccessfulCast(skill, mob);
            ch.CurrentRoom.ToRoom(mob);
            mob.AddFollower(ch);

            AffectData af = new AffectData
            {
                Type = Realm.Library.Common.EnumerationExtensions.GetEnum<AffectedByTypes>((int) skill.ID),
                Duration = (SmaugRandom.Fuzzy((level + 1)/3) + 1)*
                           GameConstants.GetConstant<int>("AffectDurationConversionValue")
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
