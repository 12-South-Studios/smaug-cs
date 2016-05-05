-- StatModLookups.LUA
-- Stat Mod Lookup data for the MUD
-- Revised: 2015.12.07
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LAddStrengthMod(value1, value2, value3, value4)
    LAddStatModLookup("Strength", "ToHit", value1);
    LAddStatModLookup("Strength", "ToDam", value2);
    LAddStatModLookup("Strength", "Carry", value3);
    LAddStatModLookup("Strength", "Wield", value4);
end

function LoadStrengthMods()
    LAddStrengthMod(1, -5, -4, 0, 0);
    LAddStrengthMod(2, -5, -4, 3, 1);
    LAddStrengthMod(3, -3, -2, 3, 2);
    LAddStrengthMod(4, -3, -1, 10, 3);
    LAddStrengthMod(5, -2, -1, 55, 5);
    LAddStrengthMod(6, -1, 0, 80, 6);
    LAddStrengthMod(7, -1, 0, 90, 7);
    LAddStrengthMod(8, 0, 0, 100, 8);
    LAddStrengthMod(9, 0, 0, 100, 9);
    LAddStrengthMod(10, 0, 0, 115, 10);
    LAddStrengthMod(11, 0, 0, 115, 11);
    LAddStrengthMod(12, 0, 0, 140, 12);
    LAddStrengthMod(13, 0, 0, 140, 13);
    LAddStrengthMod(14, 0, 1, 170, 14);
    LAddStrengthMod(15, 1, 1, 170, 15);
    LAddStrengthMod(16, 1, 2, 195, 16);
    LAddStrengthMod(17, 2, 3, 220, 22);
    LAddStrengthMod(18, 2, 4, 250, 25);
    LAddStrengthMod(19, 3, 5. 400, 30);
    LAddStrengthMod(20, 3, 6, 500, 35);
    LAddStrengthMod(21, 4, 7, 600, 40);
    LAddStrengthMod(22, 5, 7, 700, 45);
    LAddStrengthMod(23, 6, 8, 800, 50);
    LAddStrengthMod(24, 8, 10, 900, 55);
    LAddStrengthMod(25, 10, 12, 999, 60);
end
LoadStrengthMods();

-- EOF
