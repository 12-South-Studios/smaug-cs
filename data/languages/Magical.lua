-- MAGICAL.LUA
-- This is the Magical Language file for the MUD
-- Revised: 2015.08.11
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadLanguage()
	-- emulate say_spell from magic.c
	newLang = LCreateLanguage("magical", "Magical");
	lang.this = newLang;
	lang.this:AddPreConversion("ar", "abra");
	lang.this:AddPreConversion("au", "kada");
	lang.this:AddPreConversion("bless", "fido");
	lang.this:AddPreConversion("blind", "nose");
	lang.this:AddPreConversion("bur", "mosa");
	lang.this:AddPreConversion("cu", "judi");
	lang.this:AddPreConversion("de", "oculo");
	lang.this:AddPreConversion("en", "unso");
	lang.this:AddPreConversion("light", "dies");
	lang.this:AddPreConversion("lo", "hi");
	lang.this:AddPreConversion("mor", "zak");
	lang.this:AddPreConversion("move", "sido");
	lang.this:AddPreConversion("ness", "lacri");
	lang.this:AddPreConversion("ning", "illa");
	lang.this:AddPreConversion("per", "duda");
	lang.this:AddPreConversion("ra", "gru");
	lang.this:AddPreConversion("re", "candus");
	lang.this:AddPreConversion("son", "sabru");
	lang.this:AddPreConversion("tect", "infra");
	lang.this:AddPreConversion("tri", "cula");
	lang.this:AddPreConversion("ven", "nofo");
	
	lang.this.Alphabet = "abqezyopuytrwiasdfghjzxnlk";
end

LoadLanguage();

-- EOF