-- LIQUIDS.LUA
-- This is the Liquids data file for the MUD
-- Revised: 2013.11.20
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadLiquids()
	liquid = CreateLiquid(0, "water", "water", "clear");
	liquid:SetType("normal");
	liquid:AddMods(0, 1, 10, 0);

	liquid = CreateLiquid(1, "beer", "beer", "amber");
	liquid:SetType("alcohol");
	liquid:AddMods(3, 2, 5, 0);

	liquid = CreateLiquid(2, "wine", "wine", "rose");
	liquid:SetType("alcohol");
	liquid:AddMods(5, 2, 5, 0);

	liquid = CreateLiquid(3, "ale", "ale", "brown");
	liquid:SetType("alcohol");
	liquid:AddMods(2, 2, 5, 0);

	liquid = CreateLiquid(4, "dark ale", "dark ale", "dark brown");
	liquid:SetType("alcohol");
	liquid:AddMods(1, 2, 5, 0);

	liquid = CreateLiquid(5, "whiskey", "whiskey", "golden");
	liquid:SetType("alcohol");
	liquid:AddMods(6, 1, 4, 0);

	liquid = CreateLiquid(6, "lemonade", "lemonade", "yellow");
	liquid:SetType("normal");
	liquid:AddMods(0, 1, 8, 0);

	liquid = CreateLiquid(7, "firebreather", "firebreather", "reddish");
	liquid:SetType("alcohol");
	liquid:AddMods(10, 0, 0, 0);

	liquid = CreateLiquid(8, "local speciality", "local speciality", "clear");
	liquid:SetType("normal");
	liquid:AddMods(3, 3, 3, 0);

	liquid = CreateLiquid(9, "slime mold juice", "slime mold juice", "green");
	liquid:SetType("poison");
	liquid:AddMods(0, 4, -8, 0);

	liquid = CreateLiquid(10, "milk", "milk", "white");
	liquid:SetType("normal");
	liquid:AddMods(0, 3, 6, 0);

	liquid = CreateLiquid(11, "tea", "tea", "light brown");
	liquid:SetType("normal");
	liquid:AddMods(0, 1, 6, 0);

	liquid = CreateLiquid(12, "coffee", "coffee", "brown");
	liquid:SetType("normal");
	liquid:AddMods(0, 1, 6, 0);

	liquid = CreateLiquid(13, "blood", "blood", "red");
	liquid:SetType("blood");
	liquid:AddMods(0, 2, -1, 1);

	liquid = CreateLiquid(14, "salt water", "salt water", "brine");
	liquid:SetType("normal");
	liquid:AddMods(0, 1, -2, 0);

	liquid = CreateLiquid(15, "cola", "cola", "caramel");
	liquid:SetType("normal");
	liquid:AddMods(0, 1, 5, 0);

	liquid = CreateLiquid(16, "mead", "mead", "honey yellow");
	liquid:SetType("alcohol");
	liquid:AddMods(4, 2, 5, 0);

	liquid = CreateLiquid(17, "grog", "grog", "thick brown");
	liquid:SetType("alcohol");
	liquid:AddMods(3, 2, 5, 0);

	liquid = CreateLiquid(18, "osaran ale", "osaran ale", "red");
	liquid:SetType("alcohol");
	liquid:AddMods(5, 2, 2, 0);
end

function CreateLiquid(vnum, name, short_desc, color)
	newLiquid = LCreateLiquid(vnum, name);
	liquid.this = newLiquid;
	liquid.this.ShortDescription = short_desc;
	liquid.this.Color = color;
	return liquid.this;
end

LoadLiquids();

-- EOF 