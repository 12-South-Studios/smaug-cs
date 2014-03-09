using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;

using SmaugCS.Data;

// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
{
    [XmlRoot("Object")]
    public class ObjectInstance : Instance, IHasExtraFlags, IHasExtraDescriptions
    {
        public List<ObjectInstance> Contents { get; set; }
        public ObjectInstance InObject { get; set; }
        public CharacterInstance CarriedBy { get; set; }
        public List<ExtraDescriptionData> ExtraDescriptions { get; set; }
        public RoomTemplate InRoom { get; set; }
        public string Action { get; set; }
        public string Owner { get; set; }
        public ItemTypes ItemType { get; set; }
        public int mpscriptpos { get; set; }
        public ExtendedBitvector ExtraFlags { get; set; }
        public int magic_flags { get; set; }
        public int WearFlags { get; set; }
        public MudProgActData mpact { get; set; }
        public int mpactnum { get; set; }
        public WearLocations WearLocation { get; set; }
        public int Weight { get; set; }
        public int Cost { get; set; }
        public int Level { get; set; }
        public int[] Value { get; set; }
        public int Count { get; set; }
        public long room_vnum { get; set; }

        public ObjectInstance[,] PlayerEq { get; set; }
        public ObjectInstance[,] MobEq { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maxWear"></param>
        /// <param name="maxLayers"></param>
        public ObjectInstance(long id, string name, int maxWear, int maxLayers)
            : base(id, name)
        {
            Value = new int[6];
            ExtraDescriptions = new List<ExtraDescriptionData>();
            ExtraFlags = new ExtendedBitvector();
            Contents = new List<ObjectInstance>();

            PlayerEq = new ObjectInstance[maxWear, maxLayers];
            MobEq = new ObjectInstance[maxWear, maxLayers];
        }

        public ObjectTemplate ObjectIndex
        {
            get { return Parent.CastAs<ObjectTemplate>(); }
        }

        public int ApplyArmorClass
        {
            get
            {
                if (ItemType != ItemTypes.Armor)
                    return 0;

                switch (WearLocation)
                {
                    case WearLocations.Body:
                        return 3 * Value[0];
                    case WearLocations.Head:
                    case WearLocations.Legs:
                    case WearLocations.About:
                        return 2 * Value[0];
                    default:
                        return Value[0];
                }
            }
        }

        public int GetObjectNumber()
        {
            return Count;
        }

        public CharacterInstance GetCarriedBy()
        {
            return InObject != null ? InObject.CarriedBy : CarriedBy;
        }

        public int GetHitRoll()
        {
            return ObjectIndex.Affects.Where(paf => paf.Location == ApplyTypes.HitRoll).Sum(paf => paf.Modifier) +
                   Affects.Where(paf => paf.Location == ApplyTypes.HitRoll).Sum(paf => paf.Modifier);
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
                    foundEd = new ExtraDescriptionData { Keyword = keywords, Description = description };
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
    }
}
