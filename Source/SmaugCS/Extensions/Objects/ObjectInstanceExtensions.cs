using System;
using System.IO;
using System.Linq;
using System.Text;
using Ninject;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Auction;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Exceptions;
using SmaugCS.Extensions.Character;
using SmaugCS.Logging;
using SmaugCS.Managers;
using SmaugCS.Repository;

namespace SmaugCS.Extensions.Objects
{
    public static class ObjectInstanceExtensions
    {
        public static string GetFormattedDescription(this ObjectInstance obj, CharacterInstance ch,
            bool isShortDescription, ILookupManager lookupManager = null)
        {
            var lookupMgr = lookupManager ?? LookupManager.Instance;
            var glowsee = IsGlowingOrCanSee(obj, ch);

            var sb = new StringBuilder();

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Invisible))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 0));
            if ((ch.IsAffected(AffectedByTypes.DetectEvil)
                 || ch.CurrentClass == ClassTypes.Paladin)
                && obj.ExtraFlags.IsSet(ItemExtraFlags.Evil))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 1));

            if (ch.CurrentClass == ClassTypes.Paladin)
                GetPaladinDescriptions(obj, sb, lookupMgr);

            if ((ch.IsAffected(AffectedByTypes.DetectMagic)
                 || ch.Act.IsSet(PlayerFlags.HolyLight))
                && obj.ExtraFlags.IsSet(ItemExtraFlags.Magical))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 8));
            if (!glowsee && obj.ExtraFlags.IsSet(ItemExtraFlags.Glow))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 9));
            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Hum))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 10));
            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Hidden))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 11));
            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Buried))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 12));
            if (ch.IsImmortal() && obj.ExtraFlags.IsSet(ItemExtraFlags.Prototype))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 13));
            if ((ch.IsAffected(AffectedByTypes.DetectTraps)
                 || ch.Act.IsSet(PlayerFlags.HolyLight))
                && obj.IsTrapped())
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 14));

            if (isShortDescription)
            {
                if (glowsee && (ch.IsNpc() || !ch.Act.IsSet(PlayerFlags.HolyLight)))
                    sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 15));
                else if (!string.IsNullOrWhiteSpace(obj.ShortDescription))
                    sb.Append(obj.ShortDescription);
            }
            else
            {
                if (glowsee && (ch.IsNpc() || !ch.Act.IsSet(PlayerFlags.HolyLight)))
                    sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 16));
                else if (!string.IsNullOrWhiteSpace(obj.Description))
                    sb.Append(obj.Description);
            }

            return sb.ToString();
        }

        private static void GetPaladinDescriptions(ObjectInstance obj, StringBuilder sb, ILookupManager lookupMgr)
        {
            if (obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 2));
            if (!obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 3));
            if (!obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 4));
            if (obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 5));
            if (obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                && !obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 6));
            if (!obj.ExtraFlags.IsSet(ItemExtraFlags.AntiEvil)
                && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiNeutral)
                && obj.ExtraFlags.IsSet(ItemExtraFlags.AntiGood))
                sb.Append(lookupMgr.GetLookup("ObjectAffectStrings", 7));
        }

        private static bool IsGlowingOrCanSee(ObjectInstance obj, CharacterInstance ch)
        {
            return obj.ExtraFlags.IsSet(ItemExtraFlags.Invisible)
                    && obj.ExtraFlags.IsSet(ItemExtraFlags.Glow)
                    && !ch.IsAffected(AffectedByTypes.TrueSight)
                    && !ch.IsAffected(AffectedByTypes.DetectInvisibility);
        }

        public static ObjectInstance GroupWith(this ObjectInstance obj1, ObjectInstance obj2)
        {
            if (obj1 == null || obj2 == null) return null;
            if (obj1 == obj2) return obj1;

            if (obj1.ObjectIndex == obj2.ObjectIndex
                && obj1.Name.EqualsIgnoreCase(obj2.Name)
                && obj1.ShortDescription.EqualsIgnoreCase(obj2.ShortDescription)
                && obj1.Description.EqualsIgnoreCase(obj2.Description)
                && obj1.Owner.EqualsIgnoreCase(obj2.Owner)
                && obj1.ItemType == obj2.ItemType
                && obj1.ExtraFlags == obj2.ExtraFlags
                && obj1.MagicFlags == obj2.MagicFlags
                && obj1.WearFlags == obj2.WearFlags
                && obj1.WearLocation == obj2.WearLocation
                && obj1.Weight == obj2.Weight
                && obj1.Cost == obj2.Cost
                && obj1.Level == obj2.Level
                && obj1.Timer == obj2.Timer
                && obj1.Value.ToList()[0] == obj2.Value.ToList()[0]
                && obj1.Value.ToList()[1] == obj2.Value.ToList()[1]
                && obj1.Value.ToList()[2] == obj2.Value.ToList()[2]
                && obj1.Value.ToList()[3] == obj2.Value.ToList()[3]
                && obj1.Value.ToList()[4] == obj2.Value.ToList()[4]
                && obj1.Value.ToList()[5] == obj2.Value.ToList()[5]
                && obj1.ExtraDescriptions.SequenceEqual(obj2.ExtraDescriptions)
                && obj1.Affects.SequenceEqual(obj2.Affects)
                && obj1.Contents.SequenceEqual(obj2.Contents)
                && obj1.Count + obj2.Count > 0)
            {
                obj1.Count += obj2.Count;
                obj1.ObjectIndex.Count += obj2.Count;
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
                obj.RemoveFrom();
            else if (obj.InRoom != null)
                obj.InRoom.RemoveFrom(obj);
            else if (obj.InObject != null)
                obj.InObject.RemoveFrom(obj);

            var objContent = obj.Contents.Last();
            if (objContent != null)
                objContent.Extract();

            obj.Affects.Clear();
            obj.ExtraDescriptions.Clear();

            //trworld_obj_check(obj);

            foreach (var relation in db.RELATIONS
                                                .Where(relation => relation.Types == RelationTypes.OSet_On))
            {
                if (obj == relation.Subject)
                    relation.Actor.CastAs<CharacterInstance>().DestinationBuffer = null;
                else
                    continue;
                db.RELATIONS.Remove(relation);
            }

            RepositoryManager.Instance.OBJECTS.CastAs<Repository<long, ObjectInstance>>().Delete(obj.ID);

            handler.queue_extracted_obj(obj);

            obj.ObjectIndex.Count -= obj.Count;

            if (obj != handler.CurrentObject) return;

            handler.CurrentObjectExtracted = true;
            if (handler.GlobalObjectCode == ReturnTypes.None)
                handler.GlobalObjectCode = ReturnTypes.ObjectExtracted;
        }

        public static bool IsTrapped(this ObjectInstance obj)
        {
            return obj.Contents.Any(check => check.ItemType == ItemTypes.Trap);
        }

        public static string GetItemTypeName(this ObjectInstance obj)
        {
            return obj.ItemType.GetName().ToLower();
        }

        public static bool IsInMagicContainer(this ObjectInstance obj)
        {
            if (obj.ItemType == ItemTypes.Container && obj.ExtraFlags.IsSet(ItemExtraFlags.Magical))
                return true;
            return obj.InObject != null && obj.InObject.IsInMagicContainer();
        }

        public static ObjectInstance AddTo(this ObjectInstance obj, CharacterInstance ch)
        {
            var oweight = obj.GetWeight();
            var onum = obj.ObjectNumber;
            var wearLoc = (int)obj.WearLocation;
            var extraFlags = obj.ExtraFlags;

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Prototype) && !ch.IsImmortal() 
                && (!ch.IsNpc() || !ch.Act.IsSet(ActFlags.Prototype)))
                return ch.CurrentRoom.AddTo(obj);

            var skipGroup = false;

            if (handler.LoadingCharacter == ch)
            {
                for (var i = 0; i < GameConstants.MaximumWearLocations; i++)
                {
                    for (var j = 0; j < GameConstants.MaximumWearLayers; j++)
                    {
                        if (ch.IsNpc())
                        {
                            if (obj.MobEq[i, j] != obj) continue;
                            skipGroup = true;
                            break;
                        }

                        if (obj.PlayerEq[i, j] != obj) continue;
                        skipGroup = true;
                        break;
                    }
                }
            }

            if (ch.IsNpc() && ((MobileInstance)ch).MobIndex.Shop != null)
                skipGroup = true;

            ObjectInstance groupObj = null;
            var grouped = false;
            if (!skipGroup)
            {
                foreach (var carriedObj in ch.Carrying)
                {
                    groupObj = carriedObj.GroupWith(obj);
                    if (groupObj != carriedObj) continue;
                    grouped = true;
                    break;
                }
            }

            if (!grouped)
            {
                if (!ch.IsNpc() || ((MobileInstance)ch).MobIndex.Shop == null)
                {
                    ch.Carrying.ToList().Add(obj);
                    obj.CarriedBy = ch;
                    obj.InRoom = null;
                    obj.InObject = null;
                }
                else
                {
                    ObjectInstance foundObj = null;
                    foreach (var carriedObj in ch.Carrying)
                    {
                        if (obj.Level > carriedObj.Level)
                        {
                            ch.Carrying.ToList().Insert(0, carriedObj);
                            foundObj = carriedObj;
                            break;
                        }
                        if (obj.Level != carriedObj.Level || !obj.ShortDescription.Equals(carriedObj.ShortDescription))
                            continue;
                        ch.Carrying.ToList().Insert(0, carriedObj);
                        foundObj = carriedObj;
                        break;
                    }

                    if (foundObj == null)
                        ch.Carrying.ToList().Add(obj);

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

        public static void RemoveFrom(this ObjectInstance obj)
        {
            var ch = obj.CarriedBy;
            if (ch == null)
            {
                LogManager.Instance.Bug("%s: null ch", "obj_from_char");
                return;
            }

            if (obj.WearLocation != WearLocations.None)
                ch.Unequip(obj);

            if (obj.CarriedBy == null)
                return;

            ch.Carrying.ToList().Remove(obj);

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Covering) && obj.Contents != null && obj.Contents.Count > 0)
                obj.Empty();

            obj.InRoom = null;
            obj.CarriedBy = null;
            ch.CarryNumber -= obj.ObjectNumber;
            ch.CarryWeight -= obj.GetWeight();
        }

        public static ObjectInstance AddTo(this ObjectInstance o, ObjectInstance obj)
        {
            if (obj == o)
            {
                LogManager.Instance.Bug("Trying to put object inside itself: vnum {0}", obj.ObjectIndex.Vnum);
                return obj;
            }

            var who = o.CarriedBy;

            if (!o.IsInMagicContainer() && who != null)
                who.CarryWeight += obj.GetWeight();

            foreach (var otmp in o.Contents)
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

        public static void RemoveFrom(this ObjectInstance o, ObjectInstance obj)
        {
            if (obj.InObject != o)
                throw new InvalidDataException(string.Format("Object {0} is not in {1}", obj.ID, o.ID));

            bool magic = o.IsInMagicContainer();

            o.Contents.Remove(obj);

            if (obj.ExtraFlags.IsSet(ItemExtraFlags.Covering) && obj.Contents != null)
                obj.Empty(o);

            obj.InObject = null;
            obj.InRoom = null;
            obj.CarriedBy = null;

            if (!magic)
            {
                var tmp = o;
                do
                {
                    tmp = o.InObject;
                    if (tmp.CarriedBy != null)
                        tmp.CarriedBy.CarryWeight -= obj.GetWeight();
                } while (tmp != null);
            }
        }

        public static ObjectInstance Clone(this ObjectInstance obj)
        {
            var repo =
                (ObjInstanceRepository) Program.Kernel.Get<IInstanceRepository<ObjectInstance>>();
            return repo.Clone(obj);
        }

        public static void Split(this ObjectInstance obj, int number = 1)
        {
            var count = obj.Count;
            if (count <= number || number == 0)
                return;

            ObjectInstance rest = obj.Clone();
            --obj.ObjectIndex.Count;
            rest.Count = obj.Count - number;
            obj.Count = number;

            if (obj.CarriedBy != null)
            {
                obj.CarriedBy.Carrying.ToList().Add(rest);
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
            var ch = obj.CarriedBy;

            if (destobj != null)
                return EmptyInto(obj, destobj);

            if (destroom != null)
                return EmptyInto(ch, obj, destroom);

            if (obj.InObject != null)
                return EmptyInto(obj, obj.InObject);

            if (ch != null)
            {
                var retVal = false;
                foreach (var cobj in obj.Contents)
                {
                    cobj.RemoveFrom(cobj);
                    cobj.AddTo(ch);
                    retVal = true;
                }
                return retVal;
            }

            throw new InvalidOperationException(
                string.Format("Nothing specified to empty the contents of object {0} into", obj.ID));
        }

        private static bool EmptyInto(ObjectInstance obj, ObjectInstance destobj)
        {
            var retVal = false;
            foreach (var cobj in obj.Contents)
            {
                if (destobj.ItemType == ItemTypes.KeyRing && cobj.ItemType != ItemTypes.Key)
                    continue;
                if (destobj.ItemType == ItemTypes.Quiver && cobj.ItemType != ItemTypes.Projectile)
                    continue;
                if ((destobj.ItemType == ItemTypes.Container
                     || destobj.ItemType == ItemTypes.KeyRing
                     || destobj.ItemType == ItemTypes.Quiver)
                    && (cobj.GetRealWeight() + destobj.GetRealWeight() > destobj.Value.ToList()[0]))
                    continue;

                cobj.RemoveFrom(cobj);
                destobj.AddTo(cobj);
                retVal = true;
            }
            return retVal;
        }

        private static bool EmptyInto(CharacterInstance ch, ObjectInstance obj, RoomTemplate destroom)
        {
            var retVal = false;
            foreach (var cobj in obj.Contents)
            {
                if (ch != null && cobj.ObjectIndex.HasProg(MudProgTypes.Drop) && cobj.Count > 1)
                {
                    cobj.Split();
                    cobj.RemoveFrom(cobj);
                }
                else
                    cobj.RemoveFrom(cobj);

                var tObj = destroom.AddTo(cobj);

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
