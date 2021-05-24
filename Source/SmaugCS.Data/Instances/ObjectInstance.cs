using Realm.Library.Common.Extensions;
using Realm.Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Templates;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Serialization;

namespace SmaugCS.Data.Instances
{
    [XmlRoot("Object")]
    public class ObjectInstance : Instance, IHasExtraFlags, IHasExtraDescriptions
    {
        public new string Name { get; set; }
        public ICollection<ObjectInstance> Contents { get; private set; }
        public ObjectInstance InObject { get; set; }

        private CharacterInstance _carriedBy;
        public CharacterInstance CarriedBy
        {
            get { return InObject != null ? InObject.CarriedBy : _carriedBy; }
            set { _carriedBy = value; }
        }

        public ICollection<ExtraDescriptionData> ExtraDescriptions { get; }
        public RoomTemplate InRoom { get; set; }
        public string Action { get; set; }
        public string Owner { get; set; }
        public ItemTypes ItemType { get; set; }
        public int mpscriptpos { get; set; }
        public ExtendedBitvector ExtraFlags { get; set; }
        public int MagicFlags { get; set; }
        public int WearFlags { get; set; }
        public MudProgActData mpact { get; set; }
        public int mpactnum { get; set; }
        public WearLocations WearLocation { get; set; }
        public int Weight { get; set; }
        public int Cost { get; set; }
        public List<int> Value { get; set; }
        public dynamic Values { get; set; }
        public int Count { get; set; }

        public ObjectInstance[,] PlayerEq { get; private set; }
        public ObjectInstance[,] MobEq { get; private set; }

        public ObjectInstance(long id, string name, int maxWear, int maxLayers)
            : base(id, name)
        {
            Values = new ExpandoObject();
            ExtraDescriptions = new List<ExtraDescriptionData>();
            Contents = new List<ObjectInstance>();

            PlayerEq = new ObjectInstance[maxWear, maxLayers];
            MobEq = new ObjectInstance[maxWear, maxLayers];
            Value = new List<int>();
        }

        public ObjectTemplate ObjectIndex => Parent.CastAs<ObjectTemplate>();

        public int ApplyArmorClass
        {
            get
            {
                if (ItemType != ItemTypes.Armor)
                    return 0;

                switch (WearLocation)
                {
                    case WearLocations.Body:
                        return 3 * Values.CurrentAC;
                    case WearLocations.Head:
                    case WearLocations.Legs:
                    case WearLocations.About:
                        return 2 * Values.CurrentAC;
                    default:
                        return Values.CurrentAC;
                }
            }
        }

        public int ObjectNumber => Count;

        public int HitRoll
        {
            get
            {
                return ObjectIndex.Affects.Where(paf => paf.Location == ApplyTypes.HitRoll).Sum(paf => paf.Modifier) +
                       Affects.Where(paf => paf.Location == ApplyTypes.HitRoll).Sum(paf => paf.Modifier);
            }
        }

        #region IHasExtraDescriptions Implementation
        public void AddExtraDescription(string keywords, string description)
        {
            string[] words = keywords.Split(' ');
            foreach (string word in words)
            {
                ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(word));
                if (foundEd == null)
                {
                    foundEd = new ExtraDescriptionData
                    {
                        Keyword = keywords,
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
            => ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keyword));

        #endregion
    }
}
