using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Spells
{
    public static class CharmPerson
    {
        public static ReturnTypes spell_charm_person(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance)vo;
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);

            if (CheckFunctions.CheckIfEquivalent(ch, ch, victim, "You like yourself even better!"))
                return ReturnTypes.SpellFailed;

            if (victim.IsImmune(ResistanceTypes.Magic) || victim.IsImmune(ResistanceTypes.Charm))
            {
                magic.immune_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            if (!victim.IsNpc() && !ch.IsNpc())
            {
                color.send_to_char("I don't think so...", ch);
                color.send_to_char("You feel charmed...", victim);
                return ReturnTypes.SpellFailed;
            }

            int schance = magic.ris_save(victim, level, (int)ResistanceTypes.Charm);

            if (victim.IsAffected(AffectedByTypes.Charm)
                || schance == 1000
                || ch.IsAffected(AffectedByTypes.Charm)
                || level < victim.Level
                || victim.IsCircleFollowing(ch)
                || !ch.CanCharm()
                || victim.SavingThrows.CheckSaveVsSpellStaff(schance, victim))
            {
                magic.failed_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            if (victim.Master != null)
                victim.StopFollower();
            victim.AddFollower(ch);

            AffectData af = new AffectData
            {
                SkillNumber = sn,
                Duration = (SmaugRandom.Fuzzy((level + 1)/5) + 1)*
                           GameConstants.GetConstant<int>("AffectDurationConversionValue")
            };
            // af.BitVector = ExtendedBitvector.Meb((int) AffectedByTypes.Charm);
            victim.AddAffect(af);
            
            magic.successful_casting(skill, ch, victim, null);
            //TODO log_printf_plus( LOG_NORMAL, ch->level, "%s has charmed %s.", ch->name, victim->name );

            if (!ch.IsNpc())
                ch.PlayerData.NumberOfCharmies++;
            if (victim.IsNpc())
            {
                victim.StartHating(ch);
                victim.StartHunting(ch);
            }

            return ReturnTypes.None;
        }
    }
}
