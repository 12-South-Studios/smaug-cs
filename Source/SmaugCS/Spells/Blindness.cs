using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Managers;
using SmaugCS.Common;

namespace SmaugCS.Spells
{
    public static class Blindness
    {
        public static ReturnTypes spell_blindness(int sn, int level, CharacterInstance ch, object vo)
        {
            var victim = (CharacterInstance) vo;
            var skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);

            var tmp = skill.Flags.IsSet(SkillFlags.PKSensitive) ? level/2 : level;

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

            var af = new AffectData
            {
                SkillNumber = sn,
                Location = ApplyTypes.HitRoll,
                Modifier = -4,
                Duration = (1 + (level/3))*GameConstants.GetConstant<int>("AffectDurationConversionValue")
            };

            victim.AddAffect(af);
           victim.SetColor(ATTypes.AT_MAGIC);
           victim.SendTo("You are blinded!");

            if (ch != victim)
            {
                comm.act(ATTypes.AT_MAGIC, "You weave a spell of blindness around $N.", ch, null, victim, ToTypes.Character);
                comm.act(ATTypes.AT_MAGIC, "$n weaves a spell of blindness about $N.", ch, null, victim, ToTypes.NotVictim);
            }

            return ReturnTypes.None;
        }
    }
}
