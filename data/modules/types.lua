-- TYPES.LUA
-- Imports all MUD types
-- Revised: 2015.04.02
-- Author: Jason Murdick

-- Namespaces
luanet.load_assembly("SmaugCS");
luanet.load_assembly("SmaugCS.Auction");
luanet.load_assembly("SmaugCS.Ban");
luanet.load_assembly("SmaugCS.Board");
luanet.load_assembly("SmaugCS.Common");
luanet.load_assembly("SmaugCS.Communication");
luanet.load_assembly("SmaugCS.Config");
luanet.load_assembly("SmaugCS.Constants");
luanet.load_assembly("SmaugCS.DAL");
luanet.load_assembly("SmaugCS.Data");
luanet.load_assembly("SmaugCS.Language");
luanet.load_assembly("SmaugCS.Logging");
luanet.load_assembly("SmaugCS.News");
luanet.load_assembly("SmaugCS.Weather");
luanet.load_assembly("Realm.Library.Ai");
luanet.load_assembly("Realm.Library.Common");
luanet.load_assembly("Realm.Library.Controls");
luanet.load_assembly("Realm.Library.Database");
luanet.load_assembly("Realm.Library.Lua");
luanet.load_assembly("Realm.Library.NCalcExt");
luanet.load_assembly("Realm.Library.Network");
luanet.load_assembly("Realm.Library.Patterns.Command");
luanet.load_assembly("Realm.Library.Patterns.Decorator");
luanet.load_assembly("Realm.Library.Patterns.Factory");
luanet.load_assembly("Realm.Library.Patterns.Repository");
luanet.load_assembly("Realm.Library.Patterns.Singleton");
luanet.load_assembly("Realm.Library.SmallDb");
luanet.load_assembly("Realm.Library.XML");

-- Objects
luanet.import_type("Realm.Library.Common.ICell");
luanet.import_type("Realm.Library.Common.Entity");
luanet.import_type("Realm.Library.Common.IEntity");
luanet.import_type("Realm.Library.Common.Property");
luanet.import_type("Realm.Library.Common.TinyTypeBase");
luanet.import_type("Realm.Library.Common.ILogWrapper");

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
luanet.import_type("SmaugCS.Data.RoomTemplate");
luanet.import_type("SmaugCS.Data.MobTemplate");
luanet.import_type("SmaugCS.Data.ObjectTemplate");

luanet.import_type("SmaugCS.Data.Instance");
luanet.import_type("SmaugCS.Data.CharacterInstance");
luanet.import_type("SmaugCS.Data.ObjectInstance");

luanet.import_type("SmaugCS.Logging.ILogManager");

-- EOF