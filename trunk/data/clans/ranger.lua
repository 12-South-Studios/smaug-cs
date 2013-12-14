-- RANGER.LUA
-- This is the Ranger Clan file for the MUD
-- Revised: 2013.12.03
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClan()
	newClan = LCreateClan("Guild of Rangers");
	clan.this = newClan;
	clan.this:SetTypeByValue(14);
	clan.this.Class = 6;
	clan.this.Board = 21212;
	clan.this.RecallRoom = 21208;
	clan.this.Storeroom = 21210;
end

LoadClan();

-- EOF