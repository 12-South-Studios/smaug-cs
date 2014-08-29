using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Extensions;
using SmaugCS.Managers;
using SmaugCS.Common;

namespace SmaugCS.Spells
{
    public static class Blindness
    {
        public static ReturnTypes spell_blindness(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance) vo;
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);

            int tmp = skill.Flags.IsSet(SkillFlags.PKSensitive) ? level/2 : level;

            if (victim.IsImmune(ResistanceTypes.Magic))
            {
                ch.ImmuneCast(skill, victim);
                return ReturnTypes.SpellFailed;
            }

            if (ch.IsAffected(AffectedByTypes.Blind) || ch.SavingThrows.CheckSaveVsSpellStaff(tmp, victim))
            {
                ch.FailedCast(skill, victim);
                return ReturnTypes.SpellFailed;
            }

            AffectData af = AffectData.Create();
            af.SkillNumber = sn;
            af.Location = ApplyTypes.HitRoll;
            af.Modifier = -4;
            af.Duration = (1 + (level / 3)) * GameConstants.GetConstant<int>("AffectDurationConversionValue");

            victim.AddAffect(af);
            color.set_char_color(ATTypes.AT_MAGIC, victim);
            color.send_to_char("You are blinded!", victim);

            if (ch != victim)
            {
                comm.act(ATTypes.AT_MAGIC, "You weave a spell of blindness around $N.", ch, null, victim, ToTypes.Character);
                comm.act(ATTypes.AT_MAGIC, "$n weaves a spell of blindness about $N.", ch, null, victim, ToTypes.NotVictim);
            }

            return ReturnTypes.None;
        }
    }
}
