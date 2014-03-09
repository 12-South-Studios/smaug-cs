using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data;

using SmaugCS.Managers;

namespace SmaugCS.Spells
{
    public static class CureBlindness
    {
        public static ReturnTypes spell_cure_blindness(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance)vo;
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);

            color.set_char_color(ATTypes.AT_MAGIC, ch);

            if (victim.IsImmune(ResistanceTypes.Magic))
            {
                magic.immune_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            if (ch.IsAffected(AffectedByTypes.Blind))
            {
                color.send_to_char(
                    !ch.Equals(victim)
                        ? "You work your cure, but it has no apparent effect."
                        : "You don't seem to be blind.",
                    ch);
                return ReturnTypes.SpellFailed;
            }

            // TODO: affect_strip(victim, AffectedByTypes.Blind);
            
            color.set_char_color(ATTypes.AT_MAGIC, victim);
            color.send_to_char("Your vision returns!", victim);
            if (!ch.Equals(victim))
                color.send_to_char("You work your cure, restoring vision.", ch);
            return ReturnTypes.None;
        }
    }
}
