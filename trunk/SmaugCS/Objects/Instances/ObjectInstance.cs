using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Interfaces;
using SmaugCS.Managers;


// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
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
        public string ActionDescription { get; set; }
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
        public int room_vnum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maxWear"></param>
        /// <param name="maxLayers"></param>
        public ObjectInstance(int id, int maxWear, int maxLayers) : base(id)
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

        public int GetResistance()
        {
            int resist = SmaugRandom.Fuzzy(Program.MAX_ITEM_IMPACT);

            if (Macros.IS_OBJ_STAT(this, (int)ItemExtraFlags.Magical))
                resist += SmaugRandom.Fuzzy(12);

            if (Macros.IS_OBJ_STAT(this, (int)ItemExtraFlags.Metallic))
                resist += SmaugRandom.Fuzzy(5);

            if (Macros.IS_OBJ_STAT(this, (int)ItemExtraFlags.Organic))
                resist -= SmaugRandom.Fuzzy(5);

            if (Macros.IS_OBJ_STAT(this, (int)ItemExtraFlags.Blessed))
                resist += SmaugRandom.Fuzzy(5);

            if (Macros.IS_OBJ_STAT(this, (int)ItemExtraFlags.Inventory))
                resist += 20;

            resist += (Level / 10) - 2;

            if (ItemType == ItemTypes.Armor
                || ItemType == ItemTypes.Weapon)
                resist += (Value[0] / 2) - 2;

            return Check.Range(10, resist, 99);
        }

        public int GetObjectNumber()
        {
            return Count;
        }

        public bool InMagicContainer()
        {
            if (ItemType == ItemTypes.Container
                && Macros.IS_OBJ_STAT(this, (int)ItemExtraFlags.Magical))
                return true;
            return InObject != null && InObject.InMagicContainer();
        }

        public int GetObjectWeight()
        {
            int weight = Count * Weight;
            if (ItemType != ItemTypes.Container
                || !Macros.IS_OBJ_STAT(this, (int)ItemExtraFlags.Magical))
            {
                weight += Contents.Sum(obj => obj.GetObjectWeight());
            }

            return weight;
        }

        public int GetRealObjectWeight()
        {
            int weight = Count * Weight;

            weight += Contents.Sum(obj => obj.GetRealObjectWeight());

            return weight;
        }

        private static ObjectInstance[,] PlayerEq;
        private static ObjectInstance[,] MobEq;

        public ObjectInstance ToCharacter(CharacterInstance ch)
        {
            int oweight = GetObjectWeight();
            int onum = GetObjectNumber();
            int wearLoc = (int)WearLocation;
            ExtendedBitvector extraFlags = ExtraFlags;

            if (Macros.IS_OBJ_STAT(this, (int)ItemExtraFlags.Prototype)
                && !ch.IsImmortal()
                && (!ch.IsNpc()
                    || !ch.Act.IsSet((int)ActFlags.Prototype)))
                return ch.CurrentRoom.ToRoom(this);

            bool skipGroup = false;

            if (handler.LoadingCharacter == ch)
            {
                for (int i = 0; i < Program.MAX_WEAR; i++)
                {
                    for (int j = 0; j < Program.MAX_LAYERS; j++)
                    {
                        if (ch.IsNpc())
                        {
                            if (MobEq[i, j] == this)
                            {
                                skipGroup = true;
                                break;
                            }
                        }
                        else
                        {
                            if (PlayerEq[i, j] == this)
                            {
                                skipGroup = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (ch.IsNpc() && ch.MobIndex.Shop != null)
                skipGroup = true;

            ObjectInstance groupObj = null;
            bool grouped = false;
            if (!skipGroup)
            {
                foreach (ObjectInstance carriedObj in ch.Carrying)
                {
                    groupObj = handler.group_object(carriedObj, this);
                    if (groupObj == carriedObj)
                    {
                        grouped = true;
                        break;
                    }
                }
            }

            if (!grouped)
            {
                if (!ch.IsNpc() || ch.MobIndex.Shop == null)
                {
                    ch.Carrying.Add(this);
                    CarriedBy = ch;
                    InRoom = null;
                    InObject = null;
                }
                else
                {
                    ObjectInstance foundObj = null;
                    foreach (ObjectInstance carriedObj in ch.Carrying)
                    {
                        if (Level > carriedObj.Level)
                        {
                            ch.Carrying.Insert(0, carriedObj);
                            foundObj = carriedObj;
                            break;
                        }
                        if (Level == carriedObj.Level
                            && ShortDescription.Equals(carriedObj.ShortDescription))
                        {
                            ch.Carrying.Insert(0, carriedObj);
                            foundObj = carriedObj;
                            break;
                        }
                    }

                    if (foundObj == null)
                        ch.Carrying.Add(this);

                    CarriedBy = ch;
                    InRoom = null;
                    InObject = null;
                }
            }

            if (wearLoc == (int)WearLocations.None)
            {
                ch.CarryNumber += onum;
                ch.CarryWeight += oweight;
            }
            else if (!extraFlags.IsSet((int)ItemExtraFlags.Magical))
                ch.CarryWeight += oweight;

            return groupObj ?? this;
        }

        public void FromCharacter()
        {
            CharacterInstance ch = CarriedBy;
            if (ch == null)
            {
                LogManager.Bug("%s: null ch", "obj_from_char");
                return;
            }

            if (WearLocation != WearLocations.None)
                ch.Unequip(this);

            if (CarriedBy == null)
                return;

            ch.Carrying.Remove(this);

            if (Macros.IS_OBJ_STAT(this, (int)ItemExtraFlags.Covering)
                && Contents != null && Contents.Count > 0)
                handler.empty_obj(this, null, null);

            InRoom = null;
            CarriedBy = null;
            ch.CarryNumber -= GetObjectNumber();
            ch.CarryWeight -= GetObjectWeight();
        }

        public CharacterInstance GetCarriedBy()
        {
            return InObject != null ? InObject.CarriedBy : CarriedBy;
        }

        public ObjectInstance ToObject(ObjectInstance obj)
        {
            if (obj == this)
            {
                LogManager.Bug("Trying to put object inside itself: vnum {0}", obj.ObjectIndex.Vnum);
                return obj;
            }

            CharacterInstance who = GetCarriedBy();

            if (!InMagicContainer() && who != null)
                who.CarryWeight += obj.GetObjectWeight();

            foreach (ObjectInstance otmp in Contents)
            {
                ObjectInstance oret = handler.group_object(otmp, obj);
                if (oret == otmp)
                    return oret;
            }

            Contents.Add(obj);
            obj.InObject = this;
            obj.InRoom = null;
            obj.CarriedBy = null;
            return obj;
        }

        public void FromObject(ObjectInstance obj)
        {
            if (obj.InObject != this)
            {
                LogManager.Bug("null objectFrom");
                return;
            }

            bool magic = InMagicContainer();

            Contents.Remove(obj);

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Covering)
                && obj.Contents != null)
                handler.empty_obj(obj, this, null);

            obj.InObject = null;
            obj.InRoom = null;
            obj.CarriedBy = null;

            if (!magic)
            {
                ObjectInstance tmp = this;
                do
                {
                    tmp = InObject;
                    if (tmp.CarriedBy != null)
                        tmp.CarriedBy.CarryWeight -= obj.GetObjectWeight();
                } while (tmp != null);
            }
        }

        public int GetHitRoll()
        {
            return ObjectIndex.Affects.Where(paf => paf.Location == ApplyTypes.HitRoll).Sum(paf => paf.Modifier) +
                   Affects.Where(paf => paf.Location == ApplyTypes.HitRoll).Sum(paf => paf.Modifier);
        }

        #region IHasExtraDescriptions Implementation
        public ExtraDescriptionData Add(string keywords)
        {
            ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.IsEqual(keywords));
            if (foundEd == null)
            {
                foundEd = new ExtraDescriptionData { Keyword = keywords, Description = "" };
                ExtraDescriptions.Add(foundEd);
            }

            return foundEd;
        }

        public bool Delete(string keywords)
        {
            ExtraDescriptionData foundEd = ExtraDescriptions.FirstOrDefault(ed => ed.Keyword.EqualsIgnoreCase(keywords));
            if (foundEd == null)
                return false;

            ExtraDescriptions.Remove(foundEd);
            return true;
        }
        #endregion
    }
}
