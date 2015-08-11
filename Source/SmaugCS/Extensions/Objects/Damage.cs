using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.MudProgs;

namespace SmaugCS.Extensions.Objects
{
    public static class Damage
    {
        public static ReturnTypes CauseDamageTo(this ObjectInstance obj)
        {
            var ch = obj.CarriedBy;
            obj.Split();

            if (!ch.IsNpc() && (!ch.IsPKill() || (ch.IsPKill() && !((PlayerInstance)ch).PlayerData.Flags.IsSet(PCFlags.Gag))))
                comm.act(ATTypes.AT_OBJECT, "($p gets damaged)", ch, obj, null, ToTypes.Character);
            else if (obj.InRoom != null && obj.InRoom.Persons.First() != null)
            {
                ch = obj.InRoom.Persons.First();
                comm.act(ATTypes.AT_OBJECT, "($p gets damaged)", ch, obj, null, ToTypes.Room);
                comm.act(ATTypes.AT_OBJECT, "($p gets damaged)", ch, obj, null, ToTypes.Character);
                ch = null;
            }

            if (obj.ItemType != ItemTypes.Light || !ch.IsInArena())
                MudProgHandler.ExecuteObjectProg(MudProgTypes.Damage, ch, obj);

            if (handler.obj_extracted(obj))
                return handler.GlobalObjectCode;

            ReturnTypes returnVal;

            switch (obj.ItemType)
            {
                default:
                    ObjectFactory.CreateScraps(obj);
                    returnVal = ReturnTypes.ObjectScrapped;
                    break;
                case ItemTypes.Container:
                case ItemTypes.KeyRing:
                case ItemTypes.Quiver:
                    returnVal = CauseDamageToContainer(obj, ch);
                    break;
                case ItemTypes.Light:
                    returnVal = CauseDamageToLight(obj, ch);
                    break;
                case ItemTypes.Armor:
                    returnVal = CauseDamageToArmor(obj, ch);
                    break;
                case ItemTypes.Weapon:
                    returnVal = CauseDamageToWeapon(obj, ch);
                    break;
            }

            if (ch != null)
                save.save_char_obj(ch);

            return returnVal;
        }

        private static ReturnTypes CauseDamageToWeapon(ObjectInstance obj, CharacterInstance ch)
        {
            if (--obj.Values.Condition > 0) return ReturnTypes.None;
            if (!ch.IsPKill() && !ch.IsInArena())
            {
                ObjectFactory.CreateScraps(obj);
                return ReturnTypes.ObjectScrapped;
            }
            obj.Values.Condition = 1;
            return ReturnTypes.None;
        }

        private static ReturnTypes CauseDamageToArmor(ObjectInstance obj, CharacterInstance ch)
        {
            if (ch != null && obj.Values.CurrentAC >= 1)
                ch.ArmorClass += obj.ApplyArmorClass;

            if (--obj.Values.CurrentAC <= 0)
            {
                if (!ch.IsPKill() && !ch.IsInArena())
                {
                    ObjectFactory.CreateScraps(obj);
                    return ReturnTypes.ObjectScrapped;
                }

                obj.Values.CurrentAC = 1;
                ch.ArmorClass -= obj.ApplyArmorClass;
            }
            else if (ch != null && obj.Values.CurrentAC >= 1)
                ch.ArmorClass -= obj.ApplyArmorClass;

            return ReturnTypes.None;
        }

        private static ReturnTypes CauseDamageToLight(ObjectInstance obj, CharacterInstance ch)
        {
            if (--obj.Values.CurrentAC > 0) return ReturnTypes.None;
            if (!ch.IsInArena())
            {
                ObjectFactory.CreateScraps(obj);
                return ReturnTypes.ObjectScrapped;
            }
            obj.Values.CurrentAC = 1;
            return ReturnTypes.None;
        }

        private static ReturnTypes CauseDamageToContainer(ObjectInstance obj, CharacterInstance ch)
        {
            if (--obj.Values.Condition > 0) return ReturnTypes.None;
            if (!ch.IsInArena())
            {
                ObjectFactory.CreateScraps(obj);
                return ReturnTypes.ObjectScrapped;
            }
            obj.Values.Condition = 1;
            return ReturnTypes.None;
        }
    }
}
