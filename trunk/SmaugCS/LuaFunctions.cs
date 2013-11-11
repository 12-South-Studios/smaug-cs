
using System.Linq;
using Realm.Library.Common.Extensions;
using Realm.Library.Lua;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class LuaFunctions
    {
        [LuaFunction("CheckNumber", "Validates a vnum for range", "Number to check")]
        public static bool LuaCheckNumber(int vnum)
        {
            if (vnum < 1 || vnum > Program.MAX_VNUM)
            {
                LogManager.Bug("Vnum {0} is out of range (1 to {1})", vnum, Program.MAX_VNUM);
                return false;
            }

            return true;
        }

        [LuaFunction("FindInstance", "Locates a character matching the given name", "Instance executing this search", "String argument")]
        public static CharacterInstance LuaFindCharacter(CharacterInstance instance, string arg)
        {
            if (arg.EqualsIgnoreCase("self"))
                return instance;

            return
                instance.CurrentRoom.Persons.FirstOrDefault(
                    vch => handler.can_see(instance, vch) && !vch.IsNpc() && vch.Name.EqualsIgnoreCase(arg)) ??
                DatabaseManager.Instance.CHARACTERS.Values.FirstOrDefault(
                    vch => handler.can_see(instance, vch) && !vch.IsNpc() && vch.Name.EqualsIgnoreCase(arg));
        }

        public static MobTemplate LuaCreateMobile(long id, string name)
        {
            DatabaseManager.Instance.MOBILE_INDEXES.Create(id, name);
            return null;
        }
    }
}
