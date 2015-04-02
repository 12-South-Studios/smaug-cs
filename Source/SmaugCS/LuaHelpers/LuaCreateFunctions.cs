using System;
using System.Collections.Generic;
using Realm.Library.Common;
using Realm.Library.Lua;
using Realm.Library.Patterns.Command;
using SmaugCS.Common.Enumerations;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Organizations;
using SmaugCS.Data.Shops;
using SmaugCS.Interfaces;
using SmaugCS.Language;
using SmaugCS.Logging;
using SmaugCS.SpecFuns;

namespace SmaugCS.LuaHelpers
{
    public static class LuaCreateFunctions
    {
        private static ILuaManager _luaManager;
        private static IDatabaseManager _dbManager;
        private static ILogManager _logManager;

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

        public static void InitializeReferences(ILuaManager luaManager, IDatabaseManager dbManager,
            ILogManager logManager)
        {
            _luaManager = luaManager;
            _dbManager = dbManager;
            _logManager = logManager;
        }

        [LuaFunction("LCreateMudProg", "Creates a new mudprog", "Type of Prog")]
        public static MudProgData LuaCreateMudProg(string progType)
        {
            var newMudProg = new MudProgData
            {
                Type = EnumerationExtensions.GetEnumByName<MudProgTypes>(progType)
            };
            _luaManager.Proxy.CreateTable("mprog");
            AddLastObject(newMudProg);
            return newMudProg;
        }

        [LuaFunction("LCreateExit", "Creates a new Exit", "Exit Direction", "Exit Destination", "Exit Name")]
        public static ExitData LuaCreateExit(string direction, long destination, string name)
        {
            var dir = EnumerationExtensions.GetEnumIgnoreCase<DirectionTypes>(direction);
            var newExit = new ExitData((int)dir, name)
            {
                Destination = destination,
                Direction = dir,
                Keywords = direction
            };
            _luaManager.Proxy.CreateTable("exit");
            AddLastObject(newExit);

            _logManager.Boot("Exit '{0}' created in direction {1} to room {2}", name, direction, destination);
            return newExit;
        }

        [LuaFunction("LCreateShop", "Creates a new Shop", "Shop Buy Rate", "Shop Sell Rate", "Shop Open Hour",
            "Shop Close Hour")]
        public static ShopData LuaCreateShop(int buyRate, int sellRate, int openHour, int closeHour)
        {
            var newShop = new ItemShopData
                                       {
                                           ShopType = ShopTypes.Item,
                                           OpenHour = openHour,
                                           CloseHour = closeHour,
                                           ProfitBuy = buyRate,
                                           ProfitSell = sellRate
                                       };

            _luaManager.Proxy.CreateTable("shop");
            AddLastObject(newShop);
            return newShop;
        }

        [LuaFunction("LCreateReset", "Creates a new Reset", "Reset Type", "Extra", "Arg1", "Arg2", "Arg3")]
        public static ResetData LuaCreateReset(string resetType, int extra, int arg1, int arg2, int arg3)
        {
            var newReset = new ResetData
            {
                Type = EnumerationExtensions.GetEnumIgnoreCase<ResetTypes>(resetType),
                Extra = extra
            };
            newReset.SetArgs(arg1, arg2, arg3);

            _luaManager.Proxy.CreateTable("reset");
            AddLastObject(newReset);

            _logManager.Boot("Reset '{0}' created", resetType);
            return newReset;
        }

        [LuaFunction("LCreateLiquid", "Creates a new Liquid", "Liquid ID", "Liquid Name")]
        public static LiquidData LuaCreateLiquid(int id, string name)
        {
            var newLiquid = new LiquidData(id, name);

            _luaManager.Proxy.CreateTable("liquid");
            AddLastObject(newLiquid);
            _dbManager.AddToRepository(newLiquid);

            _logManager.Boot("Liquid (id={0}, name={1}) created", id, name);
            return newLiquid;
        }

        [LuaFunction("LCreateSkill", "Creates a new skill", "ID of the skill", "Skill Name", "Skill Type")]
        public static SkillData LuaCreateSkill(int id, string name, string type)
        {
            var newSkill = new SkillData(id, name)
            {
                Type = EnumerationExtensions.GetEnumIgnoreCase<SkillTypes>(type)
            };

            if (type.EqualsIgnoreCase("herb"))
                throw new InvalidOperationException(string.Format("Use of LCreateSkill for Herbs is deprecated"));
            
            _dbManager.AddToRepository(newSkill);
            _luaManager.Proxy.CreateTable("skill");

            _logManager.Boot("Skill (id={0}, name={1}) created", id, name);
            AddLastObject(newSkill);
            return newSkill;
        }

        [LuaFunction("LCreateHerb", "Creates a new herb", "ID of the herb", "Herb Name", "Herb Type")]
        public static HerbData LuaCreateHerb(int id, string name, string type)
        {
            var newHerb = new HerbData(id, name) { Type = EnumerationExtensions.GetEnumIgnoreCase<SkillTypes>(type) };

            if (!type.EqualsIgnoreCase("herb"))
                throw new InvalidOperationException(string.Format("Use of LCreateHerb for Non-Herbs is not supported"));

            _dbManager.AddToRepository(newHerb);
            _luaManager.Proxy.CreateTable("herb");

            _logManager.Boot("Herb (id={0}, name={1}) created", id, name);
            AddLastObject(newHerb);
            return newHerb;
        }

