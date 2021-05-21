using Realm.Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SmaugCS.Data
{
    [XmlRoot("Race")]
    public class RaceData : Entity
    {
        [XmlElement("RaceType")]
        public RaceTypes Type { get; set; }

        public int AffectedBy { get; set; }

        [XmlElement]
        public int StrengthBonus { get; set; }

        [XmlElement]
        public int DexterityBonus { get; set; }

        [XmlElement]
        public int WisdomBonus { get; set; }

        [XmlElement]
        public int IntelligenceBonus { get; set; }

        [XmlElement]
        public int ConstitutionBonus { get; set; }

        [XmlElement]
        public int CharismaBonus { get; set; }

        [XmlElement]
        public int LuckBonus { get; set; }

        public int Health { get; set; }
        public int Mana { get; set; }
        public int Resistance { get; set; }
        public int Susceptibility { get; set; }
        public int ClassRestriction { get; set; }
        public int Language { get; set; }

        [XmlElement]
        public int ArmorClassBonus { get; set; }

        public int Alignment { get; set; }
        public ExtendedBitvector Attacks { get; set; }
        public ExtendedBitvector Defenses { get; set; }
        public int MinimumAlignment { get; set; }
        public int MaximumAlignment { get; set; }
        public int ExperienceMultiplier { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int HungerMod { get; set; }
        public int ThirstMod { get; set; }
        public SavingThrowData SavingThrows { get; set; }
        public ICollection<string> WhereNames { get; private set; }
        public int HitrollBonus { get; set; }

        [XmlElement]
        public int ManaRegenRate { get; set; }

        [XmlElement]
        public int HealthRegenRate { get; set; }

        public int RaceRecallRoom { get; set; }

        public RaceData(long id, string name) : base(id, name)
        {
            WhereNames = new List<string>();
            SavingThrows = new SavingThrowData();
        }

        public void AddWhereName(string name)
        {
            WhereNames.Add(name);
        }

        public void AddAffectedBy(string name)
        {
            AffectedByTypes type = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumByName<AffectedByTypes>(name);
            AffectedBy = AffectedBy.SetBit(type);
        }
    }
}
