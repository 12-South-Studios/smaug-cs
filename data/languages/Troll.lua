-- TROLL.LUA
-- This is the Troll Language file for the MUD
-- Revised: 2015.08.11
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadLanguage()
	-- harsh and primitive, similar to ogre
	newLang = LCreateLanguage("trollese", "Trollish");
	lang.this = newLang;
	lang.this:AddPreConversion("ee", "au");
	lang.this:AddPreConversion("eth", "ok");
	lang.this:AddPreConversion("ith ick", "uk");
	lang.this:AddPreConversion("ath", "ak");
	lang.this:AddPreConversion("uth", "uz");
	lang.this:AddPreConversion("th", "gn");
	lang.this:AddPreConversion("oo", "uu");
	lang.this:AddPreConversion("ar", "arg");
	lang.this:AddPreConversion("en", "yth");
	lang.this:AddPreConversion("ion ul ume", "um");
	lang.this:AddPreConversion("es", "ad");
	lang.this:AddPreConversion("you", "guk");
	lang.this:AddPreConversion("ch", "k");
	lang.this:AddPreConversion("of", "uguf");
	lang.this:AddPreConversion("ove", "umm");
	lang.this:AddPreConversion("my me", "ug");
	lang.this:AddPreConversion("to", "og");
	lang.this:AddPreConversion("hi", "grug");
	lang.this:AddPreConversion("hello", "grog");
	lang.this:AddPreConversion("come", "rawg");
	lang.this:AddPreConversion("kill", "hak");
	lang.this:AddPreConversion("with", "(grunt)");
	lang.this:AddPreConversion("buy take steal", "tog");
	
	lang.this.Alphabet = "ufkdubrgugzpnmugkgztabtzom";
end

LoadLanguage();

-- EOF