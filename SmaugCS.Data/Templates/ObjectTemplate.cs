using System.Collections.Generic;
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
        public int[] Value { get; set; }
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
            Values = new object();
            Value = new int[6];
            ExtraDescriptions = new List<ExtraDescriptionData>();
            Affects = new List<AffectData>();
            Spells = new List<string>();

            ShortDescription = string.Format("A newly created {0}", name);
            Description = string.Format("Somebody dropped a newly created {0} here.", name);
            Type = ItemTypes.Trash;
            Weight = 1;
        }

        public void SetValues(int v1, int v2, int v3, int v4, int v5, int v6)
        {
            if (Type == ItemTypes.Armor)
            {
                Values.CurrentAC = v1;
                Values.OriginalAC = v2;
            }

            Value[0] = v1;
            Value[1] = v2;
            Value[2] = v3;
            Value[3] = v4;
            Value[4] = v5;
            Value[5] = v6;

            /*Abacus		None
Armor		V0=Current AC, V1=Original AC
Container	V0=Capacity, V1=Flags, V2=Key Vnum, V3=Condition
DrinkCont	V0=Capacity, V1=Quantity, V2=Liquid #, V3=Poison
Food		V0=Food Value, V1=Condition, V3=Poison
Herb		V1=Charges, V2=Herb #
Key			V0=Lock #
KeyRing		V0=Capacity
Lever		V0=LeverFlags, V1=Vnum/SN, V2=Vnum, V3=Vnum/Val
Light		V0=Current AC, V1=Lightable?, V2=Hours Left, V3=Flags**
Missiles	V0=Condition, V2=Damage Bonus, V3=WeaponTYpe, V4=Range
Money		V0=# of Coins, V1=Coin Type
Piece		V0=Prev Vnum, V1=Next Vnum, V2=Final Vnum
Pill		V0=Spell Level, V1=SN #1, V2=SN #2, V3=SN #3, V4=Food Value
Pipe		V0=Capacity, V1=# of Draws, V2=Herb SN, V3=Flags**
Potion		V0=Spell Level, V1=SN #1, V2=SN #2, V3=SN #3
Projectile	None
Puddle		V0=Capacity, V1=Quantity, V2=Liquid #, V3=Poison
Quiver		V0=Capacity, V1=Flgas, V2=Key Vnum, V3=Condition
Salve		V0=Spell Level, V1=Charges, V2=Max Charges, V3=Delay, V4=SN, V5=SN
Scroll		V0=Spell Level, V1=SN #1, V2=SN #2, V3=SN #3
Staff		V0=Spell Level, V1=Max Charges, V2=Charges, V3=SN
Switch		V0=Lever Flags, V1=Vnum/SN, V2=Vnum, V3=Vnum/Val
Trap		V0=Charges, V1=Type, V3=Level, V4=Flags
Treasure	V0=Type, V1=Condition
Wand		V0=Level, V1=Max Charges, V2=Charges, V3=SN
Weapon		V0=Condition, V1=Num Dice, V2=Size Dice, V3=Weapon Type
Furniture	V2=FurniturePositionFlags, V3=Max People, V4=Max Weight*/

        }

        public void AddSpell(string spell)
        {
            if (!Spells.Contains(spell.ToLower()))
                Spells.Add(spell.ToLower());
        }

        public void AddAffect(int type, int duration, int modifier, int location, string bitvector)
        {
            AffectData newAffect = new AffectData();
            newAffect.Type = Realm.Library.Common.EnumerationExtensions.GetEnum<AffectedByTypes>(type);
            newAffect.Duration = duration;
            newAffect.Modifier = modifier;
            newAffect.Location = Realm.Library.Common.EnumerationExtensions.GetEnum<ApplyTypes>(location);
            newAffect.BitVector = bitvector.ToBitvector();
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
