using System.Collections.Generic;

namespace SmaugCS.Objects
{
    public class SkillData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Info { get; set; }
        public int Flags { get; set; }
        public short Target { get; set; }

        public string SpellFunctionName { get; set; }
        public DoFunction SkillFunction { get; set; }
        public string SkillFunctionName { get; set; }
        public SpellFunction SpellFunction { get; set; }


        public short[] skill_level { get; set; }
        public short[] skill_adept { get; set; }
        public short[] race_level { get; set; }
        public short[] race_adept { get; set; }
        public short minimum_position { get; set; }
        public short slot { get; set; }
        public short min_mama { get; set; }
        public short beats { get; set; }
        public string noun_damage { get; set; }
        public string msg_off { get; set; }
        public short guild { get; set; }
        public short min_level { get; set; }
        public short range { get; set; }
        public string hit_char { get; set; }
        public string hit_vict { get; set; }
        public string hit_room { get; set; }
        public string hit_dest { get; set; }
        public string miss_char { get; set; }
        public string miss_vict { get; set; }
        public string miss_room { get; set; }
        public string die_char { get; set; }
        public string die_vict { get; set; }
        public string die_room { get; set; }
        public string imm_char { get; set; }
        public string imm_vict { get; set; }
        public string imm_room { get; set; }
        public string dice { get; set; }
        public int value { get; set; }
        public int spell_sector { get; set; }
        public char saves { get; set; }
        public char difficulty { get; set; }
        public List<smaug_affect> affects { get; set; }
        public string components { get; set; }
        public string teachers { get; set; }
        public char participants { get; set; }
        public timerset userec { get; set; }

        public SkillData()
        {
            skill_level = new short[Program.MAX_CLASS];
            skill_adept = new short[Program.MAX_CLASS];
            race_level = new short[Program.MAX_RACE];
            race_adept = new short[Program.MAX_RACE];
        }
    }
}
