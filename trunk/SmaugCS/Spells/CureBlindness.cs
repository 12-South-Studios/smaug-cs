using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Helpers;
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

            if (CheckFunctions.CheckIfTrue(ch, ch.IsAffected(AffectedByTypes.Blind),
                ch != victim
                    ? "You work your cure, but it has no apparent effect."
                    : "You don't seem to be blind.")) return ReturnTypes.SpellFailed;
            
            // TODO: affect_strip(victim, AffectedByTypes.Blind);
            
            color.set_char_color(ATTypes.AT_MAGIC, victim);
            color.send_to_char("Your vision returns!", victim);
            if (ch != victim)
                color.send_to_char("You work your cure, restoring vision.", ch);
            return ReturnTypes.None;
        }
    }
}
