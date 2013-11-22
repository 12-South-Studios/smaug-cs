using System.Collections.Generic;
using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    [XmlRoot("Liquid")]
    public class LiquidData
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string ShortDescription { get; set; }

        [XmlElement]
        public string Color { get; set; }

        [XmlElement("ID")]
        public int Vnum { get; set; }

        [XmlElement("LiquidType")]
        public LiquidTypes Type { get; set; }

        public Dictionary<ConditionTypes, int> Mods { get; set; }

        public LiquidData()
        {
            Mods = new Dictionary<ConditionTypes, int>();
        }

        public void SetType(string type)
        {
            Type = EnumerationExtensions.GetEnumIgnoreCase<LiquidTypes>(type);
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
