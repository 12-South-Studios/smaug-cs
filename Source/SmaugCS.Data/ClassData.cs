using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    [XmlRoot("Class")]
    public class ClassData : Entity
    {
        [XmlElement("ClassType")]
        public ClassTypes Type { get; set; }

        public ExtendedBitvector AffectedBy { get; set; }

        [XmlElement]
        public StatisticTypes PrimaryAttribute { get; set; }

        [XmlElement]
        public StatisticTypes SecondaryAttribute { get; set; }

        [XmlElement]
        public StatisticTypes DeficientAttribute { get; set; }

        public int Weapon { get; set; }
        public int Guild { get; set; }
        public int SkillAdept { get; set; }

        public int ToHitArmorClass0 { get; set; }
        public int ToHitArmorClass32 { get; set; }

        public bool IsSpellcaster { get; set; }

        public int MinimumHealthGain { get; set; }
        public int MaximumHealthGain { get; set; }
        public bool UseMana { get; set; }
        public int BaseExperience { get; set; }

        public int Resistance { get; set; }
        public int Susceptibility { get; set; }
        public int Immunity { get; set; }

        public ICollection<ClassSkillAdeptData> Skills { get; private set; }
        public Dictionary<int, Tuple<string, string>> Titles { get; private set; }
 
        public ClassData(long id, string name) : base(id, name)
        {
            Skills = new List<ClassSkillAdeptData>();
            Titles = new Dictionary<int, Tuple<string, string>>();
        }

        public void SetPrimaryAttribute(string name)
        {
            PrimaryAttribute = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumByName<StatisticTypes>(name);
        }

        public void SetSecondaryAttribute(string name)
        {
            SecondaryAttribute = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumByName<StatisticTypes>(name);
        }

        public void SetDeficientAttribute(string name)
        {
            DeficientAttribute = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumByName<StatisticTypes>(name);
        }

        public void AddSkill(string name, int level, int adept)
        {
            if (Skills.Any(x => x.Skill.EqualsIgnoreCase(name)))
                throw new InvalidDataException($"Skill {name} is already found on Class {Name}");

            ClassSkillAdeptData data = new ClassSkillAdeptData
                {
                    Skill = name,
                    Level = level, 
                    Adept = adept
                };

            Skills.Add(data);
        }

        public void AddTitle(int level, string title1, string title2)
        {
            Titles[level] = new Tuple<string, string>(title1, title2);
        }

        public void SetType(string type)
        {
            Type = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumByName<ClassTypes>(type);
        }
    }
}
