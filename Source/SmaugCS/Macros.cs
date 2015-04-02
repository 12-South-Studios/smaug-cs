using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class Macros
    {
        public static string PERS(CharacterInstance ch, CharacterInstance looker)
        {
            return ch.CanSee(looker)
                       ? ch.IsNpc() ? ch.ShortDescription : ch.Name
                       : "someone";
        }

        public static bool CHECK_SUBRESTRICTED(PlayerInstance ch)
        {
            return !CheckFunctions.CheckIfTrue(ch, ch.SubState == CharacterSubStates.Restricted,
                "You cannot use this command from within another command.");
        }

        public static bool NO_WEATHER_SECT(SectorTypes sect)
        {
            return sect == SectorTypes.Inside ||
                   sect == SectorTypes.Underwater ||
                   sect == SectorTypes.OceanFloor ||
                   sect == SectorTypes.Underground;
        }

        public static void WAIT_STATE(CharacterInstance ch, int npulse)
        {
            ch.wait = (short)((!ch.IsNpc()
                && ((PlayerInstance)ch).PlayerData.Nuisance != null
                && (((PlayerInstance)ch).PlayerData.Nuisance.Flags > 4))
                ? ch.wait.GetHighestOfTwoNumbers((npulse + (((PlayerInstance)ch).PlayerData.Nuisance.Flags - 4) + (short)((PlayerInstance)ch).PlayerData.Nuisance.Power))
                          : ch.wait.GetHighestOfTwoNumbers(npulse));
        }

        public static bool IS_VALID_SN(int sn)
        {
            return DatabaseManager.Instance.SKILLS.Get(sn) != null;
        }
        public static bool IS_VALID_DISEASE(int sn)
        {
            return sn >= 0 && sn < Program.MAX_DISEASE && db.DISEASES[sn] != null && !string.IsNullOrEmpty(db.DISEASES[sn].Name);
        }
        public static bool IS_PACIFIST(CharacterInstance ch)
        {
            return ch.IsNpc() && ch.Act.IsSet(ActFlags.Pacifist);
        }

        public static int SPELL_DAMAGE(SkillData skill)
        {
            return (skill.Info & 7);
        }
        public static int SPELL_ACTION(SkillData skill)
        {
            return ((skill.Info >> 3) & 7);
        }
        public static int SPELL_CLASS(SkillData skill)
        {
            return ((skill.Info >> 6) & 7);
        }
        public static int SPELL_POWER(SkillData skill)
        {
            return ((skill.Info >> 9) & 3);
        }
        public static int SPELL_SAVE(SkillData skill)
        {
            return ((skill.Info >> 11) & 7);
        }
        public static void SET_SDAM(SkillData skill, int val)
        {
            skill.Info = (skill.Info & Program.SDAM_MASK) + (val & 7);
        }
        public static void SET_SACT(SkillData skill, int val)
        {
            skill.Info = (skill.Info & Program.SDAM_MASK) + ((val & 7) << 3);
        }
        public static void SET_SCLA(SkillData skill, int val)
        {
            skill.Info = (skill.Info & Program.SDAM_MASK) + ((val & 7) << 6);
        }
        public static void SET_SPOW(SkillData skill, int val)
        {
            skill.Info = (skill.Info & Program.SDAM_MASK) + ((val & 3) << 9);
        }
        public static void SET_SSAV(SkillData skill, int val)
        {
            skill.Info = (skill.Info & Program.SDAM_MASK) + ((val & 7) << 11);
        }

        public static bool IS_FIRE(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(DatabaseManager.Instance.SKILLS.Values.ToList()[dt]) == (int)SpellDamageTypes.Fire;
        }
        public static bool IS_COLD(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(DatabaseManager.Instance.SKILLS.Values.ToList()[dt]) == (int)SpellDamageTypes.Cold;
        }
        public static bool IS_ACID(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(DatabaseManager.Instance.SKILLS.Values.ToList()[dt]) == (int)SpellDamageTypes.Acid;
        }
        public static bool IS_ELECTRICITY(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(DatabaseManager.Instance.SKILLS.Values.ToList()[dt]) == (int)SpellDamageTypes.Electricty;
        }
        public static bool IS_ENERGY(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(DatabaseManager.Instance.SKILLS.Values.ToList()[dt]) == (int)SpellDamageTypes.Energy;
        }
        public static bool IS_DRAIN(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(DatabaseManager.Instance.SKILLS.Values.ToList()[dt]) == (int)SpellDamageTypes.Drain;
        }
        public static bool IS_POISON(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(DatabaseManager.Instance.SKILLS.Values.ToList()[dt]) == (int)SpellDamageTypes.Poison;
        }

        public static string NAME(CharacterInstance ch)
        {
            return ch.IsNpc() ? ch.ShortDescription : ch.Name;
        }

        public static string MORPHERS(CharacterInstance ch, CharacterInstance looker)
        {
            return looker.CanSee(ch) ? ch.CurrentMorph.Morph.ShortDescription : "someone";
        }

        public static void DamageMessage(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            fight.new_dam_message(ch, victim, dam, dt, null);
        }

        public static long LEARNED(CharacterInstance ch, int sn)
        {
            return ch.IsNpc() ? 80 : ((PlayerInstance)ch).PlayerData.Learned.ToList()[sn].GetNumberThatIsBetween(0, 101);
        }
    }
}
