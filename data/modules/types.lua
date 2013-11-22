-- TYPES.LUA
-- Imports all MUD types
-- Revised: 2013.11.09
-- Author: Jason Murdick

luanet.load_assembly("SmaugCS");
luanet.load_assembly("SmaugCS.Common");
luanet.load_assembly("Realm.Library.Common");
luanet.load_assembly("Realm.Library.Lua");
luanet.load_assembly("Realm.Library.NCalcExt");

luanet.import_type("Realm.Library.Common.Cell");
luanet.import_type("Realm.Library.Common.Entity");
luanet.import_type("Realm.Library.Common.IEntity");
luanet.import_type("Realm.Library.Common.Property");

luanet.import_type("SmaugCS.Data");
luanet.import_type("SmaugCS.Data.AreaData");
luanet.import_type("SmaugCS.Data.Template");
luanet.import_type("SmaugCS.Data.Templates.RoomTemplate");
luanet.import_type("SmaugCS.Data.Templates.MobTemplate");
luanet.import_type("SmaugCS.Data.Templates.ObjectTemplate");
luanet.import_type("SmaugCS.Data.Instances.Instance");
luanet.import_type("SmaugCS.Data.Instances.CharacterInstance");
luanet.import_type("SmaugCS.Data.Instances.ObjectInstance");
luanet.import_type("SmaugCS.Data.MudProgData");
luanet.import_type("SmaugCS.Data.ExitData");
luanet.import_type("SmaugCS.Data.ExtraDescriptionData");
luanet.import_type("SmaugCS.Data.LiquidData");
luanet.import_type("SmaugCS.Data.SkillData");

-- EOF