﻿using Realm.Library.Common.Objects;
using SmaugCS.Constants.Enums;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SmaugCS.Data
{
    [XmlRoot("Liquid")]
    public class LiquidData : Entity
    {
        public LiquidData(long id, string name) : base(id, name)
        {
            Mods = new Dictionary<ConditionTypes, int>();
        }

        [XmlElement]
        public string ShortDescription { get; set; }

        [XmlElement]
        public string Color { get; set; }

        [XmlElement("ID")]
        public int Vnum => (int)ID;

        [XmlElement("LiquidType")]
        public LiquidTypes Type { get; set; }

        public Dictionary<ConditionTypes, int> Mods { get; private set; }


        public void SetType(string type)
        {
            Type = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumIgnoreCase<LiquidTypes>(type);
        }

        public void AddMods(int mod1, int mod2, int mod3, int mod4)
        {
            Mods[ConditionTypes.Drunk] = mod1;
            Mods[ConditionTypes.Full] = mod2;
            Mods[ConditionTypes.Thirsty] = mod3;
            Mods[ConditionTypes.Bloodthirsty] = mod4;
        }

        public int GetMod(ConditionTypes type)
        {
            return Mods.ContainsKey(type) ? Mods[type] : -1;
        }
    }
}
