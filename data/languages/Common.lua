-- COMMON.LUA
-- This is the Common Language file for the MUD
-- Revised: 2015.08.11
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadLanguage()
	newLang = LCreateLanguage("Common", "Common");
	lang.this = newLang;
	lang.this.Alphabet = "abcdefghijklmnopqrstuvwxyz";
end

LoadLanguage();

-- EOF