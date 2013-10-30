
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Exceptions;
using SmaugCS.Interfaces;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class Macros
    {
        public static int ASSIGN_GSN(string value)
        {
            int retVal = db.AddSkill(value);
            if (retVal == -1)
                throw new DuplicateSkillException("{0} already exists", value);
            return retVal;
        }

        public static string PERS(CharacterInstance ch, CharacterInstance looker)
        {
            return handler.can_see(ch, looker)
                       ? ch.IsNpc() ? ch.ShortDescription : ch.Name
                       : "someone";
        }

        public static bool CHECK_SUBRESTRICTED(CharacterInstance ch)
        {
            if (ch.SubState == CharacterSubStates.Restricted)
            {
                color.send_to_char("You cannot use this command from within another command.\n\r", ch);
                return false;
            }
            return true;
        }


        public static long GET_TIME_PLAYED(CharacterInstance ch)
        {
            // TODO return (ch.played + (current_time - ch.logon)) / 3600;
            return 0;
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
                && ch.PlayerData.Nuisance != null
                && (ch.PlayerData.Nuisance.Flags > 4))
                ? Check.Maximum(ch.wait, (npulse + (ch.PlayerData.Nuisance.Flags - 4) + (short)ch.PlayerData.Nuisance.Power))
                          : Check.Maximum(ch.wait, npulse));
        }

        public static ExitData EXIT(CharacterInstance ch, short door)
        {
            return ch.CurrentRoom.GetExit(door);
        }
        public static bool CAN_GO(CharacterInstance ch, short door)
        {
            ExitData exit = EXIT(ch, door);
            return exit != null && exit.Destination != null && !exit.Flags.IsSet((int)ExitFlags.Closed);
        }
        public static bool IS_VALID_SN(int sn)
        {
            return sn >= 0 && sn < Program.MAX_SKILL && db.SKILLS[sn] != null && !string.IsNullOrEmpty(db.SKILLS[sn].Name);
        }
        public static bool IS_VALID_HERB(int sn)
        {
            return sn >= 0 && sn < Program.MAX_HERB && db.HERBS[sn] != null && !string.IsNullOrEmpty(db.HERBS[sn].Name);
        }
        public static bool IS_VALID_DISEASE(int sn)
        {
            return sn >= 0 && sn < Program.MAX_DISEASE && db.DISEASES[sn] != null && !string.IsNullOrEmpty(db.DISEASES[sn].Name);
        }
        public static bool IS_PACIFIST(CharacterInstance ch)
        {
            return ch.IsNpc() && ch.Act.IsSet((int)ActFlags.Pacifist);
        }

        public static bool SPELL_FLAG(SkillData skill, int flag)
        {
            return skill.Flags.IsSet(flag);
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
            return IS_VALID_SN(dt) && SPELL_DAMAGE(db.SKILLS[dt]) == (int)SpellDamageTypes.Fire;
        }
        public static bool IS_COLD(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(db.SKILLS[dt]) == (int)SpellDamageTypes.Cold;
        }
        public static bool IS_ACID(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(db.SKILLS[dt]) == (int)SpellDamageTypes.Acid;
        }
        public static bool IS_ELECTRICITY(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(db.SKILLS[dt]) == (int)SpellDamageTypes.Electricty;
        }
        public static bool IS_ENERGY(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(db.SKILLS[dt]) == (int)SpellDamageTypes.Energy;
        }
        public static bool IS_DRAIN(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(db.SKILLS[dt]) == (int)SpellDamageTypes.Drain;
        }
        public static bool IS_POISON(int dt)
        {
            return IS_VALID_SN(dt) && SPELL_DAMAGE(db.SKILLS[dt]) == (int)SpellDamageTypes.Poison;
        }

        public static bool CAN_WEAR(ObjectInstance obj, int part)
        {
            return obj.WearFlags.IsSet(part);
        }
        public static bool IS_OBJ_STAT(IHasExtraFlags obj, int stat)
        {
            return obj.ExtraFlags.IsSet(stat);
        }

        public static string NAME(CharacterInstance ch)
        {
            return ch.IsNpc() ? ch.ShortDescription : ch.Name;
        }

        public static string MORPHERS(CharacterInstance ch, CharacterInstance looker)
        {
            return handler.can_see(looker, ch) ? ch.CurrentMorph.Morph.ShortDescription : "someone";
        }

        public static void DamageMessage(CharacterInstance ch, CharacterInstance victim, int dam, int dt)
        {
            fight.new_dam_message(ch, victim, dam, dt, null);
        }

        public static int GET_ADEPT(CharacterInstance ch, int sn)
        {
            return db.SKILLS[sn].skill_adept[(int)ch.CurrentClass];
        }

        public static int LEARNED(CharacterInstance ch, int sn)
        {
            return ch.IsNpc() ? 80 : Check.Range(0, ch.PlayerData.Learned[sn], 101);
        }
    }
}
