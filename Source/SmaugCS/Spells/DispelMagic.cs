using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Repository;

namespace SmaugCS.Spells
{
    public static class DispelMagic
    {
        public static ReturnTypes spell_dispel_magic(int sn, int level, CharacterInstance ch, object vo)
        {
            var victim = (CharacterInstance)vo;
            var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);

            ch.SetColor(ATTypes.AT_MAGIC);

            if (CheckFunctions.CheckIfTrueCasting(victim.Immunity.IsSet(ResistanceTypes.Magic), skill, ch, CastingFunctionType.Immune, victim))
                return ReturnTypes.SpellFailed;
            
            if (CheckFunctions.CheckIfTrueCasting(victim.IsNpc() && victim.IsAffected(AffectedByTypes.Possess), skill,
                ch, CastingFunctionType.Immune, victim))
                return ReturnTypes.SpellFailed;

            if (ch == victim)
                return DispelSelf(ch, victim);

            var isMage = ch.IsNpc() || ch.CurrentClass == ClassTypes.Mage;
            if (!isMage && !ch.IsAffected(AffectedByTypes.DetectMagic))
            {
                ch.SendTo("You don't sense a magical aura to dispel.");
                return ReturnTypes.Error;
            }

            int chance = ch.GetCurrentIntelligence() - victim.GetCurrentIntelligence();
            if (isMage)
                chance += 5;
            else
                chance -= 15;

            // todo finish this from magic.c:2692

            return ReturnTypes.None;
        }

        private static ReturnTypes DispelSelf(CharacterInstance ch, CharacterInstance victim)
        {
            ch.SendTo("You pass your hands around your body...");
            if (ch.Affects.Any())
                return ReturnTypes.None;

            while (ch.Affects.Any())
            {
                var affect = ch.Affects.First();
                ch.RemoveAffect(affect);
            }

            if (!ch.IsNpc())
                victim.update_aris();
            return ReturnTypes.None;
        }
    }
}
