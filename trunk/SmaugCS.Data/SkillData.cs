using System.Collections.Generic;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
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

        private int _minimumPosition;
        public int MinimumPosition
        {
            get { return _minimumPosition; }
            set
            {
                _minimumPosition = value;
                _minimumPosition = GetModifiedPosition();
            }
        }

        public int Slot { get; set; }
        public int MinimumMana { get; set; }
        public int Rounds { get; set; }
        public string DamageMessage { get; set; }
        public string WearOffMessage { get; set; }
        public int guild { get; set; }
        public int MinimumLevel { get; set; }
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
        public List<SmaugAffect> Affects { get; set; }
        public string Components { get; set; }
        public string teachers { get; set; }
        public char participants { get; set; }
        public UseHistory UseHistory { get; set; }

        public SkillData(int maxClasses, int maxRaces)
        {
            skill_level = new int[maxClasses];
            skill_adept = new int[maxClasses];
            RaceLevel = new int[maxRaces];
            RaceAdept = new int[maxRaces];
            Affects = new List<SmaugAffect>();
        }

        public static int Compare(SkillData sk1, SkillData sk2)
        {
            if (sk1 == null && sk2 != null)
                return 1;
            if (sk1 != null && sk2 == null)
                return -1;
            if (sk1 == null)
                return 0;

            return (int)sk1.Name.CaseCompare(sk2.Name);
        }

        public static int CompareByType(SkillData sk1, SkillData sk2)
        {
            if (sk1 == null && sk2 != null)
                return 1;
            if (sk1 != null && sk2 == null)
                return -1;
            if (sk1 == null)
                return 0;

            if ((int)sk1.Type < (int)sk2.Type)
                return -1;
            if ((int)sk1.Type > (int)sk2.Type)
                return 1;

            return (int)sk1.Name.CaseCompare(sk2.Name);
        }

        public void SetTarget(string type)
        {
            Target = EnumerationExtensions.GetEnumIgnoreCase<TargetTypes>(type);
        }

        public void SetTargetByValue(int type)
        {
            Target = EnumerationExtensions.GetEnum<TargetTypes>(type);
        }

        public void AddAffect(SmaugAffect affect)
        {
            Affects.Add(affect);
        }

        public int GetModifiedPosition()
        {
            int originalPosition = MinimumPosition;
            if (originalPosition < 100)
            {
                switch (originalPosition)
                {
                    case 5:
                        originalPosition = 6;
                        break;
                    case 6:
                        originalPosition = 8;
                        break;
                    case 7:
                        originalPosition = 9;
                        break;
                    case 8:
                        originalPosition = 12;
                        break;
                    case 9:
                        originalPosition = 13;
                        break;
                    case 10:
                        originalPosition = 14;
                        break;
                    case 11:
                        originalPosition = 15;
                        break;
                }
            }

            return originalPosition >= 100 ? originalPosition - 100 : originalPosition;
        }

        public void SetFlags(string flags)
        {
            string[] words = flags.Split(new[] {' '});
            foreach (string word in words)
            {
                Flags += (int)EnumerationExtensions.GetEnumIgnoreCase<SkillFlags>(word);
            }
        }
    }
}
