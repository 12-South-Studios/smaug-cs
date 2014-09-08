using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;

// ReSharper disable once CheckNamespace
namespace SmaugCS.Data
{
    public class ObjectTemplate : Template, IHasExtraFlags, IHasExtraDescriptions
    {
        public List<ExtraDescriptionData> ExtraDescriptions { get; set; }
        public List<AffectData> Affects { get; set; }
        public int ExtraFlags { get; set; }
        public string Flags { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Action { get; set; }
        public dynamic Values { get; set; }
        public int serial { get; set; }
        public int Cost { get; set; }
        public int Rent { get; set; }
        public int magic_flags { get; set; }
        public string WearFlags { get; set; }
        public int count { get; set; }
        public int Weight { get; set; }
        public int Layers { get; set; }
        public int Level { get; set; }
        public ItemTypes Type { get; set; }
        public List<string> Spells { get; set; }

        public ObjectTemplate(long id, string name)
            : base(id, name)
        {
            Values = new ExpandoObject();
            ExtraDescriptions = new List<ExtraDescriptionData>();
            Affects = new List<AffectData>();
            Spells = new List<string>();

            ShortDescription = string.Format("A newly created {0}", name);
            Description = string.Format("Somebody dropped a newly created {0} here.", name);
            Type = ItemTypes.Trash;
            Weight = 1;
        }

        public void SetType(string type)
        {
            Type = Realm.Library.Common.EnumerationExtensions.GetEnumByName<ItemTypes>(type);
        }
        public void SetValues(int v1, int v2, int v3, int v4, int v5, int v6)
        {
            Values.Val0 = v1;
            Values.Val1 = v2;
            Values.Val2 = v3;
            Values.Val3 = v4;
            Values.Val4 = v5;
            Values.Val5 = v6;

            if (Type == ItemTypes.Armor)
            {
                Values.CurrentAC = v1;
                Values.OriginalAC = v2;
            }
            else if (Type == ItemTypes.Container)
            {
                Values.Capacity = v1;
                Values.Flags = v2;
                Values.KeyID = v3;
                Values.Condition = v4;
            }
            else if (Type == ItemTypes.DrinkContainer)
            {
                Values.Capacity = v1;
                Values.Quantity = v2;
                Values.LiquidID = v3;
                Values.Poison = v4;
            }
            else if (Type == ItemTypes.Food)
            {
                Values.FoodValue = v1;
                Values.Condition = v2;
                Values.Poison = v3;
            }
            else if (Type == ItemTypes.Herb)
            {
                Values.Charges = v1;
                Values.HerbID = v2;
            }
            else if (Type == ItemTypes.Key)
            {
                Values.LockID = v1;
            }
            else if (Type == ItemTypes.KeyRing)
            {
                Values.Capacity = v1;

                Values.Condition = v4;
            }
            else if (Type == ItemTypes.Lever)
            {
                Values.Flags = v1;
                Values.SkillID = v2;
                Values.ID = v3;
                Values.Val = v4;
            }
            else if (Type == ItemTypes.Light)
            {
                Values.CurrentAC = v1;
                Values.Lightable = v2;
                Values.HoursLeft = v3;
                Values.Flags = v4;
            }
            else if (Type == ItemTypes.Missile)
            {
                Values.Condition = v1;
                Values.DamageBonus = v2;
                Values.WeaponType = v3;
                Values.Range = v4;
            }
            else if (Type == ItemTypes.Money)
            {
                Values.NumberOfCoins = v1;
                Values.CoinType = v2;
            }
            else if (Type == ItemTypes.Pill)
            {
                Values.SpellLevel = v1;
                Values.Skill1ID = v2;
                Values.Skill2ID = v3;
                Values.Skill3ID = v4;
                Values.FoodValue = v5;
            }
            else if (Type == ItemTypes.Pipe)
            {
                Values.Capacity = v1;
                Values.NumberOfDraws = v2;
                Values.HerbSkillID = v3;
                Values.Flags = v4;
            }
            else if (Type == ItemTypes.Potion)
            {
                Values.SpellLevel = v1;
                Values.Skill1ID = v2;
                Values.Skill2ID = v3;
                Values.Skill3ID = v4;
            }
            else if (Type == ItemTypes.Quiver)
            {
                Values.Capacity = v1;
                Values.Flags = v2;
                Values.KeyID = v3;
                Values.Condition = v4;
            }
            else if (Type == ItemTypes.Salve)
            {
                Values.SpellLevel = v1;
                Values.Charges = v2;
                Values.MaxCharges = v3;
                Values.Delay = v4;
                Values.Skill1ID = v5;
                Values.Skill2ID = v6;
            }
            else if (Type == ItemTypes.Scroll)
            {
                Values.SpellLevel = v1;
                Values.Skill1ID = v2;
                Values.Skill2ID = v3;
                Values.Skill3ID = v4;
            }
            else if (Type == ItemTypes.Staff)
            {
                Values.SpellLevel = v1;
                Values.MaxCharges = v2;
                Values.Charges = v3;
                Values.SkillID = v4;
            }
            else if (Type == ItemTypes.Switch)
            {
                Values.Flags = v1;
                Values.SkillID = v2;
                Values.ID = v3;
                Values.Val = v4;
            }
            else if (Type == ItemTypes.Trap)
            {
                Values.Charges = v1;
                Values.Type = v2;
                Values.Level = v3;
                Values.Flags = v4;
            }
            else if (Type == ItemTypes.Treasure)
            {
                Values.Type = v1;
                Values.Condition = v2;
            }
            else if (Type == ItemTypes.Wand)
            {
                Values.Level = v1;
                Values.MaxCharges = v2;
                Values.Charges = v3;
                Values.SkillID = v4;
            }
            else if (Type == ItemTypes.Weapon)
            {
                Values.Condition = v1;
                Values.NumberOfDice = v2;
                Values.SizeOfDice = v3;
                Values.WeaponType = v4;
            }
            else if (Type == ItemTypes.Furniture)
            {
                Values.FurniturePositionFlags = v1;
                Values.MaxPeople = v2;
                Values.MaxWeight = v3;
            }
            else
                throw new InvalidDataException(string.Format("SetValues called with an invalid item type {0}", Type));
        }

        public void AddSpell(string spell)
        {
            if (!Spells.Contains(spell.ToLower()))
                Spells.Add(spell.ToLower());
        }

        public void AddAffect(int type, int duration, int modifier, int location, int flags)
        {
            AffectData newAffect = new AffectData
            {
                Type = Realm.Library.Common.EnumerationExtensions.GetEnum<AffectedByTypes>(type),
                Duration = duration,
                Modifier = modifier,
                Location = Realm.Library.Common.EnumerationExtensions.GetEnum<ApplyTypes>(location),
                Flags = flags
            };
            Affects.Add(newAffect);
        }

        public void SetStats(int weight, int cost, int rent, int level, int layers)
        {
            Weight = weight;
            Cost = cost;
            Rent = rent;
            Level = level;
            Layers = layers;
        }

        #region IHasExtraDescriptions Implementation
        public void AddExtraDescription(string keywords, string description)
        {
            string[] words = keywords.Split(new[] { ' ' });
            foreach (string word in words)
            {
                ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(word));
                if (foundEd == null)
                {
                    foundEd = new ExtraDescriptionData
                    {
                        Keyword = word, 
                        Description = description
                    };
                    ExtraDescriptions.Add(foundEd);
                }
            }
        }

