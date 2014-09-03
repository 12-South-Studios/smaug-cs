using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    public class SkillData : Entity
    {
        public SkillTypes Type { get; set; }
        public int Info { get; set; }
        public int Flags { get; set; }
        public TargetTypes Target { get; set; }

        public string SpellFunctionName { get; set; }
        public DoFunction SkillFunction { get; set; }
        public string SkillFunctionName { get; set; }
        public SpellFunction SpellFunction { get; set; }

        public IEnumerable<int> SkillLevels { get; set; }
        public IEnumerable<SkillMasteryData> SkillMasteries { get; set; }
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
        public string HitDestinationMessage { get; set; }
        public string MissCharacterMessage { get; set; }
        public string MissVictimMessage { get; set; }
        public string MissRoomMessage { get; set; }
        public string DieCharacterMessage { get; set; }
        public string DieVictimMessage { get; set; }
        public string DieRoomMessage { get; set; }
        public string ImmuneCharacterMessage { get; set; }
        public string ImmuneVictimMessage { get; set; }
        public string ImmuneRoomMessage { get; set; }
        public string Dice { get; set; }
        public int value { get; set; }
        public int spell_sector { get; set; }
        public SaveVsTypes SaveVs { get; set; }
        public char difficulty { get; set; }
        public List<SmaugAffect> Affects { get; set; }
        public List<SpellComponent> Components { get; set; }
        public List<string> Teachers { get; set; } 
        public char participants { get; set; }
        public UseHistory UseHistory { get; set; }
        public string NounDamage { get; set; }

        public SkillData(long id, string name) : base(id, name)
        {
            Affects = new List<SmaugAffect>();
            Components = new List<SpellComponent>();
            Teachers = new List<string>();
            SkillMasteries = new List<SkillMasteryData>();
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

        public void AddTeacher(string value)
        {
            if(!Teachers.Contains(value))
                Teachers.Add(value);
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

        public void AddComponent(SpellComponent component)
        {
            Components.Add(component);
        }

        public void SetSaveVsByValue(int value)
        {
            SaveVs = EnumerationExtensions.GetEnum<SaveVsTypes>(value);
        }
    }
}
