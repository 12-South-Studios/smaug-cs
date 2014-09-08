using System;
using System.IO;
using System.Linq;
using Ninject;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Auction;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;
using SmaugCS.Managers;
using SmaugCS.Objects;
using SmaugCS.Repositories;

namespace SmaugCS.Extensions
{
    public static class ObjectInstanceExtensions
    {
        public static int GetArmorRepairCost(this ObjectInstance obj, int baseCost)
        {
            int cost = baseCost;
            if (obj.Values.CurrentAC >= obj.Values.OriginalAC)
                cost = -2;
            else
                cost *= (obj.Values.OriginalAC - obj.Values.CurrentAC);
            return cost;
        }
        public static int GetWeaponRepairCost(this ObjectInstance obj, int baseCost, int weaponCondition)
        {
            int cost = baseCost;
            if (weaponCondition == obj.Values.Condition)
                cost = -2;
            else
                cost *= (weaponCondition - obj.Values.Condition);
            return cost;
        }
        public static int GetImplementRepairCost(this ObjectInstance obj, int baseCost)
        {
            int cost = baseCost;
            if (obj.Value[2] >= obj.Value[1])
                cost = -2;
            else
                cost *= (obj.Value[1] - obj.Value[2]);
            return cost;
        }

        public static ObjectInstance GroupWith(this ObjectInstance obj1, ObjectInstance obj2)
        {
            if (obj1 == null || obj2 == null)
                return null;
            if (obj1 == obj2)
                return obj1;

            if (obj1.ObjectIndex == obj2.ObjectIndex
                && obj1.Name.EqualsIgnoreCase(obj2.Name)
                && obj1.ShortDescription.EqualsIgnoreCase(obj2.ShortDescription)
                && obj1.Description.EqualsIgnoreCase(obj2.Description)
                && obj1.Owner.EqualsIgnoreCase(obj2.Owner)
                && obj1.ItemType == obj2.ItemType
                //&& obj1.ExtraFlags.SameBits(obj2.ExtraFlags)
                && obj1.MagicFlags == obj2.MagicFlags
                && obj1.WearFlags == obj2.WearFlags
                && obj1.WearLocation == obj2.WearLocation
                && obj1.Weight == obj2.Weight
                && obj1.Cost == obj2.Cost
                && obj1.Level == obj2.Level
                && obj1.Timer == obj2.Timer
                && obj1.Value[0] == obj2.Value[0]
                && obj1.Value[1] == obj2.Value[1]
                && obj1.Value[2] == obj2.Value[2]
                && obj1.Value[3] == obj2.Value[3]
                && obj1.Value[4] == obj2.Value[4]
                && obj1.Value[5] == obj2.Value[5]
                && obj1.ExtraDescriptions.SequenceEqual(obj2.ExtraDescriptions)
                && obj1.Affects.SequenceEqual(obj2.Affects)
                && obj1.Contents.SequenceEqual(obj2.Contents)
                && obj1.Count + obj2.Count > 0)
            {
                obj1.Count += obj2.Count;
                obj1.ObjectIndex.count += obj2.Count;
                obj2.Extract();
                return obj1;
            }
            return obj2;

        }
        public static void Extract(this ObjectInstance obj)
        {
            if (handler.obj_extracted(obj))
                throw new ObjectAlreadyExtractedException("Object {0}", obj.ObjectIndex.ID);

            if (obj.ItemType == ItemTypes.Portal)
                update.remove_portal(obj);

            if (AuctionManager.Instance.Auction.ItemForSale == obj)
                Commands.Objects.Auction.StopAuction(AuctionManager.Instance.Auction.Seller,
                    "Sale of {0} has been stopped by a system action.");

            if (obj.CarriedBy != null)
                obj.FromCharacter();
            else if (obj.InRoom != null)
                obj.InRoom.FromRoom(obj);
            else if (obj.InObject != null)
                obj.InObject.FromObject(obj);

            ObjectInstance objContent = obj.Contents.Last();
            if (objContent != null)
                objContent.Extract();

            obj.Affects.Clear();
            obj.ExtraDescriptions.Clear();

            //trworld_obj_check(obj);

            foreach (RelationData relation in db.RELATIONS
                                                .Where(relation => relation.Types == RelationTypes.OSet_On))
            {
                if (obj == relation.Subject)
                    relation.Actor.CastAs<CharacterInstance>().DestinationBuffer = null;
                else
                    continue;
                db.RELATIONS.Remove(relation);
            }

            DatabaseManager.Instance.OBJECTS.CastAs<Repository<long, ObjectInstance>>().Delete(obj.ID);

            handler.queue_extracted_obj(obj);

            obj.ObjectIndex.count -= obj.Count;
            db.NumberOfObjectsLoaded -= obj.Count;
            --db.PhysicalObjects;

            if (obj == handler.CurrentObject)
            {
                handler.CurrentObjectExtracted = true;
                if (handler.GlobalObjectCode == ReturnTypes.None)
                    handler.GlobalObjectCode = ReturnTypes.ObjectExtracted;
            }
        }

        public static bool IsTrapped(this ObjectInstance obj)
        {
            return obj.Contents.Any(check => check.ItemType == ItemTypes.Trap);
        }

        public static string GetItemTypeName(this ObjectInstance obj)
        {
            return obj.ItemType.GetName().ToLower();
        }

