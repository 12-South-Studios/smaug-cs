-- WARRIOR.LUA
-- This is the Warrior Clan file for the MUD
-- Revised: 2013.12.03
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClan()
	newClan = LCreateClan("Guild of Warriors");
	clan.this = newClan;
	clan.this:SetTypeByValue(14);
	clan.this.Class = 3;
	clan.this.Board = 21239;
	clan.this.RecallRoom = 21236;
	clan.this.Storeroom = 21240;
end

LoadClan();

-- EOF