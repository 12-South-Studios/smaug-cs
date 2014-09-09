using System.Collections.Generic;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Interfaces;
using SmaugCS.Managers;

namespace SmaugCS.Extensions
{
    public static class ObjectLocator
    {
        public static ObjectInstance GetObjectOfType(this CharacterInstance ch, ItemTypes type)
        {
            return ch.Carrying.FirstOrDefault(obj => obj.ItemType == type);
        }

        public static ObjectInstance GetCarriedObject(this CharacterInstance ch, string argument)
        {
            int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
            string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

            IEnumerable<ObjectInstance> items =
                ch.Carrying.Where(obj => obj.WearLocation == WearLocations.None && ch.CanSee(obj));

            int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
            return vnum > 0
                ? GetObjectInList(items.Where(obj => obj.ObjectIndex.ID == vnum), number)
                : GetObjectInList(items.Where(obj => arg.IsAnyEqual(obj.Name) || arg.IsAnyEqualPrefix(obj.Name)), number);
        }

        public static ObjectInstance GetObjectOnMeOrInRoom(this CharacterInstance ch, string argument)
        {
            return GetObjectInReversedList(ch.CurrentRoom.Contents, argument)
                ?? ch.GetCarriedObject(argument)
                ?? GetWornObject(ch, argument);
        }

        public static ObjectInstance GetWornObject(this CharacterInstance ch, string argument)
        {
            int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
            string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

            IEnumerable<ObjectInstance> items =
                ch.Carrying.Where(obj => obj.WearLocation != WearLocations.None && ch.CanSee(obj));

            int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
            return vnum > 0
                ? GetObjectInList(items.Where(obj => obj.ObjectIndex.ID == vnum), number)
                : GetObjectInList(items.Where(obj => arg.IsAnyEqual(obj.Name) || arg.IsAnyEqualPrefix(obj.Name)), number);
        }

        public static ObjectInstance GetObjectInWorld(this CharacterInstance ch, string argument,
            IDatabaseManager dbManager = null)
        {
            int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
            string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

            IEnumerable<ObjectInstance> items = (dbManager ?? DatabaseManager.Instance).OBJECTS.Values.Where(ch.CanSee);

            int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
            return vnum > 0
                ? GetObjectInList(items.Where(obj => obj.ObjectIndex.ID == vnum), number)
                : GetObjectInList(items.Where(obj => arg.IsAnyEqual(obj.Name) || arg.IsAnyEqualPrefix(obj.Name)), number);
        }

        public static CharacterInstance GetCharacterInRoom(this CharacterInstance ch, string argument)
        {
            int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
            string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

            if (arg.EqualsIgnoreCase("self"))
                return ch;

            IEnumerable<CharacterInstance> chars = ch.CurrentRoom.Persons.Where(ch.CanSee);

            int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
            return vnum > 0
                ? GetObjectInList(chars.Where(vch => vch.IsNpc() && ((MobileInstance)vch).MobIndex.ID == vnum), number)
                : GetObjectInList(chars.Where(vch => arg.IsAnyEqual(vch.Name) || arg.IsAnyEqualPrefix(vch.Name)),
                    number);
        }

        public static CharacterInstance GetCharacterInWorld(this CharacterInstance ch, string argument,
            IDatabaseManager dbManager = null)
        {
            int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
            string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

            if (arg.EqualsIgnoreCase("self"))
                return ch;

            IEnumerable<CharacterInstance> chars =
                (dbManager ?? DatabaseManager.Instance).CHARACTERS.Values.Where(ch.CanSee);

            int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
            return vnum > 0
                ? GetObjectInList(chars.Where(vch => vch.IsNpc() && ((MobileInstance)vch).MobIndex.ID == vnum), number)
                : GetObjectInList(chars.Where(vch => arg.IsAnyEqual(vch.Name) || arg.IsAnyEqualPrefix(vch.Name)),
                    number);
        }

        private static T GetObjectInReversedList<T>(IEnumerable<T> items, string argument) where T : Cell
        {
            List<T> reversedList = new List<T>();
            reversedList.AddRange(items);
            reversedList.Reverse();

            return GetObjectInList(reversedList
                .Where(obj => argument.IsAnyEqual(obj.Name) || argument.IsAnyEqualPrefix(obj.Name)), 1);
        }

        public static ObjectInstance GetObjectInList(this CharacterInstance ch, IEnumerable<ObjectInstance> objects,
            string argument)
        {
            int number = argument.IsNumberArgument() ? argument.GetNumberArgument() : 1;
            string arg = argument.IsNumberArgument() ? argument.StripNumberArgument() : argument;

            int vnum = GetVnumFromArgumentIfImmortal(ch, arg);
            return vnum > 0
                ? GetObjectInList(objects.Where(obj => obj.ObjectIndex.ID == vnum), number)
                : GetObjectInList(objects.Where(obj => arg.IsAnyEqual(obj.Name) || arg.IsAnyEqualPrefix(obj.Name)),
                    number);
        }

        private static T GetObjectInList<T>(IEnumerable<T> objects, int number) where T : Cell
        {
            int count = 0;
            foreach (T obj in objects)
            {
                count++;
                if (count >= number)
                    return obj;
            }
            return null;
        }

        private static int GetVnumFromArgumentIfImmortal(CharacterInstance ch, string arg)
        {
            return (ch.Trust >= LevelConstants.GetLevel(ImmortalTypes.Savior) && arg.IsNumber()) ? arg.ToInt32() : -1;
        }
    }
}
