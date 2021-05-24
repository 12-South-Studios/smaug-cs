using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Repository;

namespace SmaugCS.Spells
{
    public static class AffectCharacter
    {
        public static ReturnTypes spell_affectchar(int sn, int level, CharacterInstance ch, object vo)
        {
            var skill = RepositoryManager.Instance.SKILLS.Get(sn);
            var victim = (CharacterInstance)vo;

            if (skill.Flags.IsSet(SkillFlags.ReCastable))
                victim.StripAffects(sn);

            var first = true;
            var affected = false;

            foreach (var saf in skill.Affects)
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