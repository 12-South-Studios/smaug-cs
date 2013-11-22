using System;
using System.Collections.Generic;
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
        #region LastObject
        private static readonly Dictionary<Type, object> LastObjects = new Dictionary<Type, object>(); 
        public static object LastObject { get; private set; }

        private static void AddLastObject(object obj)
        {
            LastObjects[obj.GetType()] = obj;
            LastObject = obj;
        }
        public static object GetLastObject(Type type)
        {
            return LastObjects.ContainsKey(type) ? LastObjects[type] : LastObject;
        }
        #endregion

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
            AddLastObject(newMudProg);
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
            AddLastObject(newExit);
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
            AddLastObject(newShop);
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
            AddLastObject(newReset);
            return newReset;
        }

        [LuaFunction("LCreateLiquid", "Creates a new Liquid", "Liquid ID", "Liquid Name")]
        public static LiquidData LuaCreateLiquid(int id, string name)
        {
            LiquidData newLiquid = new LiquidData {Vnum = id, Name = name};

            LuaManager.Instance.Proxy.CreateTable("liquid");
            AddLastObject(newLiquid);
            DatabaseManager.Instance.LIQUIDS.Add(newLiquid);
            return newLiquid;
        }

        [LuaFunction("LCreateSkill", "Creates a new skill", "Skill Name", "Skill Type")]
        public static SkillData LuaCreateSkill(string name, string type)
        {
            SkillData newSkill = new SkillData(99, 99)
                {
                    Name = name,
                    Type = EnumerationExtensions.GetEnumIgnoreCase<SkillTypes>(type)
                };

            if (type.EqualsIgnoreCase("herb"))
            {
                DatabaseManager.Instance.HERBS.Add(newSkill);
                LuaManager.Instance.Proxy.CreateTable("herb");
            }
            else
            {
                DatabaseManager.Instance.SKILLS.Add(newSkill);
                LuaManager.Instance.Proxy.CreateTable("skill");
            }

            AddLastObject(newSkill);
            return newSkill;
        }

        [LuaFunction("LSetCode", "Sets the skill function on a skill", "Skill reference", "function")]
        public static void LuaSetCode(SkillData skill, string function)
        {
            Action<CharacterInstance, string> skillFunc = tables.GetSkillFunction(function);
            if (skillFunc != null && skillFunc != tables.SkillNotfound)
            {
                skill.SkillFunctionName = function;
                skill.SkillFunction = new DoFunction {Value = skillFunc};
                return;
            }

            Func<int, int, CharacterInstance, object, ReturnTypes> spellFunc = tables.GetSpellFunction(function);
            if (spellFunc != null && spellFunc != tables.SpellNotfound)
            {
                skill.SpellFunctionName = function;
                skill.SpellFunction = new SpellFunction() {Value = spellFunc};
                return;
            }
        }

        [LuaFunction("LCreateSmaugAffect", "Creates a new Smaug Affect", "duration", "location", "modifier", "flags")]
        public static SmaugAffect LuaCreateSmaugAffect(string duration, int location, string modifier, int flags)
        {
            SmaugAffect newAffect = new SmaugAffect
                {
                    Duration = duration,
                    Location = location,
                    Modifier = modifier,
                    Flags = flags
                };

            LuaManager.Instance.Proxy.CreateTable("affect");
            AddLastObject(newAffect);

            return newAffect;
        }

        [LuaFunction("LCreateSpecFun", "Creates a new special function", "Name of the function")]
        public static SpecialFunction LuaCreateSpecialFunction(string name)
        {
            SpecialFunction newSpecFun = new SpecialFunction
                {
                    Name = name,
                    Value = special.GetSpecFunReference(name)
                };

            LuaManager.Instance.Proxy.CreateTable("specfun");
            AddLastObject(newSpecFun);
            DatabaseManager.Instance.SPEC_FUNS.Add(newSpecFun);
            return newSpecFun;
        }

        [LuaFunction("LCreateCommand", "Creates a new command", "Name of the command", "Command function", "Position",
            "Min Level", "Log Flag", "Flags")]
        public static CommandData LuaCreateCommand(string name, string function, int position, int level, int log,
                                                   int flags)
        {
            CommandData newCommand = new CommandData
                {
                    Name = name,
                    Flags = flags,
                    Position = position,
                    Level = level,
                    Log = log,
                    FunctionName = function,
                    DoFunction = new DoFunction()
                };
            newCommand.Position = newCommand.GetModifiedPosition();
            newCommand.DoFunction.Value = tables.GetSkillFunction(function);

            LuaManager.Instance.Proxy.CreateTable("command");
            AddLastObject(newCommand);
            db.COMMANDS.Add(newCommand);
            return newCommand;
        }

        [LuaFunction("LCreateSocial", "Creates a new Social", "Name of the social")]
        public static SocialData LuaCreateSocial(string name)
        {
            SocialData newSocial = new SocialData() {Name = name};

            LuaManager.Instance.Proxy.CreateTable("social");
            AddLastObject(newSocial);
            db.SOCIALS.Add(newSocial);
            return newSocial;
        }

        [LuaFunction("LCreateSpellComponent", "Creates a new spell component", "Required Component Type",
            "Component Data", "Component Operator Type")]
        public static SpellComponent LuaCreateSpellComponent(string requiredType, string data, string operatorType)
        {
            SpellComponent newComponent = new SpellComponent
                {
                    RequiredType = EnumerationExtensions.GetEnumByName<ComponentRequiredTypes>(requiredType),
                    RequiredData = data
                };

            if (!operatorType.IsNullOrEmpty())
                newComponent.OperatorType = EnumerationExtensions.GetEnumByName<ComponentOperatorTypes>(operatorType);

            LuaManager.Instance.Proxy.CreateTable("component");
            AddLastObject(newComponent);
            return newComponent;
        }
    }
}
