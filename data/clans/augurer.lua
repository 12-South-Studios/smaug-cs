-- AUGURER.LUA
-- This is the Augurer Clan file for the MUD
-- Revised: 2013.11.27
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClan()
	newClan = LCreateClan("Guild of Augurers");
	clan.this = newClan;
	clan.this:SetTypeByValue(14);
	clan.this.Class = 7;
	clan.this.Board = 21433;
	clan.this.RecallRoom = 21430;
	clan.this.Storeroom = 21434;
end

LoadClan();

-- EOF