-- ARAN.LUA
-- This is the zone-file for the Aran
-- Revised: 2015.07.29
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_area.lua")();

function LoadArea()
	LBootLog("=================== AREA 'ARAN' INITIALIZING ===================");
	newArea = LCreateArea(3, "Aran");
	area.this = newArea;
	area.this.Author = "AmonGwareth";
	area.this.HighSoftRange = 60;
	area.this.HighHardRange = 60;
	area.this.HighEconomy = 45009000;
	area.this.ResetFrequency = 60;
	area.this:SetFlags("NoPlayerVsPlayer");
	area.this.ResetMessage = "A horn sounds clear and crisp in the distance...";
	
	if (executeScript(LDataPath() .. "\\areas\\aran\\mobs.lua", true) == false) then 
        LBootLog("Failed to load Aran\\Mobs.lua");
        return;
    end

    if (executeScript(LDataPath() .. "\\areas\\aran\\objects.lua", true) == false) then 
        LBootLog("Failed to load Aran\\Objects.lua");
        return;
    end

    if (executeScript(LDataPath() .. "\\areas\\aran\\rooms.lua", true) == false) then 
        LBootLog("Failed to load Aran\\Rooms.lua");
        return;
    end
	
	LBootLog("=================== AREA 'ARAN' - COMPLETED ================");
end

LoadArea()	-- EXECUTE THE AREA

-- EOF 