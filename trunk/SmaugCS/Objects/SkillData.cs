using System.Collections.Generic;
using Realm.Library.Common.Extensions;
using SmaugCS.Enums;

namespace SmaugCS.Objects
{
    public class SkillData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public SkillTypes Type { get; set; }
        public int Info { get; set; }
        public int Flags { get; set; }
        public TargetTypes Target { get; set; }

        public string SpellFunctionName { get; set; }
        public DoFunction SkillFunction { get; set; }
        public string SkillFunctionName { get; set; }
        public SpellFunction SpellFunction { get; set; }


        public int[] skill_level { get; set; }
        public int[] skill_adept { get; set; }
        public int[] RaceLevel { get; set; }
        public int[] RaceAdept { get; set; }
        public int MinimumPosition { get; set; }
        public int slot { get; set; }
        public int MinimumMana { get; set; }
        public int Beats { get; set; }
        public string noun_damage { get; set; }
        public string WearOffMessage { get; set; }
        public int guild { get; set; }
        public int min_level { get; set; }
        public int range { get; set; }
        public string HitCharacterMessage { get; set; }
        public string HitVictimMessage { get; set; }
        public string HitRoomMessage { get; set; }
        public string hit_dest { get; set; }
        public string MissCharacterMessage { get; set; }
        public string MissVictimMessage { get; set; }
        public string MissRoomMessage { get; set; }
        public string die_char { get; set; }
        public string die_vict { get; set; }
        public string die_room { get; set; }
        public string ImmuneCharacterMessage { get; set; }
        public string ImmuneVictimMessage { get; set; }
        public string ImmuneRoomMessage { get; set; }
        public string dice { get; set; }
        public int value { get; set; }
        public int spell_sector { get; set; }
        public SaveVsTypes SaveVs { get; set; }
        public char difficulty { get; set; }
        public List<smaug_affect> affects { get; set; }
        public string Components { get; set; }
        public string teachers { get; set; }
        public char participants { get; set; }
        public UseHistory UseHistory { get; set; }

        public SkillData()
        {
            skill_level = new int[Program.MAX_CLASS];
            skill_adept = new int[Program.MAX_CLASS];
            RaceLevel = new int[Program.MAX_RACE];
            RaceAdept = new int[Program.MAX_RACE];
        }

        public static int Compare(SkillData sk1, SkillData sk2)
        {
            if (sk1 == null && sk2 != null)
                return 1;
            if (sk1 != null && sk2 == null)
                return -1;
            if (sk1 == null)
                return 0;

            return (int) sk1.Name.CaseCompare(sk2.Name);
        }

        public static int CompareByType(SkillData sk1, SkillData sk2)
        {
            if (sk1 == null && sk2 != null)
                return 1;
            if (sk1 != null && sk2 == null)
                return -1;
            if (sk1 == null)
                return 0;

            if ((int) sk1.Type < (int) sk2.Type)
                return -1;
            if ((int) sk1.Type > (int) sk2.Type)
                return 1;

            return (int) sk1.Name.CaseCompare(sk2.Name);
        }

    }
}
