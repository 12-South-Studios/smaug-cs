
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Library.Lua;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Shops;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS
{
    public static class LuaFunctions
    {
        public static object LastObject { get; private set; }

        [LuaFunction("LGetLastEntity", "Retrieves the Last Entity")]
        public static object LuaGetLastEntity()
        {
            return LastObject;
        }

        [LuaFunction("LGetRoom", "Retrieves a room with a given ID", "ID of the room")]
        public static RoomTemplate LuaGetRoom(long id)
        {
            return DatabaseManager.Instance.ROOMS.Get(id);
        }

        [LuaFunction("LGetMobile", "Retrieves a mob with a given ID", "ID of the mobile")]
        public static MobTemplate LuaGetMobile(long id)
        {
            return DatabaseManager.Instance.MOBILE_INDEXES.Get(id);
        }

        [LuaFunction("LGetObject", "Retrieves an object with a given ID", "ID of the object")]
        public static ObjectTemplate LuaGetObject(long id)
        {
            return DatabaseManager.Instance.OBJECT_INDEXES.Get(id);
        }

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

        [LuaFunction("LDataPath", "Retrieves the game's data path")]
        public static string LuaGetDataPath()
        {
            return Program.GetDataPath();
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

        [LuaFunction("LCreateMudProg", "Creates a new mudprog", "Type of Prog")]
        public static MudProgData LuaCreateMudProg(string progType)
        {
            MudProgData newMudProg = new MudProgData { Type = EnumerationExtensions.GetEnumByName<MudProgTypes>(progType) };
            LuaManager.Instance.Proxy.CreateTable("mprog");
            LastObject = newMudProg;
            return newMudProg;
        }

        [LuaFunction("LCreateExit", "Creates a new Exit", "Exit Direction", "Exit Destination", "Exit Name")]
        public static ExitData LuaCreateExit(string direction, long destination, string name)
        {
            DirectionTypes dir = EnumerationExtensions.GetEnumIgnoreCase<DirectionTypes>(direction);
            ExitData newExit = new ExitData((int)dir, name)
                                   {
                                       Destination = destination,
                                       Direction = dir,
                                       Keywords = direction
                                   };
            LuaManager.Instance.Proxy.CreateTable("exit");
            LastObject = newExit;
            return newExit;
        }

        [LuaFunction("LCreateShop", "Creates a new Shop", "Shop Buy Rate", "Shop Sell Rate", "Shop Open Hour",
            "Shop Close Hour")]
        public static ShopData LuaCreateShop(int buyRate, int sellRate, int openHour, int closeHour)
        {
            ItemShopData newShop = new ItemShopData
                                       {
                                           ShopType = ShopTypes.Item,
                                           OpenHour = openHour,
                                           CloseHour = closeHour,
                                           ProfitBuy = buyRate,
                                           ProfitSell = sellRate
                                       };

            LuaManager.Instance.Proxy.CreateTable("shop");
            LastObject = newShop;
            return newShop;
        }

        [LuaFunction("LCreateReset", "Creates a new Reset", "Reset Type", "Extra", "Arg1", "Arg2", "Arg3")]
        public static ResetData LuaCreateReset(string resetType, int extra, int arg1, int arg2, int arg3)
        {
            ResetData newReset = new ResetData
                                     {
                                         Type = EnumerationExtensions.GetEnumIgnoreCase<ResetTypes>(resetType),
                                         Extra = extra
                                     };
            newReset.SetArgs(arg1, arg2, arg3);

            LuaManager.Instance.Proxy.CreateTable("reset");
            LastObject = newReset;
            return newReset;
        }
    }
}
