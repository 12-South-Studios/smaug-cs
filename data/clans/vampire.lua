-- VAMPIRE.LUA
-- This is the Vampire Clan file for the MUD
-- Revised: 2013.12.03
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadClan()
	newClan = LCreateClan("Guild of Vampires");
	clan.this = newClan;
	clan.this:SetTypeByValue(14);
	clan.this.Class = 4;
	clan.this.Board = 21136;
	clan.this.RecallRoom = 21135;
	clan.this.Storeroom = 21139;
end

LoadClan();

-- EOF