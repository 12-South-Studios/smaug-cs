-- THIEF.LUA
-- This is the Thief Clan file for the MUD
-- Revised: 2013.12.03
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClan()
	newClan = LCreateClan("Guild of Thieves");
	clan.this = newClan;
	clan.this:SetTypeByValue(14);
	clan.this.Class = 2;
	clan.this.Board = 21145;
	clan.this.RecallRoom = 21141;
	clan.this.Storeroom = 21144;
end

LoadClan();

-- EOF