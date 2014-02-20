using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.Extensions
{
    public static class ObjectInstanceExtensions
    {
        public static int GetResistance(this ObjectInstance obj)
        {
            int resist = SmaugRandom.Fuzzy(Program.MAX_ITEM_IMPACT);

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Magical))
                resist += SmaugRandom.Fuzzy(12);

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Metallic))
                resist += SmaugRandom.Fuzzy(5);

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Organic))
                resist -= SmaugRandom.Fuzzy(5);

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Blessed))
                resist += SmaugRandom.Fuzzy(5);

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Inventory))
                resist += 20;

            resist += (obj.Level / 10) - 2;

            if (obj.ItemType == ItemTypes.Armor
                || obj.ItemType == ItemTypes.Weapon)
                resist += (obj.Value[0] / 2) - 2;

            return resist.GetNumberThatIsBetween(10, 99);
        }

        public static bool InMagicContainer(this ObjectInstance obj)
        {
            if (obj.ItemType == ItemTypes.Container
                && Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Magical))
                return true;
            return obj.InObject != null && obj.InObject.InMagicContainer();
        }

        public static int GetObjectWeight(this ObjectInstance obj)
        {
            int weight = obj.Count * obj.Weight;
            if (obj.ItemType != ItemTypes.Container
                || !Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Magical))
            {
                weight += obj.Contents.Sum(o => o.GetObjectWeight());
            }

            return weight;
        }

        public static int GetRealObjectWeight(this ObjectInstance obj)
        {
            int weight = obj.Count * obj.Weight;

            weight += obj.Contents.Sum(o => o.GetRealObjectWeight());

            return weight;
        }

        public static ObjectInstance ToCharacter(this ObjectInstance obj, CharacterInstance ch)
        {
            int oweight = obj.GetObjectWeight();
            int onum = obj.GetObjectNumber();
            int wearLoc = (int)obj.WearLocation;
            ExtendedBitvector extraFlags = obj.ExtraFlags;

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Prototype)
                && !ch.IsImmortal()
                && (!ch.IsNpc()
                    || !ch.Act.IsSet((int)ActFlags.Prototype)))
                return ch.CurrentRoom.ToRoom(obj);

            bool skipGroup = false;

            if (handler.LoadingCharacter == ch)
            {
                for (int i = 0; i < GameConstants.MaximumWearLocations; i++)
                {
                    for (int j = 0; j < GameConstants.MaximumWearLayers; j++)
                    {
                        if (ch.IsNpc())
                        {
                            if (obj.MobEq[i, j] == obj)
                            {
                                skipGroup = true;
                                break;
                            }
                        }
                        else
                        {
                            if (obj.PlayerEq[i, j] == obj)
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
                    groupObj = handler.group_object(carriedObj, obj);
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
                    ch.Carrying.Add(obj);
                    obj.CarriedBy = ch;
                    obj.InRoom = null;
                    obj.InObject = null;
                }
                else
                {
                    ObjectInstance foundObj = null;
                    foreach (ObjectInstance carriedObj in ch.Carrying)
                    {
                        if (obj.Level > carriedObj.Level)
                        {
                            ch.Carrying.Insert(0, carriedObj);
                            foundObj = carriedObj;
                            break;
                        }
                        if (obj.Level == carriedObj.Level
                            && obj.ShortDescription.Equals(carriedObj.ShortDescription))
                        {
                            ch.Carrying.Insert(0, carriedObj);
                            foundObj = carriedObj;
                            break;
                        }
                    }

                    if (foundObj == null)
                        ch.Carrying.Add(obj);

                    obj.CarriedBy = ch;
                    obj.InRoom = null;
                    obj.InObject = null;
                }
            }

            if (wearLoc == (int)WearLocations.None)
            {
                ch.CarryNumber += onum;
                ch.CarryWeight += oweight;
            }
            else if (!extraFlags.IsSet((int)ItemExtraFlags.Magical))
                ch.CarryWeight += oweight;

            return groupObj ?? obj;
        }

        public static void FromCharacter(this ObjectInstance obj)
        {
            CharacterInstance ch = obj.CarriedBy;
            if (ch == null)
            {
                LogManager.Instance.Bug("%s: null ch", "obj_from_char");
                return;
            }

            if (obj.WearLocation != WearLocations.None)
                ch.Unequip(obj);

            if (obj.CarriedBy == null)
                return;

            ch.Carrying.Remove(obj);

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Covering)
                && obj.Contents != null && obj.Contents.Count > 0)
                handler.empty_obj(obj, null, null);

            obj.InRoom = null;
            obj.CarriedBy = null;
            ch.CarryNumber -= obj.GetObjectNumber();
            ch.CarryWeight -= obj.GetObjectWeight();
        }


        public static ObjectInstance ToObject(this ObjectInstance o, ObjectInstance obj)
        {
            if (obj == o)
            {
                LogManager.Instance.Bug("Trying to put object inside itself: vnum {0}", obj.ObjectIndex.Vnum);
                return obj;
            }

            CharacterInstance who = o.GetCarriedBy();

            if (!o.InMagicContainer() && who != null)
                who.CarryWeight += obj.GetObjectWeight();

            foreach (ObjectInstance otmp in o.Contents)
            {
                ObjectInstance oret = handler.group_object(otmp, obj);
                if (oret == otmp)
                    return oret;
            }

            o.Contents.Add(obj);
            obj.InObject = o;
            obj.InRoom = null;
            obj.CarriedBy = null;
            return obj;
        }

        public static void FromObject(this ObjectInstance o, ObjectInstance obj)
        {
            if (obj.InObject != o)
            {
                LogManager.Instance.Bug("null objectFrom");
                return;
            }

            bool magic = o.InMagicContainer();

            o.Contents.Remove(obj);

            if (Macros.IS_OBJ_STAT(obj, (int)ItemExtraFlags.Covering)
                && obj.Contents != null)
                handler.empty_obj(obj, o, null);

            obj.InObject = null;
            obj.InRoom = null;
            obj.CarriedBy = null;

            if (!magic)
            {
                ObjectInstance tmp = o;
                do
                {
                    tmp = o.InObject;
                    if (tmp.CarriedBy != null)
                        tmp.CarriedBy.CarryWeight -= obj.GetObjectWeight();
                } while (tmp != null);
            }
        }
    }
}
