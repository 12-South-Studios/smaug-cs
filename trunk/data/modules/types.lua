-- TYPES.LUA
-- Imports all MUD types
-- Revised: 2013.11.25
-- Author: Jason Murdick

luanet.load_assembly("SmaugCS");
luanet.load_assembly("SmaugCS.Common");
luanet.load_assembly("SmaugCS.Constants");
luanet.load_assembly("SmaugCS.Data");
luanet.load_assembly("SmaugCS.Language");
luanet.load_assembly("SmaugCS.Weather");
luanet.load_assembly("Realm.Library.Common");
luanet.load_assembly("Realm.Library.Lua");
luanet.load_assembly("Realm.Library.NCalcExt");

luanet.import_type("Realm.Library.Common.Cell");
luanet.import_type("Realm.Library.Common.Entity");
luanet.import_type("Realm.Library.Common.IEntity");
luanet.import_type("Realm.Library.Common.Property");

luanet.import_type("SmaugCS.Data.AffectData");
luanet.import_type("SmaugCS.Data.AreaData");
luanet.import_type("SmaugCS.Data.AuctionData");
luanet.import_type("SmaugCS.Data.BoardData");
luanet.import_type("SmaugCS.Data.CharacterMorph");
luanet.import_type("SmaugCS.Data.ClassData");
luanet.import_type("SmaugCS.Data.CommandData");
luanet.import_type("SmaugCS.Data.DeityData");
luanet.import_type("SmaugCS.Data.ExitData");
luanet.import_type("SmaugCS.Data.ExtraDescriptionData");
luanet.import_type("SmaugCS.Data.HelpData");
luanet.import_type("SmaugCS.Data.HintData");
luanet.import_type("SmaugCS.Data.HolidayData");
luanet.import_type("SmaugCS.Data.LiquidData");
luanet.import_type("SmaugCS.Data.MixtureData");
luanet.import_type("SmaugCS.Data.MorphData");
luanet.import_type("SmaugCS.Data.MudProgActData");
luanet.import_type("SmaugCS.Data.MudProgData");
luanet.import_type("SmaugCS.Data.NoteData");
luanet.import_type("SmaugCS.Data.PlaneData");
luanet.import_type("SmaugCS.Data.ProjectData");
luanet.import_type("SmaugCS.Data.RaceData");
luanet.import_type("SmaugCS.Data.ResetData");
luanet.import_type("SmaugCS.Data.SkillData");
luanet.import_type("SmaugCS.Data.SmaugAffect");
luanet.import_type("SmaugCS.Data.SocialData");
luanet.import_type("SmaugCS.Data.SpellComponent");
luanet.import_type("SmaugCS.Data.TeleportData");
luanet.import_type("SmaugCS.Data.VariableData");
luanet.import_type("SmaugCS.Data.WeatherData");

luanet.import_type("SmaugCS.Data.Template");
luanet.import_type("SmaugCS.Data.Templates.RoomTemplate");
luanet.import_type("SmaugCS.Data.Templates.MobTemplate");
luanet.import_type("SmaugCS.Data.Templates.ObjectTemplate");

luanet.import_type("SmaugCS.Data.Instances.Instance");
luanet.import_type("SmaugCS.Data.Instances.CharacterInstance");
luanet.import_type("SmaugCS.Data.Instances.ObjectInstance");

-- EOF