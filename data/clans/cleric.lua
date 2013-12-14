-- CLERIC.LUA
-- This is the Cleric Clan file for the MUD
-- Revised: 2013.12.03
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClan()
	newClan = LCreateClan("Guild of Clerics");
	clan.this = newClan;
	clan.this:SetTypeByValue(14);
	clan.this.Class = 1;
	clan.this.Board = 21172;
	clan.this.RecallRoom = 21177;
	clan.this.Storeroom = 21178;
end

LoadClan();

-- EOF