using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;

namespace SmaugCS.Data.Templates
{
    public class ObjectTemplate : Template, IHasExtraFlags, IHasExtraDescriptions
    {
        public ICollection<ExtraDescriptionData> ExtraDescriptions { get; }
        public ICollection<AffectData> Affects { get; }
        public int ExtraFlags { get; set; }
        public string Flags { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Action { get; set; }
        public dynamic Values { get; set; }
        public int Cost { get; set; }
        public int Rent { get; private set; }
        public int MagicFlags { get; set; }
        public string WearFlags { get; set; }
        public int Count { get; set; }
        public int Weight { get; set; }
        public int Layers { get; private set; }
        public int Level { get; private set; }
        public ItemTypes Type { get; set; }
        public ICollection<string> Spells { get; }

        public ObjectTemplate(long id, string name)
            : base(id, name)
        {
            Values = new ExpandoObject();
            ExtraDescriptions = new List<ExtraDescriptionData>();
            Affects = new List<AffectData>();
            Spells = new List<string>();

            ShortDescription = $"A newly created {name}";
            Description = $"Somebody dropped a newly created {name} here.";
            Type = ItemTypes.Trash;
            Weight = 1;
        }

        public void SetType(string type) => Type = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnumByName<ItemTypes>(type);

        [SuppressMessage("Microsoft.Design", "CA1025:ReplaceRepetitiveArgumentsWithParamsArray",
            Justification = "This function is required by LUA and cannot handle lists or parameter arrays")]
        public void SetValues(int v1, int v2, int v3, int v4, int v5, int v6)
        {
            var valuesToSet = new List<int> { v1, v2, v3, v4, v5, v6 };
            ObjectTemplateValueFunctions.SetObjectTemplateValues(this, valuesToSet);
        }

        public void AddSpell(string spell)
        {
            if (!Spells.Contains(spell.ToLower()))
                Spells.Add(spell.ToLower());
        }

        public void AddAffect(long type, int duration, int modifier, int location, int flags)
        {
            if (type < 0) throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(AffectedByTypes));

            AffectData newAffect = new AffectData
            {
                Type = Common.EnumerationExtensions.GetEnum<AffectedByTypes>(type),
                Duration = duration,
                Modifier = modifier,
                Location = Realm.Library.Common.Extensions.EnumerationExtensions.GetEnum<ApplyTypes>(location),
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
            string[] words = keywords.Split(' ');
            foreach (string word in words)
            {
                ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(word));
                if (foundEd != null) continue;

                foundEd = new ExtraDescriptionData
                {
                    Keyword = word,
                    Description = description
                };
                ExtraDescriptions.Add(foundEd);
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
            => ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keyword));

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
