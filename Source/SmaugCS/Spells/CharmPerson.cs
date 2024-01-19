using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;

namespace SmaugCS.Spells
{
    public static class CharmPerson
    {
        public static ReturnTypes spell_charm_person(int sn, int level, CharacterInstance ch, object vo)
        {
            var victim = (CharacterInstance)vo;
            var skill = Program.RepositoryManager.GetEntity<SkillData>(sn);

            if (CheckFunctions.CheckIfEquivalent(ch, ch, victim, "You like yourself even better!"))
                return ReturnTypes.SpellFailed;

            if (CheckFunctions.CheckIfTrueCasting(victim.IsImmune(ResistanceTypes.Magic)
                || victim.IsImmune(ResistanceTypes.Charm), skill, ch, CastingFunctionType.Immune, victim))
                return ReturnTypes.SpellFailed;

            if (!victim.IsNpc() && !ch.IsNpc())
            {
                ch.SendTo("I don't think so...");
                victim.SendTo("You feel charmed...");
                return ReturnTypes.SpellFailed;
            }

            var schance = victim.ModifySavingThrowWithResistance(level, ResistanceTypes.Charm);

            if (victim.IsAffected(AffectedByTypes.Charm)
                || schance == 1000
                || ch.IsAffected(AffectedByTypes.Charm)
                || level < victim.Level
                || victim.IsCircleFollowing(ch)
                || !ch.CanCharm()
                || victim.SavingThrows.CheckSaveVsSpellStaff(schance, victim))
            {
                ch.FailedCast(skill, victim);
                return ReturnTypes.SpellFailed;
            }

            if (victim.Master != null)
                victim.StopFollower();
            victim.AddFollower(ch);

            var af = new AffectData
            {
                SkillNumber = sn,
                Duration = GetDuration(level)
            };
            victim.AddAffect(af);

            ch.SuccessfulCast(skill, victim);

            if (!ch.IsNpc())
                ((PlayerInstance)ch).PlayerData.NumberOfCharmies++;
            if (!victim.IsNpc()) return ReturnTypes.None;

            var mob = (MobileInstance)victim;
            mob.StartHating(ch);
            mob.StartHunting(ch);

            return ReturnTypes.None;
        }

        private static int GetDuration(int level)
        {
            return (SmaugRandom.Fuzzy((level + 1) / 5) + 1) *
                   GameConstants.GetConstant<int>("AffectDurationConversionValue");
        }
    }
}