        public bool DeleteExtraDescription(string keyword)
        {
            ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keyword));
            if (foundEd == null)
                return false;

            ExtraDescriptions.Remove(foundEd);
            return true;
        }

        public ExtraDescriptionData GetExtraDescription(string keyword)
        {
            return ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keyword));
        }

        #endregion

        /*public void SaveFUSS(TextWriterProxy proxy, bool install)
        {
            if (install)
                ExtraFlags.RemoveBit((int)ItemExtraFlags.Prototype);

            proxy.Write("#OBJECT\n");
            proxy.Write("Vnum     {0}\n", Vnum);
            proxy.Write("Keywords {0}~\n", Name);
            proxy.Write("Type     {0}~\n", BuilderConstants.o_types[(int)Type]);
            proxy.Write("Short    {0}~\n", ShortDescription);
            if (!Description.IsNullOrEmpty())
                proxy.Write("Long     {0}~\n", Description);
            if (!Action.IsNullOrEmpty())
                proxy.Write("Action   {0}~\n", Action);
            if (!ExtraFlags.IsEmpty())
                proxy.Write("Flags    {0}~\n", ExtraFlags.GetFlagString(BuilderConstants.o_flags));
            if (WearFlags > 0)
                proxy.Write("WFlags   {0}~\n", WearFlags.GetFlagString(BuilderConstants.w_flags));

            int[] values = new int[6];
            PopulateValuesByItemType(values);

            proxy.Write(string.Format("Values   {0} {1} {2} {3} {4} {5}\n", values[0], values[1], values[2], values[3],
                                      values[4], values[5]));
            proxy.Write(string.Format("Stats    {0} {1} {2} {3} {4}\n", Weight, Cost, Rent > 0 ? Rent : (int)Cost / 10,
                                      Level, Layers));

            foreach (AffectData af in Affects)
                af.SaveFUSS(proxy);

            SaveObjectDataByType(proxy);

            foreach (ExtraDescriptionData ed in ExtraDescriptions)
                ed.SaveFUSS(proxy);

            foreach (MudProgData mp in MudProgs)
                mp.Save(proxy);

            proxy.Write("#ENDOBJECT\n\n");
        }*/

        /* private void PopulateValuesByItemType(IList<int> values)
         {
             values[0] = Value[0];
             values[1] = Value[1];
             values[2] = Value[2];
             values[3] = Value[3];
             values[4] = Value[4];
             values[5] = Value[5];

             switch (Type)
             {
                 case ItemTypes.Pill:
                 case ItemTypes.Potion:
                 case ItemTypes.Scroll:
                     if (Macros.IS_VALID_SN(values[0]))
                         values[1] = Program.HAS_SPELL_INDEX;
                     if (Macros.IS_VALID_SN(values[2]))
                         values[2] = Program.HAS_SPELL_INDEX;
                     if (Macros.IS_VALID_SN(values[3]))
                         values[3] = Program.HAS_SPELL_INDEX;
                     break;
                 case ItemTypes.Staff:
                 case ItemTypes.Wand:
                     if (Macros.IS_VALID_SN(values[3]))
                         values[3] = Program.HAS_SPELL_INDEX;
                     break;
                 case ItemTypes.Salve:
                     if (Macros.IS_VALID_SN(values[4]))
                         values[4] = Program.HAS_SPELL_INDEX;
                     if (Macros.IS_VALID_SN(values[5]))
                         values[5] = Program.HAS_SPELL_INDEX;
                     break;
             }
         }

         private void SaveObjectDataByType(TextWriterProxy proxy)
         {
             switch (Type)
             {
                 case ItemTypes.Pill:
                 case ItemTypes.Potion:
                 case ItemTypes.Scroll:
                     proxy.Write("Spells   '{0}' '{1}' '{2}'\n",
                                 Macros.IS_VALID_SN(Value[1]) ? db.GetSkill(Value[1]).Name : "NONE",
                                 Macros.IS_VALID_SN(Value[2]) ? db.GetSkill(Value[2]).Name : "NONE",
                                 Macros.IS_VALID_SN(Value[3]) ? db.GetSkill(Value[3]).Name : "NONE");
                     break;
                 case ItemTypes.Staff:
                 case ItemTypes.Wand:
                     proxy.Write("Spells   '{0}'\n", Macros.IS_VALID_SN(Value[3]) ? db.GetSkill(Value[3]).Name : "NONE");
                     break;
                 case ItemTypes.Salve:
                     proxy.Write("Spells   '{0}' '{1}'\n",
                                 Macros.IS_VALID_SN(Value[4]) ? db.GetSkill(Value[4]).Name : "NONE",
                                 Macros.IS_VALID_SN(Value[5]) ? db.GetSkill(Value[5]).Name : "NONE");
                     break;
             }
         }*/
    }
}