        [LuaFunction("LCreateSmaugAffect", "Creates a new Smaug Affect", "duration", "location", "modifier", "flags")]
        public static SmaugAffect LuaCreateSmaugAffect(string duration, int location, string modifier, int flags)
        {
            var newAffect = new SmaugAffect
                {
                    Duration = duration,
                    Location = location,
                    Modifier = modifier,
                    Flags = flags
                };

            _luaManager.Proxy.CreateTable("affect");
            AddLastObject(newAffect);

            return newAffect;
        }

        [LuaFunction("LCreateSpecFun", "Creates a new special function", "Name of the function")]
        public static SpecialFunction LuaCreateSpecialFunction(string name)
        {
            var newSpecFun = new SpecialFunction(_dbManager.GenerateNewId<SpecialFunction>(),
                                                             name) {Value = SpecFunHandler.GetSpecFunReference(name)};

            if (newSpecFun.Value == null)
                throw new EntryNotFoundException("SpecFun {0} not found", name);

            _luaManager.Proxy.CreateTable("specfun");
            AddLastObject(newSpecFun);
            _dbManager.AddToRepository(newSpecFun);

            return newSpecFun;
        }

        [LuaFunction("LCreateCommand", "Creates a new command", "Name of the command", "Command function", "Position",
            "Min Level", "Log Flag", "Flags")]
        public static CommandData LuaCreateCommand(string name, string function, int position, int level, int log,
                                                   int flags)
        {
            var newCommand = new CommandData(_dbManager.GenerateNewId<CommandData>(), name)
                {
                    Flags = flags,
                    Position = position,
                    Level = level,
                    Log = EnumerationExtensions.GetEnum<LogAction>(log),
                    FunctionName = function
                };
            newCommand.Position = newCommand.ModifiedPosition;

            _luaManager.Proxy.CreateTable("command");
            AddLastObject(newCommand);
            _dbManager.AddToRepository(newCommand);

            _logManager.Boot("Command '{0}' created", name);
            return newCommand;
        }

        [LuaFunction("LCreateSocial", "Creates a new Social", "Name of the social")]
        public static SocialData LuaCreateSocial(string name)
        {
            var newSocial = new SocialData(_dbManager.GenerateNewId<SocialData>(), name);

            _luaManager.Proxy.CreateTable("social");
            AddLastObject(newSocial);
            _dbManager.AddToRepository(newSocial);

            _logManager.Boot("Social {0} created", name);
            return newSocial;
        }

        [LuaFunction("LCreateSpellComponent", "Creates a new spell component", "Required Component Type",
            "Component Data", "Component Operator Type")]
        public static SpellComponent LuaCreateSpellComponent(string requiredType, string data, string operatorType)
        {
            var newComponent = new SpellComponent
                {
                    RequiredType = EnumerationExtensions.GetEnumByName<ComponentRequiredTypes>(requiredType),
                    RequiredData = data
                };

            if (!operatorType.IsNullOrEmpty())
                newComponent.OperatorType = EnumerationExtensions.GetEnumByName<ComponentOperatorTypes>(operatorType);

            _luaManager.Proxy.CreateTable("component");
            AddLastObject(newComponent);
            return newComponent;
        }

        [LuaFunction("LCreateClass", "Creates a new class", "Name of the class", "Numeric type of the class")]
        public static ClassData LuaCreateClass(string name, int type)
        {
            var newClass = new ClassData(_dbManager.GenerateNewId<ClassData>(), name)
                {
                    Type = EnumerationExtensions.GetEnum<ClassTypes>(type)
                };

            _luaManager.Proxy.CreateTable("class");
            AddLastObject(newClass);
            _dbManager.AddToRepository(newClass);

            _logManager.Boot("Class '{0}' created", name);
            return newClass;
        }

        [LuaFunction("LCreateRace", "Creates a new Race", "Name of the Race", "Numeric type of the race")]
        public static RaceData LuaCreateRace(string name, int type)
        {
            var newRace = new RaceData(_dbManager.GenerateNewId<RaceData>(), name)
                {
                    Type = EnumerationExtensions.GetEnum<RaceTypes>(type)
                };

            _luaManager.Proxy.CreateTable("race");
            AddLastObject(newRace);
            _dbManager.AddToRepository(newRace);

            _logManager.Boot("Race '{0}' created", name);
            return newRace;
        }

        [LuaFunction("LCreateClan", "Creates a new Clan", "Name of the clan")]
        public static ClanData LuaCreateClan(string name)
        {
            var newClan = new ClanData(_dbManager.GenerateNewId<ClanData>(), name);

            _luaManager.Proxy.CreateTable("clan");
            AddLastObject(newClan);
            _dbManager.AddToRepository(newClan);

            _logManager.Boot("Clan '{0}' created", name);
            return newClan;
        }

        [LuaFunction("LCreateDeity", "Creates a new Deity", "Name of the Deity")]
        public static DeityData LuaCreateDeity(string name)
        {
            var newDeity = new DeityData(_dbManager.GenerateNewId<DeityData>(), name);

            _luaManager.Proxy.CreateTable("deity");
            AddLastObject(newDeity);
            _dbManager.AddToRepository(newDeity);

            _logManager.Boot("Deity '{0}' created", name);
            return newDeity;
        }

        [LuaFunction("LCreateLanguage", "Creates a new Language", "Name of the language", "Language Type")]
        public static LanguageData LuaCreateLanguage(string name, string type)
        {
            var newLang = new LanguageData(_dbManager.GenerateNewId<LanguageData>(), name,
                                                    EnumerationExtensions.GetEnumIgnoreCase<LanguageTypes>(type));

            _luaManager.Proxy.CreateTable("lang");
            AddLastObject(newLang);
            _dbManager.AddToRepository(newLang);

            _logManager.Boot("Language '{0}' created", name);
            return newLang;
        }
    }
}
