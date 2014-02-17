﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    [XmlRoot("Class")]
    public class ClassData
    {
        [XmlElement("ClassType")]
        public ClassTypes Type { get; set; }

        [XmlElement]
        public string Name { get; set; }

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

        public int MinimumHealthGain { get; set; }
        public int MaximumHealthGain { get; set; }
        public bool UseMana { get; set; }
        public int BaseExperience { get; set; }

        public int Resistance { get; set; }
        public int Susceptibility { get; set; }
        public int Immunity { get; set; }

        public List<ClassSkillAdeptData> Skills { get; set; }
        public Dictionary<int, Tuple<string, string>> Titles { get; set; }
 
        public ClassData()
        {
            Skills = new List<ClassSkillAdeptData>();
            Titles = new Dictionary<int, Tuple<string, string>>();
        }

        public void SetPrimaryAttribute(string name)
        {
            PrimaryAttribute = EnumerationExtensions.GetEnumByName<StatisticTypes>(name);
        }

        public void SetSecondaryAttribute(string name)
        {
            SecondaryAttribute = EnumerationExtensions.GetEnumByName<StatisticTypes>(name);
        }

        public void SetDeficientAttribute(string name)
        {
            DeficientAttribute = EnumerationExtensions.GetEnumByName<StatisticTypes>(name);
        }

        public void AddSkill(string name, int level, int adept)
        {
            if (Skills.Any(x => x.Skill.EqualsIgnoreCase(name)))
                return;

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
            Type = EnumerationExtensions.GetEnumByName<ClassTypes>(type);
        }
    }
}