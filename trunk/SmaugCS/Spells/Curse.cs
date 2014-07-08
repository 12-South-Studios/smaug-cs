using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.Spells
{
    public static class Curse
    {
        public static ReturnTypes spell_curse(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance) vo;
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);

            if (victim.Immunity.IsSet((int)ResistanceTypes.Magic))
            {
                magic.immune_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            if (victim.IsAffected(AffectedByTypes.Curse) || victim.SavingThrows.CheckSaveVsSpellStaff(level, victim))
            {
                magic.failed_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            AffectData af = new AffectData
            {
                SkillNumber = sn,
                Duration = ((4*level)*GameConstants.GetIntegerConstant("AffectDurationConversionValue")),
                Location = ApplyTypes.HitRoll,
                Modifier = -1
            };
            //af.BitVector = meb(AFF_CURSE);
            victim.AddAffect(af);
            
            af = new AffectData
            {
                SkillNumber = sn,
                Duration = ((4*level)*GameConstants.GetIntegerConstant("AffectDurationConversionValue")),
                Location = ApplyTypes.SaveVsSpell,
                Modifier = 1
            };
            victim.AddAffect(af);

            color.set_char_color(ATTypes.AT_MAGIC, victim);
            color.send_to_char("You feel unclean.\r\n", victim);

            if (ch != victim)
            {
                comm.act(ATTypes.AT_MAGIC, "You utter a curse upon $N.", ch, null, victim, ToTypes.Character);
                comm.act(ATTypes.AT_MAGIC, "$n utters a curse upon $N.", ch, null, victim, ToTypes.NotVictim);
            }

            return ReturnTypes.None;
        }
    }
}
