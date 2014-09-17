using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Spells
{
    public static class CureBlindness
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "level")]
        public static ReturnTypes spell_cure_blindness(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance)vo;
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);

            color.set_char_color(ATTypes.AT_MAGIC, ch);

            if (CheckFunctions.CheckIfTrueCasting(victim.IsImmune(ResistanceTypes.Magic), skill, ch,
                CastingFunctionType.Immune, victim)) return ReturnTypes.SpellFailed;

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
