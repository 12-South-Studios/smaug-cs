-- DRUID.LUA
-- This is the Druid Clan file for the MUD
-- Revised: 2013.12.03
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClan()
	newClan = LCreateClan("Guild of Druids");
	clan.this = newClan;
	clan.this:SetTypeByValue(14);
	clan.this.Class = 5;
	clan.this.Board = 21191;
	clan.this.RecallRoom = 21187;
	clan.this.Storeroom = 21188;
end

LoadClan();

-- EOF