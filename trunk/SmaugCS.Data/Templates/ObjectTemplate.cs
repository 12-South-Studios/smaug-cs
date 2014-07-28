using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;


// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public ObjectTemplate(long id, string name)
            : base(id, name)
        {
            Value = new int[6];
            ExtraDescriptions = new List<ExtraDescriptionData>();
            Affects = new List<AffectData>();
            Spells = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        /// <param name="v5"></param>
        /// <param name="v6"></param>
        public void SetValues(int v1, int v2, int v3, int v4, int v5, int v6)
        {
            Value[0] = v1;
            Value[1] = v2;
            Value[2] = v3;
            Value[3] = v4;
            Value[4] = v5;
            Value[5] = v6;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spell"></param>
        public void AddSpell(string spell)
        {
            if (!Spells.Contains(spell.ToLower()))
                Spells.Add(spell.ToLower());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="duration"></param>
        /// <param name="modifier"></param>
        /// <param name="location"></param>
        /// <param name="bitvector"></param>
        public void AddAffect(int type, int duration, int modifier, int location, string bitvector)
        {
            AffectData newAffect = new AffectData
                {
                    Type = Realm.Library.Common.EnumerationExtensions.GetEnum<AffectedByTypes>(type),
                    Duration = duration,
                    Modifier = modifier,
                    Location = Realm.Library.Common.EnumerationExtensions.GetEnum<ApplyTypes>(location),
                    BitVector = bitvector.ToBitvector()
                };
            Affects.Add(newAffect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="cost"></param>
        /// <param name="rent"></param>
        /// <param name="level"></param>
        /// <param name="layers"></param>
        public void SetStats(int weight, int cost, int rent, int level, int layers)
        {
            Weight = weight;
            Cost = cost;
            Rent = rent;
            Level = level;
            Layers = layers;
        }

        #region IHasExtraDescriptions Implementation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="description"></param>
        public void AddExtraDescription(string keywords, string description)
        {
            string[] words = keywords.Split(new[] { ' ' });
            foreach (string word in words)
            {
                ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(word));
                if (foundEd == null)
                {
                    foundEd = new ExtraDescriptionData { Keyword = word, Description = description };
                    ExtraDescriptions.Add(foundEd);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public bool DeleteExtraDescription(string keyword)
        {
            ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keyword));
            if (foundEd == null)
                return false;

            ExtraDescriptions.Remove(foundEd);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
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
