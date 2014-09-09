using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.Spells.Smaug
{
    public static class AffectCharacter
    {
        public static ReturnTypes spell_affectchar(int sn, int level, CharacterInstance ch, object vo)
        {
            SkillData skill = DatabaseManager.Instance.SKILLS.Get(sn);
            CharacterInstance victim = (CharacterInstance)vo;

            if (skill.Flags.IsSet(SkillFlags.ReCastable))
                victim.StripAffects(sn);

            bool first = true;
            bool affected = false;

            foreach (SmaugAffect saf in skill.Affects)
            {
                if (saf.Location >= Program.REVERSE_APPLY)
                {
                    if (!skill.Flags.IsSet(SkillFlags.Accumulative))
                    {
                        if (first)
                        {
                            if (skill.Flags.IsSet(SkillFlags.ReCastable))
                                ch.StripAffects(sn);
                            if (ch.IsAffectedBy(sn))
                                affected = true;
                        }
                        first = false;
                        if (affected)
                            continue;
                    }
                    victim = ch;
                }
                else
                    victim = (CharacterInstance)vo;

                // TODO Something with smaug bitvectors
            }

            victim.UpdatePositionByCurrentHealth();
            return ReturnTypes.None;
        }
    }
}