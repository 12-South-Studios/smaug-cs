-- LIQUIDS.LUA
-- This is the Liquids data file for the MUD
-- Revised: 2013.11.20
-- Author: Jason Murdick
-- Version: 1.0

newLiquid = LCreateLiquid(0, "water");
liquid.this = newLiquid;
liquid.this.ShortDescription = "water";
liquid.this.Color = "clear";
liquid.this:SetType("normal");
liquid.this:AddMods(0, 1, 10, 0);

newLiquid = LCreateLiquid(1, "beer");
liquid.this = newLiquid;
liquid.this.ShortDescription = "beer";
liquid.this.Color = "amber";
liquid.this:SetType("alcohol");
liquid.this:AddMods(3, 2, 5, 0);

newLiquid = LCreateLiquid(2, "wine");
liquid.this = newLiquid;
liquid.this.ShortDescription = "wine";
liquid.this.Color = "rose";
liquid.this:SetType("alcohol");
liquid.this:AddMods(5, 2, 5, 0);

newLiquid = LCreateLiquid(3, "ale");
liquid.this = newLiquid;
liquid.this.ShortDescription = "ale";
liquid.this.Color = "brown";
liquid.this:SetType("alcohol");
liquid.this:AddMods(2, 2, 5, 0);

newLiquid = LCreateLiquid(4, "dark ale");
liquid.this = newLiquid;
liquid.this.ShortDescription = "dark ale";
liquid.this.Color = "dark brown";
liquid.this:SetType("alcohol");
liquid.this:AddMods(1, 2, 5, 0);

newLiquid = LCreateLiquid(5, "whiskey");
liquid.this = newLiquid;
liquid.this.ShortDescription = "whiskey";
liquid.this.Color = "golden";
liquid.this:SetType("alcohol");
liquid.this:AddMods(6, 1, 4, 0);

newLiquid = LCreateLiquid(6, "lemonade");
liquid.this = newLiquid;
liquid.this.ShortDescription = "lemonade";
liquid.this.Color = "yellow";
liquid.this:SetType("normal");
liquid.this:AddMods(0, 1, 8, 0);

newLiquid = LCreateLiquid(7, "firebreather");
liquid.this = newLiquid;
liquid.this.ShortDescription = "firebreather";
liquid.this.Color = "reddish";
liquid.this:SetType("alcohol");
liquid.this:AddMods(10, 0, 0, 0);

newLiquid = LCreateLiquid(8, "local speciality");
liquid.this = newLiquid;
liquid.this.ShortDescription = "local speciality";
liquid.this.Color = "clear";
liquid.this:SetType("normal");
liquid.this:AddMods(3, 3, 3, 0);

newLiquid = LCreateLiquid(9, "slime mold juice");
liquid.this = newLiquid;
liquid.this.ShortDescription = "slime mold juice";
liquid.this.Color = "green";
liquid.this:SetType("poison");
liquid.this:AddMods(0, 4, -8, 0);

newLiquid = LCreateLiquid(10, "milk");
liquid.this = newLiquid;
liquid.this.ShortDescription = "milk";
liquid.this.Color = "white";
liquid.this:SetType("normal");
liquid.this:AddMods(0, 3, 6, 0);

newLiquid = LCreateLiquid(11, "tea");
liquid.this = newLiquid;
liquid.this.ShortDescription = "tea";
liquid.this.Color = "light brown";
liquid.this:SetType("normal");
liquid.this:AddMods(0, 1, 6, 0);

newLiquid = LCreateLiquid(12, "coffee");
liquid.this = newLiquid;
liquid.this.ShortDescription = "coffee";
liquid.this.Color = "brown";
liquid.this:SetType("normal");
liquid.this:AddMods(0, 1, 6, 0);

newLiquid = LCreateLiquid(13, "blood");
liquid.this = newLiquid;
liquid.this.ShortDescription = "blood";
liquid.this.Color = "red";
liquid.this:SetType("blood");
liquid.this:AddMods(0, 2, -1, 1);

newLiquid = LCreateLiquid(14, "salt water");
liquid.this = newLiquid;
liquid.this.ShortDescription = "salt water";
liquid.this.Color = "brine";
liquid.this:SetType("normal");
liquid.this:AddMods(0, 1, -2, 0);

newLiquid = LCreateLiquid(15, "cola");
liquid.this = newLiquid;
liquid.this.ShortDescription = "cola";
liquid.this.Color = "caramel";
liquid.this:SetType("normal");
liquid.this:AddMods(0, 1, 5, 0);

newLiquid = LCreateLiquid(16, "mead");
liquid.this = newLiquid;
liquid.this.ShortDescription = "mead";
liquid.this.Color = "honey yellow";
liquid.this:SetType("alcohol");
liquid.this:AddMods(4, 2, 5, 0);

newLiquid = LCreateLiquid(17, "grog");
liquid.this = newLiquid;
liquid.this.ShortDescription = "grog";
liquid.this.Color = "thick brown";
liquid.this:SetType("alcohol");
liquid.this:AddMods(3, 2, 5, 0);

newLiquid = LCreateLiquid(18, "osaran ale");
liquid.this = newLiquid;
liquid.this.ShortDescription = "osaran ale";
liquid.this.Color = "red";
liquid.this:SetType("alcohol");
liquid.this:AddMods(5, 2, 2, 0);

-- EOF 