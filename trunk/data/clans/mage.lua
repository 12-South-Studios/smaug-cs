-- MAGE.LUA
-- This is the Mage Clan file for the MUD
-- Revised: 2013.12.03
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClan()
	newClan = LCreateClan("Guild of Mages");
	clan.this = newClan;
	clan.this:SetTypeByValue(14);
	clan.this.Class = 0;
	clan.this.Board = 21228;
	clan.this.RecallRoom = 21127;
	clan.this.Storeroom = 21071;
end

LoadClan();

-- EOF