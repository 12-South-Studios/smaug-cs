using System.Linq;
using LuaInterface;
using Realm.Library.Lua;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.LuaHelpers
{
    public static class LuaMudProgFunctions
    {
        [LuaFunction("LIsCarrying", "Gets true if the character is carrying the object", "Character", "Object", "Id to Check")]
        public static bool LuaIsCarrying(CharacterInstance ch, ObjectInstance obj, int id)
        {
            if (ch == null)
                throw new LuaException("Character cannot be null");
            if (id <= 0)
                throw new LuaException("Id cannot be less than or equal to zero");

            if (obj == null)
                return ch.Carrying.Any(x => x.ID == id);

            if (obj.WearLocation == WearLocations.None && (obj.ObjectIndex.ID == id || obj.ID == id))
                return true;

            return obj.Contents.Any(x => LuaIsCarrying(ch, x, id));
        }
    }
}
