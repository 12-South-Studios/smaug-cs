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

luanet.import_type("SmaugCS.Objects");
luanet.import_type("SmaugCS.Objects.AreaData");
luanet.import_type("SmaugCS.Objects.Template");
luanet.import_type("SmaugCS.Objects.RoomTemplate");
luanet.import_type("SmaugCS.Objects.MobTemplate");
luanet.import_type("SmaugCS.Objects.ObjectTemplate");
luanet.import_type("SmaugCS.Objects.Instance");
luanet.import_type("SmaugCS.Objects.CharacterInstance");
luanet.import_type("SmaugCS.Objects.ObjectInstance");

-- EOF