        public static int GetResistance(this ObjectInstance obj)
        {
            int resist = SmaugRandom.Fuzzy(Program.MAX_ITEM_IMPACT);

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Magical))
                resist += SmaugRandom.Fuzzy(12);

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Metallic))
                resist += SmaugRandom.Fuzzy(5);

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Organic))
                resist -= SmaugRandom.Fuzzy(5);

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Blessed))
                resist += SmaugRandom.Fuzzy(5);

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Inventory))
                resist += 20;

            resist += (obj.Level / 10) - 2;

            if (obj.ItemType == ItemTypes.Armor || obj.ItemType == ItemTypes.Weapon)
                resist += (obj.Value[0] / 2) - 2;

            return resist.GetNumberThatIsBetween(10, 99);
        }

        public static bool InMagicContainer(this ObjectInstance obj)
        {
            if (obj.ItemType == ItemTypes.Container && obj.ExtraFlags.IsSet(ItemExtraFlags.Magical))
                return true;
            return obj.InObject != null && obj.InObject.InMagicContainer();
        }

        public static int GetObjectWeight(this ObjectInstance obj)
        {
            int weight = obj.Count * obj.Weight;
            if (obj.ItemType != ItemTypes.Container || !obj.ExtraFlags.IsSet(ItemExtraFlags.Magical))
                weight += obj.Contents.Sum(o => o.GetObjectWeight());
            
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
            int extraFlags = obj.ExtraFlags;

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Prototype) && !ch.IsImmortal() 
                && (!ch.IsNpc() || !ch.Act.IsSet(ActFlags.Prototype)))
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
                    groupObj = carriedObj.GroupWith(obj);
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
            else if (!extraFlags.IsSet(ItemExtraFlags.Magical))
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

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Covering) && obj.Contents != null && obj.Contents.Count > 0)
                obj.Empty();

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
                ObjectInstance oret = otmp.GroupWith(obj);
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
                throw new InvalidDataException(string.Format("Object {0} is not in {1}", obj.ID, o.ID));

            bool magic = o.InMagicContainer();

            o.Contents.Remove(obj);

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Covering) && obj.Contents != null)
                obj.Empty(o);

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

        public static ObjectInstance Clone(this ObjectInstance obj)
        {
            ObjInstanceRepository repo =
                (ObjInstanceRepository) Program.Kernel.Get<IInstanceRepository<ObjectInstance>>();
            return repo.Clone(obj);
        }

        public static void Split(this ObjectInstance obj, int number = 1)
        {
            int count = obj.Count;
            if (count <= number || number == 0)
                return;

            ObjectInstance rest = obj.Clone();
            --obj.ObjectIndex.count;
            rest.Count = obj.Count - number;
            obj.Count = number;

            if (obj.CarriedBy != null)
            {
                obj.CarriedBy.Carrying.Add(rest);
                rest.CarriedBy = obj.CarriedBy;
                rest.InRoom = null;
                rest.InObject = null;
            }
            else if (obj.InRoom != null)
            {
                obj.InRoom.Contents.Add(rest);
                rest.CarriedBy = null;
                rest.InRoom = obj.InRoom;
                rest.InObject = null;
            }
            else if (obj.InObject != null)
            {
                obj.InObject.Contents.Add(rest);
                rest.InObject = obj.InObject;
                rest.InRoom = null;
                rest.CarriedBy = null;
            }
        }

        public static bool Empty(this ObjectInstance obj, ObjectInstance destobj = null, RoomTemplate destroom = null)
        {
            CharacterInstance ch = obj.CarriedBy;

            if (destobj != null)
                return EmptyIntoObject(obj, destobj);

            if (destroom != null)
                return EmptyIntoRoom(ch, obj, destroom);

            if (obj.InObject != null)
                return EmptyIntoObject(obj, obj.InObject);

            if (ch != null)
            {
                bool retVal = false;
                foreach (ObjectInstance cobj in obj.Contents)
                {
                    cobj.FromObject(cobj);
                    cobj.ToCharacter(ch);
                    retVal = true;
                }
                return retVal;
            }

            throw new InvalidOperationException(
                string.Format("Nothing specified to empty the contents of object {0} into", obj.ID));
        }

        private static bool EmptyIntoObject(ObjectInstance obj, ObjectInstance destobj)
        {
            bool retVal = false;
            foreach (ObjectInstance cobj in obj.Contents)
            {
                if (destobj.ItemType == ItemTypes.KeyRing && cobj.ItemType != ItemTypes.Key)
                    continue;
                if (destobj.ItemType == ItemTypes.Quiver && cobj.ItemType != ItemTypes.Projectile)
                    continue;
                if ((destobj.ItemType == ItemTypes.Container
                     || destobj.ItemType == ItemTypes.KeyRing
                     || destobj.ItemType == ItemTypes.Quiver)
                    && (cobj.GetRealObjectWeight() + destobj.GetRealObjectWeight() > destobj.Value[0]))
                    continue;

                cobj.FromObject(cobj);
                destobj.ToObject(cobj);
                retVal = true;
            }
            return retVal;
        }

        private static bool EmptyIntoRoom(CharacterInstance ch, ObjectInstance obj, RoomTemplate destroom)
        {
            bool retVal = false;
            foreach (ObjectInstance cobj in obj.Contents)
            {
                if (ch != null && cobj.ObjectIndex.HasProg(MudProgTypes.Drop) && cobj.Count > 1)
                {
                    cobj.Split();
                    cobj.FromObject(cobj);
                }
                else
                    cobj.FromObject(cobj);

                ObjectInstance tObj = destroom.ToRoom(cobj);

                if (ch != null)
                {
                    mud_prog.oprog_drop_trigger(ch, tObj);
                    if (ch.CharDied())
                        ch = null;
                }
                retVal = true;
            }
            return retVal;
        }
    }
}
